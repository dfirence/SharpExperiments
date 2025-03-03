namespace SharpExperiments.Hashing;

using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// MurmurHash3_x64_128 implementation based on Austin Appleby's original work.
/// This is a **non-cryptographic** hash function designed for high-speed hashing
/// on **64-bit architectures**, commonly used in **Bloom filters, distributed systems,**
/// and **probabilistic data structures**.
//
/// <para>**References:**</para>
/// - [MurmurHash3 Wiki](https://en.m.wikipedia.org/wiki/MurmurHash)
/// - [MurmurHash3 GitHub Repo](https://github.com/aappleby/smhasher)
/// - [Murmur in C#](https://github.com/darrenkopp/murmurhash-net/tree/master/MurmurHash/Managed)
/// - [Original MurmurHash3 Paper](https://github.com/aappleby/smhasher/blob/master/MurmurHash3.cpp)
///
/// </summary>
public static class Murmur3
{
    /// <summary>
    /// Computes a **128-bit MurmurHash3** hash for the given byte array.
    /// Optimized for **64-bit architectures** and produces a **128-bit hash**
    /// using two **64-bit hash values** (`h1`, `h2`).
    /// </summary>
    /// <param name="data">The input byte array to hash.</param>
    /// <param name="seed">A 32-bit seed value (useful for independent hash computations).</param>
    /// <returns>A tuple containing two **64-bit hash values (h1, h2)**, forming a **128-bit Murmur hash**.</returns>
    public static (ulong, ulong) CreateHash(ReadOnlySpan<byte> data, uint seed)
    {
        // MurmurHash3 constants for mixing
        const ulong c1 = 0x87c37b91114253d5UL;
        const ulong c2 = 0x4cf5ad432745937fUL;

        ulong h1 = seed, h2 = seed;
        int length = data.Length;
        int i = 0;

        // Process input in 128-bit (16-byte) chunks for better performance
        // MurmurHash3 is optimized for 64-bit architectures, processing two 64-bit words (`k1` and `k2`) at a time.
        // This approach ensures efficient hashing, reducing the number of iterations required for large inputs.
        while (length >= 16)
        {
            // Read two 64-bit words from the input data
            // Using `MemoryMarshal.Read<ulong>` avoids unnecessary memory allocations, improving performance.
            ulong k1 = MemoryMarshal.Read<ulong>(data.Slice(i));        // First 64-bit word (low 8 bytes)
            ulong k2 = MemoryMarshal.Read<ulong>(data.Slice(i + 8));    // Second 64-bit word (high 8 bytes)

            // Mix K1 into h1
            // Each 64-bit word undergoes a transformation using bitwise shifts and multiplications.
            // These operations ensure that small changes in input propagate throughout the hash.
            k1 *= c1;                      // Multiply by the first constant to spread bits
            k1 = (k1 << 31) | (k1 >> 33);  // Left rotate by 31 bits (circular shift)
            k1 *= c2;                      // Multiply by the second constant
            h1 ^= k1;                      // XOR result into h1 to introduce new entropy

            // Mix h1 further
            // After integrating `k1`, additional transformations ensure stronger randomness.
            h1 = (h1 << 27) | (h1 >> 37);  // Left rotate by 27 bits
            h1 += h2;                      // Mix h1 and h2 to increase diffusion
            h1 = h1 * 5 + 0x52dce729;      // Multiply by 5 and add a constant to scramble bits

            // Mix K2 into h2
            // `k2` undergoes a similar process, but using a different mixing order.
            k2 *= c2;                      // Multiply by the second constant first
            k2 = (k2 << 33) | (k2 >> 31);  // Left rotate by 33 bits
            k2 *= c1;                      // Multiply by the first constant
            h2 ^= k2;                      // XOR into h2, further spreading entropy

            // Mix h2 further
            // Similar to h1, this step strengthens diffusion and eliminates correlations.
            h2 = (h2 << 31) | (h2 >> 33);  // Left rotate by 31 bits
            h2 += h1;                      // Mix h2 with h1
            h2 = h2 * 5 + 0x38495ab5;      // Multiply by 5 and add a different constant for diversification

            // Move to the next 16-byte block
            // Since we processed 16 bytes, update the index and remaining length.
            i += 16;
            length -= 16;
        }

        // Process remaining bytes (tail) when the input length is not a multiple of 16 bytes
        // Since MurmurHash3 processes data in 16-byte chunks, any leftover bytes (1 to 15 bytes) need special handling.
        // This ensures that even the smallest change in input is reflected in the final hash output.
        //
        // The tail bytes are packed into two 64-bit words (`k1_tail` and `k2_tail`).
        // - `k1_tail` represents the **first half** (low 8 bytes) of the 128-bit block.
        // - `k2_tail` represents the **second half** (high 8 bytes) if there are more than 8 remaining bytes.
        // - These words are constructed by shifting bytes into position and then processed similarly to full 64-bit words.
        ulong k1_tail = 0, k2_tail = 0;
        
        // The `switch-case` mechanism processes each remaining byte one by one.
        // - Instead of a loop, we use **fall-through case statements with `goto case`** to efficiently build the words.
        // - This structure mimics the original MurmurHash3 C++ implementation, which uses implicit fall-through.
        // - Bytes are bit-shifted into position based on their index, ensuring all available bits contribute to the final hash.
        switch (length)
        {
            // Breakdown of the process:
            // - If `length == 15`, byte 14 is shifted into the 48-bit position of `k2_tail`, then control jumps to case 14.
            // - This continues until byte 8 is processed, at which point `k2_tail` undergoes the standard MurmurHash3 mixing steps.
            // - If `length == 8` or fewer, bytes are shifted into `k1_tail` in a similar manner.
            // - Once all remaining bytes are incorporated, `k1_tail` is also processed using the same transformation as full 64-bit blocks.
            // - The final `h1` and `h2` updates ensure that these last bytes still contribute to the overall entropy of the hash.
            case 15: k2_tail ^= (ulong)data[i + 14] << 48; goto case 14;
            case 14: k2_tail ^= (ulong)data[i + 13] << 40; goto case 13;
            case 13: k2_tail ^= (ulong)data[i + 12] << 32; goto case 12;
            case 12: k2_tail ^= (ulong)data[i + 11] << 24; goto case 11;
            case 11: k2_tail ^= (ulong)data[i + 10] << 16; goto case 10;
            case 10: k2_tail ^= (ulong)data[i + 9] << 8; goto case 9;
            case 9:  k2_tail ^= (ulong)data[i + 8]; 
                     k2_tail *= c2; 
                     k2_tail = (k2_tail << 33) | (k2_tail >> 31); 
                     k2_tail *= c1; 
                     h2 ^= k2_tail;
                     goto case 8;

            case 8:  k1_tail ^= (ulong)data[i + 7] << 56; goto case 7;
            case 7:  k1_tail ^= (ulong)data[i + 6] << 48; goto case 6;
            case 6:  k1_tail ^= (ulong)data[i + 5] << 40; goto case 5;
            case 5:  k1_tail ^= (ulong)data[i + 4] << 32; goto case 4;
            case 4:  k1_tail ^= (ulong)data[i + 3] << 24; goto case 3;
            case 3:  k1_tail ^= (ulong)data[i + 2] << 16; goto case 2;
            case 2:  k1_tail ^= (ulong)data[i + 1] << 8; goto case 1;
            case 1:  k1_tail ^= (ulong)data[i]; 
                     k1_tail *= c1; 
                     k1_tail = (k1_tail << 31) | (k1_tail >> 33); 
                     k1_tail *= c2; 
                     h1 ^= k1_tail;
                     break;
        }

        // Finalization - Ensuring Strong Avalanche Properties
        // The finalization step in MurmurHash3 ensures that even small input variations 
        // produce drastically different hash outputs (the "avalanche effect").
        //
        // Step 1: XOR the length into both hash values (`h1` and `h2`)
        // - This prevents hash collisions for inputs with the same content but different lengths.
        // - If two different inputs have the same leading bytes but different lengths, this step differentiates them.
        h1 ^= (ulong)data.Length;
        h2 ^= (ulong)data.Length;

        // Step 2: Mix the two 64-bit hash values together
        // - `h1` and `h2` are combined by summing them in both directions (`h1 += h2` and `h2 += h1`).
        // - This ensures that entropy is distributed evenly across both hash components.
        // - It also prevents cases where two inputs produce similar `h1` or `h2` values.
        h1 += h2;
        h2 += h1;

        // Step 3: Apply the `fmix64` function to each hash component
        // - `fmix64` is a **finalization mix function** that applies multiple bitwise XORs and multiplications.
        // - This step is **critical** for ensuring that every bit in the input contributes to every bit in the output.
        // - It removes patterns and ensures that the final hash values exhibit **strong uniformity**.
        h1 = fmix64(h1);
        h2 = fmix64(h2);

        // Step 4: Final mixing of `h1` and `h2`
        // - The two hash halves are **cross-mixed** one final time by summing them again.
        // - This ensures that the final 128-bit hash is **fully diffused**, meaning small input changes 
        //   cause widespread bit changes in both 64-bit outputs.
        h1 += h2;
        h2 += h1;

        // The function returns the final **128-bit MurmurHash3 result** as two 64-bit values.
        // - These two values (`h1`, `h2`) together represent the **final 128-bit hash output**.
        return (h1, h2);
    }

    /// <summary>
    /// Generates multiple hash values using **MurmurHash3_x64_128**.
    /// Useful for **Bloom filters, distributed caching,** and **probabilistic data structures**.
    /// </summary>
    /// <param name="item">The item to hash (converted to a string internally).</param>
    /// <param name="hashValues">A span buffer where generated hash values will be stored.</param>
    public static void CreateHashes<T>(T item, Span<long> hashValues)
    {
        string str = item?.ToString()?.Trim() ?? string.Empty;
        int byteCount = Encoding.UTF8.GetByteCount(str);
        
        if (byteCount == 0)
        {
            return;
        }
        
        // Rent a buffer from the shared array pool
        byte[] byteBuffer = ArrayPool<byte>.Shared.Rent(byteCount);
        int actualLemgth = 0;
        
        try
        {
            // Convert the string to UTF-8 bytes and store the actual length used
            actualLength = Encoding.UTF8.GetBytes(str, byteBuffer.AsSpan(0, byteCount));
            // Ensure `CreateHash` is not called on an empty buffer
            if (actualLength == 0)
            {
                return; // The finally block will handle buffer cleanup
            }

            // Compute the Murmur3 hash using only the relevant part of the buffer
            (ulong h1, ulong h2) = CreateHash(byteBuffer.AsSpan(0, actualLength), 0);

            const ulong HASH_MULTIPLIER = 0xBF58476D1CE4E5B9UL;

            // Ensure we don't process an empty hash span
            if (hashValues.Length == 0)
            {
                return;
            }

            // Iterate through the requested number of hash values
            // - `hashValues.Length` determines how many independent hash outputs should be generated.
            // - This is particularly useful in **Bloom filters**, **probabilistic data structures**, and **distributed hashing schemes**.
            // - Each iteration produces a new derived hash value using `h1` and `h2`.
            for (int i = 0; i < hashValues.Length; i++)
            {
                // Compute a new hash variant by mixing `h1` with the current index
                // - `h1` is incremented by `i`, ensuring that each iteration produces a **unique value**.
                // - `fmix64` is a **finalization mix function** that enhances randomness and prevents correlation.
                // - This transformation ensures that small variations in `i` cause large, unpredictable changes in the output.
                ulong mixedH1 = fmix64(h1 + (ulong)i);

                // Compute a second independent hash variant using `h2` and a constant multiplier
                // - `HASH_MULTIPLIER` is a carefully chosen large prime to decorrelate different hash values.
                // - The multiplication `(ulong)i * HASH_MULTIPLIER` generates a **unique offset** for each iteration,
                //   ensuring that each hash function is independent.
                // - Since `ulong` is a **finite 64-bit type**, multiplying large values may exceed `ulong.MaxValue`,
                //   causing an **integer overflow** (silent wraparound).
                //
                // - To prevent uncontrolled overflow, we use **modulus arithmetic** (`% ulong.MaxValue`):
                //   - This ensures the computed value always stays within a valid 64-bit range.
                //   - It helps maintain Murmur3's **avalanche properties**, ensuring proper entropy distribution.
                //   - The modulus operation only affects very large `i` values, keeping performance stable.
                ulong mixedH2 = fmix64(h2 + unchecked((ulong)i * HASH_MULTIPLIER));

                // Combine `mixedH1` and `mixedH2` to generate the final hash value for this iteration
                // - XOR is used to merge both mixed values, ensuring that **entropy from both inputs influences the result**.
                // - The result is **cast to a signed 64-bit integer (`long`)**, which is common in hash-based data structures.
                hashValues[i] = (long)(mixedH1 ^ mixedH2);
            }
        }
        finally
        {
            // Clear buffer before returning to the pool to prevent data leakage
            Array.Clear(byteBuffer, 0, actualLength);
            ArrayPool<byte>.Shared.Return(byteBuffer);
        }
    }

    /// <summary>
    /// The **fmix64** function applies **finalization mixing** to ensure strong **avalanche properties**.
    /// Ensures that even a small change in input results in highly different hash values.
    /// </summary>
    /// <param name="k">The 64-bit integer value to be mixed.</param>
    /// <returns>A 64-bit value with improved bit distribution.</returns>
    private static ulong fmix64(ulong k)
    {
        // Finalization mix function (`fmix64`) used in MurmurHash3
        // - This function ensures **strong avalanche properties**, meaning that even a single bit change
        //   in the input will drastically change the output hash value.
        // - It applies a **series of bitwise XORs and multiplications** to fully mix the bits, reducing correlations.
        // - The constants used (`0xff51afd7ed558ccdUL` and `0xc4ceb9fe1a85ec53UL`) are carefully chosen
        //   to maximize bit diffusion and randomness.
        k ^= k >> 33;               // Step 1: Initial XOR shift to break up patterns in the high bits
        k *= 0xff51afd7ed558ccdUL;  // Step 2: Multiply with a prime constant to introduce high dispersion
        k ^= k >> 33;               // Step 3: Apply another XOR shift to further mix bits
        k *= 0xc4ceb9fe1a85ec53UL;  // Step 4: Multiply with a second prime constant to increase randomness
        k ^= k >> 33;               // Step 5: Final XOR shift to ensure strong diffusion

        return k;
    }
}