namespace SharpExperiments.Hashing;
using System.Text;
using System;

public static class FNV1a64
{
    // Constants for 64-bit FNV-1a
    private const ulong FNV64OffsetBasis = 14695981039346656037;
    private const ulong FNV64Prime = 1099511628211;

    /// <summary>
    /// Creates a 64-bit FNV-1a hash from a ReadOnlySpan<byte>.
    /// </summary>
    /// <param name="data">The input byte data.</param>
    /// <returns>The 64-bit hash value.</returns>
    public static ulong CreateHash(ReadOnlySpan<byte> data)
    {
        ulong hash = FNV64OffsetBasis;

        foreach (byte b in data)
        {
            hash ^= b;                  // XOR byte with hash
            hash *= FNV64Prime;         // Multiply by prime
        }

        return hash;
    }
}