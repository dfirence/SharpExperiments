/***Status: DRAFT ***/

namespace SharpExperiments.Hashing;

using System;
using System.Runtime.CompilerServices;

/// <summary>
/// <para>XXHash64 - A fast non-cryptographic hash function.</para>
/// <para>**References:**</para>
/// - [Main - xxHash](https://xxhash.com/)
/// - [Github - XXHash](https://github.com/Cyan4973/xxHash)
/// </summary>
public static class XXHash64
{
    private const ulong Prime64_1 = 11400714785074694791UL;
    private const ulong Prime64_2 = 14029467366897019727UL;
    private const ulong Prime64_3 = 1609587929392839161UL;
    private const ulong Prime64_4 = 9650029242287828579UL;
    private const ulong Prime64_5 = 2870177450012600261UL;

    /// <summary>
    /// Computes the XXHash64 for a given byte array.
    /// </summary>
    /// <param name="data">The input data.</param>
    /// <param name="seed">An optional seed (default is 0).</param>
    /// <returns>The 64-bit hash.</returns>
    public static ulong CreateHash(byte[] data, ulong seed = 0)
    {
        return CreateHash(new ReadOnlySpan<byte>(data), seed);
    }

    /// <summary>
    /// Computes the XXHash64 for a given ReadOnlySpan<byte> (more efficient, avoids allocations).
    /// </summary>
    /// <param name="data">The input data as a span.</param>
    /// <param name="seed">An optional seed (default is 0).</param>
    /// <returns>The 64-bit hash.</returns>
    public static ulong CreateHash(ReadOnlySpan<byte> data, ulong seed = 0)
    {
        int length = data.Length;
        ulong hash;

        if (length >= 32)
        {
            ulong v1 = seed + Prime64_1 + Prime64_2;
            ulong v2 = seed + Prime64_2;
            ulong v3 = seed;
            ulong v4 = seed - Prime64_1;

            int end32 = length - 32;
            int i = 0;

            // Process in 32-byte blocks
            while (i <= end32)
            {
                v1 = Round(v1, ReadUInt64(data, i));
                v2 = Round(v2, ReadUInt64(data, i + 8));
                v3 = Round(v3, ReadUInt64(data, i + 16));
                v4 = Round(v4, ReadUInt64(data, i + 24));
                i += 32;
            }

            hash = RotateLeft(v1, 1) + RotateLeft(v2, 7) + RotateLeft(v3, 12) + RotateLeft(v4, 18);
            hash = MergeRound(hash, v1);
            hash = MergeRound(hash, v2);
            hash = MergeRound(hash, v3);
            hash = MergeRound(hash, v4);
        }
        else
        {
            // Optimize small input handling
            hash = seed + Prime64_5 + (ulong)length;
        }

        // Improved 8-byte block processing (for non-multiple-of-32 inputs)
        int offset = 0;
        while (length >= 8)
        {
            hash = MergeRound(hash, ReadUInt64(data, offset));
            offset += 8;
            length -= 8;
        }

        // Process remaining bytes (< 8 bytes)
        if (length > 0)
        {
            ulong extra = 0;
            for (int i = 0; i < length; i++)
            {
                extra |= ((ulong)data[offset + i]) << (i * 8);
            }
            hash ^= Round(0, extra);
        }

        // Final mixing to ensure good hash distribution
        hash = FinalMix(hash);
        return hash;
    }

    /// <summary>
    /// Reads a 64-bit unsigned integer safely from a ReadOnlySpan<byte>.
    /// Prevents out-of-bounds access by padding with zeros if needed.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong ReadUInt64(ReadOnlySpan<byte> data, int offset)
    {
        if (offset + 8 > data.Length)
        {
            ulong result = 0;
            int remaining = data.Length - offset;
            for (int i = 0; i < remaining; i++)
            {
                result |= ((ulong)data[offset + i]) << (i * 8);
            }
            return result;
        }
        return BitConverter.ToUInt64(data.Slice(offset, 8));
    }

    /// <summary>
    /// Core XXHash round function to process input data.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Round(ulong acc, ulong input)
    {
        acc += input * Prime64_2;
        acc = RotateLeft(acc, 31);
        acc *= Prime64_1;
        return acc;
    }

    /// <summary>
    /// Merges accumulator values to reduce hash collisions.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong MergeRound(ulong acc, ulong val)
    {
        val = Round(0, val);
        acc ^= val;
        acc = acc * Prime64_1 + Prime64_4;
        return acc;
    }

    /// <summary>
    /// Performs a left bitwise rotation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong RotateLeft(ulong value, int count)
    {
        return (value << count) | (value >> (64 - count));
    }

    /// <summary>
    /// Final mixing stage to scramble the hash and ensure good distribution.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong FinalMix(ulong hash)
    {
        hash ^= hash >> 33;
        hash *= Prime64_2;
        hash ^= hash >> 29;
        hash *= Prime64_3;
        hash ^= hash >> 32;
        return hash;
    }
}