namespace SharpExperiments.Hashing;

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Buffers.Binary;

#if NET7_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

/// <summary>
/// Optimized Murmur3_x64_128 Hash Implementation (Version 2).
/// - Uses **SIMD acceleration** on .NET 7+.
/// - Avoids **unnecessary memory allocations** using `stackalloc`.
/// - Uses **alignment-safe memory reads** via `BinaryPrimitives`.
/// </summary>
public static class Murmur3_V2
{
    private static readonly (ulong h1, ulong h2) s_default_invalid = (ulong.MinValue, ulong.MinValue);
    private static readonly string c_null_murmur3_string = "00000000000000000000000000000000";

    public static ulong GetXorHash(string value, uint seed = 0)
    {
        if (string.IsNullOrEmpty(value))
        {
            return s_default_invalid.h1;
        }

        (ulong h1, ulong h2) = HashItem(value, seed);
        return (h1 == s_default_invalid.h1 && h2 == s_default_invalid.h2) ? s_default_invalid.h1 : h1 ^ h2;
    }

    public static string GetStringHash(string value, uint seed = 0)
    {
        if (string.IsNullOrEmpty(value))
        {
            return c_null_murmur3_string;
        }

        (ulong h1, ulong h2) = HashItem(value, seed);
        return (h1 == s_default_invalid.h1 && h2 == s_default_invalid.h2) ? c_null_murmur3_string : $"{h1:X16}{h2:X16}";
    }

    public static (ulong, ulong) HashItem(string value, uint seed = 0)
    {
        if (string.IsNullOrEmpty(value))
        {
            return s_default_invalid;
        }

        int byteCount = Encoding.UTF8.GetByteCount(value);
        Span<byte> buffer = byteCount <= 128 ? stackalloc byte[byteCount] : new byte[byteCount];
        Encoding.UTF8.GetBytes(value, buffer);

        return CreateHash(buffer, seed);
    }

    public static (ulong, ulong) CreateHash(ReadOnlySpan<byte> data, uint seed = 0)
    {
        const ulong c1 = 0x87c37b91114253d5UL;
        const ulong c2 = 0x4cf5ad432745937fUL;

        ulong h1 = seed, h2 = seed;
        int length = data.Length;
        int i = 0;

        #if NET7_0_OR_GREATER
        // SIMD-accelerated processing for .NET 7+
        if (Avx2.IsSupported && length >= 16)
        {
            while (length >= 16)
            {
                Vector128<ulong> v1 = Unsafe.ReadUnaligned<Vector128<ulong>>(ref MemoryMarshal.GetReference(data.Slice(i)));
                Vector128<ulong> v2 = Unsafe.ReadUnaligned<Vector128<ulong>>(ref MemoryMarshal.GetReference(data.Slice(i + 8)));

                v1 = Avx2.Multiply(v1, Vector128.Create(c1));
                v1 = Avx2.ShiftLeftLogical(v1, 31) | Avx2.ShiftRightLogical(v1, 33);
                v1 = Avx2.Multiply(v1, Vector128.Create(c2));
                h1 ^= v1.ToScalar();

                v2 = Avx2.Multiply(v2, Vector128.Create(c2));
                v2 = Avx2.ShiftLeftLogical(v2, 33) | Avx2.ShiftRightLogical(v2, 31);
                v2 = Avx2.Multiply(v2, Vector128.Create(c1));
                h2 ^= v2.ToScalar();

                i += 16;
                length -= 16;
            }
        }
        #else
        // Non-SIMD fallback for .NET 6 and earlier
        while (length >= 16)
        {
            ulong k1 = BinaryPrimitives.ReadUInt64LittleEndian(data.Slice(i));
            ulong k2 = BinaryPrimitives.ReadUInt64LittleEndian(data.Slice(i + 8));

            k1 *= c1;
            k1 = (k1 << 31) | (k1 >> 33);
            k1 *= c2;
            h1 ^= k1;
            h1 = (h1 << 27) | (h1 >> 37);
            h1 += h2;
            h1 = h1 * 5 + 0x52dce729;

            k2 *= c2;
            k2 = (k2 << 33) | (k2 >> 31);
            k2 *= c1;
            h2 ^= k2;
            h2 = (h2 << 31) | (h2 >> 33);
            h2 += h1;
            h2 = h2 * 5 + 0x38495ab5;

            i += 16;
            length -= 16;
        }
        #endif

        // Process remaining tail bytes
        ulong k1_tail = 0, k2_tail = 0;
        for (int j = length - 1; j >= 0; j--)
        {
            k1_tail ^= (ulong)data[i + j] << (j * 8);
        }

        k1_tail *= c1;
        k1_tail = (k1_tail << 31) | (k1_tail >> 33);
        k1_tail *= c2;
        h1 ^= k1_tail;

        h1 ^= (ulong)data.Length;
        h2 ^= (ulong)data.Length;
        h1 += h2;
        h2 += h1;

        h1 = fmix64(h1);
        h2 = fmix64(h2);

        h1 += h2;
        h2 += h1;

        return (h1, h2);
    }

    private static ulong fmix64(ulong k)
    {
        k ^= k >> 33;
        k *= 0xff51afd7ed558ccdUL;
        k ^= k >> 31;
        k *= 0xc4ceb9fe1a85ec53UL;
        k ^= k >> 30;

        return k;
    }
}