// | Method                     | Job      | Runtime  | UrlLength | Mean        | Error     | StdDev    | Median      | Allocated |
// |--------------------------- |--------- |--------- |---------- |------------:|----------:|----------:|------------:|----------:|
// | Murmur3_UNICODE_CreateHash | .NET 6.0 | .NET 6.0 | 50        |    78.58 ns |  1.831 ns |  5.400 ns |    80.05 ns |         - |
// | Murmur3_UTF32_CreateHash   | .NET 6.0 | .NET 6.0 | 50        |    71.80 ns |  1.368 ns |  1.142 ns |    71.32 ns |         - |
// | Murmur3_UTF8_CreateHash    | .NET 6.0 | .NET 6.0 | 50        |    21.28 ns |  0.119 ns |  0.111 ns |    21.28 ns |         - |
// | Murmur3_ASCII_CreateHash   | .NET 6.0 | .NET 6.0 | 50        |    21.25 ns |  0.161 ns |  0.151 ns |    21.28 ns |         - |
// | Murmur3_UNICODE_CreateHash | .NET 8.0 | .NET 8.0 | 50        |    36.43 ns |  0.243 ns |  0.227 ns |    36.49 ns |         - |
// | Murmur3_UTF32_CreateHash   | .NET 8.0 | .NET 8.0 | 50        |    74.00 ns |  0.429 ns |  0.401 ns |    73.91 ns |         - |
// | Murmur3_UTF8_CreateHash    | .NET 8.0 | .NET 8.0 | 50        |    19.97 ns |  0.064 ns |  0.057 ns |    19.97 ns |         - |
// | Murmur3_ASCII_CreateHash   | .NET 8.0 | .NET 8.0 | 50        |    20.13 ns |  0.162 ns |  0.143 ns |    20.12 ns |         - |
// | Murmur3_UNICODE_CreateHash | .NET 6.0 | .NET 6.0 | 200       |    80.50 ns |  0.318 ns |  0.282 ns |    80.48 ns |         - |
// | Murmur3_UTF32_CreateHash   | .NET 6.0 | .NET 6.0 | 200       |   155.93 ns |  1.245 ns |  1.164 ns |   155.64 ns |         - |
// | Murmur3_UTF8_CreateHash    | .NET 6.0 | .NET 6.0 | 200       |    41.11 ns |  0.103 ns |  0.096 ns |    41.13 ns |         - |
// | Murmur3_ASCII_CreateHash   | .NET 6.0 | .NET 6.0 | 200       |    41.18 ns |  0.107 ns |  0.095 ns |    41.21 ns |         - |
//
// | Murmur3_UNICODE_CreateHash | .NET 8.0 | .NET 8.0 | 200       |    83.06 ns |  0.472 ns |  0.441 ns |    83.02 ns |         - |
// | Murmur3_UTF32_CreateHash   | .NET 8.0 | .NET 8.0 | 200       |   153.27 ns |  0.636 ns |  0.564 ns |   153.12 ns |         - |
// | Murmur3_UTF8_CreateHash    | .NET 8.0 | .NET 8.0 | 200       |    39.76 ns |  0.174 ns |  0.163 ns |    39.71 ns |         - |
// | Murmur3_ASCII_CreateHash   | .NET 8.0 | .NET 8.0 | 200       |    40.18 ns |  0.482 ns |  0.402 ns |    40.04 ns |         - |
// | Murmur3_UNICODE_CreateHash | .NET 6.0 | .NET 6.0 | 1000      |   313.16 ns |  1.623 ns |  1.439 ns |   312.60 ns |         - |
// | Murmur3_UTF32_CreateHash   | .NET 6.0 | .NET 6.0 | 1000      |   624.67 ns |  4.399 ns |  3.674 ns |   625.24 ns |         - |
// | Murmur3_UTF8_CreateHash    | .NET 6.0 | .NET 6.0 | 1000      |   157.85 ns |  0.791 ns |  0.739 ns |   157.82 ns |         - |
// | Murmur3_ASCII_CreateHash   | .NET 6.0 | .NET 6.0 | 1000      |   157.55 ns |  1.090 ns |  1.019 ns |   157.44 ns |         - |
// | Murmur3_UNICODE_CreateHash | .NET 8.0 | .NET 8.0 | 1000      |   316.57 ns |  4.982 ns |  6.118 ns |   314.36 ns |         - |
// | Murmur3_UTF32_CreateHash   | .NET 8.0 | .NET 8.0 | 1000      |   624.92 ns |  2.024 ns |  1.795 ns |   624.85 ns |         - |
// | Murmur3_UTF8_CreateHash    | .NET 8.0 | .NET 8.0 | 1000      |   159.73 ns |  0.657 ns |  0.548 ns |   159.69 ns |         - |
// | Murmur3_ASCII_CreateHash   | .NET 8.0 | .NET 8.0 | 1000      |   154.98 ns |  0.392 ns |  0.328 ns |   155.03 ns |         - |
// | Murmur3_UNICODE_CreateHash | .NET 6.0 | .NET 6.0 | 5000      | 1,473.56 ns |  6.559 ns |  6.135 ns | 1,471.83 ns |         - |
// | Murmur3_UTF32_CreateHash   | .NET 6.0 | .NET 6.0 | 5000      | 2,926.34 ns | 12.570 ns | 11.758 ns | 2,924.87 ns |         - |
// | Murmur3_UTF8_CreateHash    | .NET 6.0 | .NET 6.0 | 5000      |   743.57 ns |  3.804 ns |  3.372 ns |   743.90 ns |         - |
// | Murmur3_ASCII_CreateHash   | .NET 6.0 | .NET 6.0 | 5000      |   741.51 ns |  1.983 ns |  1.757 ns |   741.56 ns |         - |
// | Murmur3_UNICODE_CreateHash | .NET 8.0 | .NET 8.0 | 5000      | 1,466.28 ns |  6.837 ns |  6.395 ns | 1,464.72 ns |         - |
// | Murmur3_UTF32_CreateHash   | .NET 8.0 | .NET 8.0 | 5000      | 2,950.16 ns | 15.375 ns | 13.629 ns | 2,945.31 ns |         - |
// | Murmur3_UTF8_CreateHash    | .NET 8.0 | .NET 8.0 | 5000      |   743.28 ns |  2.849 ns |  2.379 ns |   743.85 ns |         - |
// | Murmur3_ASCII_CreateHash   | .NET 8.0 | .NET 8.0 | 5000      |   737.07 ns |  3.814 ns |  3.568 ns |   736.22 ns |         - |

namespace SharpExperiments.Benchmarks.Hashing;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SharpExperiments.Hashing;

/// <summary>
/// Benchmarking Murmur3 hashing with different string encodings and varying URL lengths.
/// </summary>
[SimpleJob(RuntimeMoniker.Net60)] // Run the benchmark on .NET 6.0
[SimpleJob(RuntimeMoniker.Net80)] // Run the benchmark on .NET 8.0
[MemoryDiagnoser] // Track memory allocations during benchmarking
public class Hashing_Murmur3_Benchmarks
{
    // Byte arrays to store encoded versions of the test URL
    private byte[]? _unicodeBytes;
    private byte[]? _utf32Bytes;
    private byte[]? _utf8Bytes;
    private byte[]? _asciiBytes;

    /// <summary>
    /// Defines different URL lengths for testing.
    /// Small (50 chars), Medium (200 chars), Large (1000 chars), and X-Large (5000 chars).
    /// BenchmarkDotNet will run tests for each length.
    /// </summary>
    [Params(50, 200, 1000, 5000)]
    public int UrlLength;

    /// <summary>
    /// Generates test URLs and encodes them in multiple formats before running benchmarks.
    /// Runs once before each benchmark iteration.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        // Generate a sample URL dynamically based on the requested size
        string testUrl = GenerateTestUrl(UrlLength);

        // Convert the URL to different encodings for benchmarking
        _unicodeBytes = Encoding.Unicode.GetBytes(testUrl);  // UTF-16
        _asciiBytes = Encoding.ASCII.GetBytes(testUrl);      // ASCII (if possible)
        _utf32Bytes = Encoding.UTF32.GetBytes(testUrl);      // UTF-32
        _utf8Bytes = Encoding.UTF8.GetBytes(testUrl);        // UTF-8
    }

    /// <summary>
    /// Dynamically generates a test URL with a specific character length.
    /// The URL consists of a domain, query parameters, and randomly generated characters.
    /// </summary>
    /// <param name="length">Desired length of the generated URL.</param>
    /// <returns>A formatted test URL string.</returns>
    private string GenerateTestUrl(int length)
    {
        // Base URL domain
        string baseDomain = "https://gooneygoogoo.com.co/";

        // Sample query parameters
        string sampleQuery = "search?q=benchmarking&sort=desc&tracking=";

        // Generate a random alphanumeric string of the required length
        string randomData = new string(Enumerable.Repeat("abcdef1234567890", length / 16)
                                 .SelectMany(s => s) // Flatten into a single sequence
                                 .Take(length)        // Take only the required number of characters
                                 .ToArray());

        // Construct and return the full URL
        return $"{baseDomain}{sampleQuery}{randomData}";
    }

    /// <summary>
    /// Benchmarks Murmur3 hashing on a UTF-16 (Unicode) encoded string.
    /// </summary>
    [Benchmark]
    public void Murmur3_UNICODE_CreateHash()
    {
        _ = Murmur3.CreateHash(_unicodeBytes);
    }

    /// <summary>
    /// Benchmarks Murmur3 hashing on a UTF-32 encoded string.
    /// </summary>
    [Benchmark]
    public void Murmur3_UTF32_CreateHash()
    {
        _ = Murmur3.CreateHash(_utf32Bytes);
    }

    /// <summary>
    /// Benchmarks Murmur3 hashing on a UTF-8 encoded string.
    /// </summary>
    [Benchmark]
    public void Murmur3_UTF8_CreateHash()
    {
        _ = Murmur3.CreateHash(_utf8Bytes);
    }

    /// <summary>
    /// Benchmarks Murmur3 hashing on an ASCII encoded string.
    /// Note: ASCII only supports basic English characters (no special Unicode symbols).
    /// </summary>
    [Benchmark]
    public void Murmur3_ASCII_CreateHash()
    {
        _ = Murmur3.CreateHash(_asciiBytes);
    }
}