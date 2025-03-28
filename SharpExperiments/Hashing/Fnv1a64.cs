namespace SharpExperiments.Hashing;
using System.Text;

public static class FNV1a64
{
    // Constants for 64-bit FNV-1a
    private const ulong FNV64OffsetBasis = 14695981039346656037;
    private const ulong FNV64Prime = 1099511628211;

    public static ulong CreateHash(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new ArgumentException("Data cannot be null or empty");
        } 

        byte[] bytes = Encoding.UTF8.GetBytes(data);
        ulong hash = FNV64OffsetBasis;

        foreach (byte b in bytes)
        {
            hash ^= b;
            hash *= FNV64Prime;
        }

        return hash;
    }
}