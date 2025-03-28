namespace SharpExperiments.Benchmarks.Hashing;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SharpExperiments.Hashing;
using System;
using System.Linq;
using System.Text;

/// <summary>
/// Benchmarking Murmur3 with realistic URL sizes.
/// </summary>
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public class Hashing_Murmur3_RealWorldURLs
{
    private byte[]? _utf8Bytes;

    /// <summary>
    /// Defines common URL lengths for real-world benchmarking.
    ///
    /// Test Case	    URL Length	Reason
    /// Short URL	    500 chars	Well within any safe limit
    /// Common URL	    2,000 chars	Matches Google’s indexing limit
    /// IE Max URL      2,083 chars	Covers legacy compatibility
    /// Standard Limit	8,192 chars	Matches Apache/Nginx defaults
    /// Upper Limit     16,384 chars	Covers IIS and high-end web apps
    /// </summary>
    [Params(500, 2000, 2083, 8192, 16384)]
    public int UrlLength;

    /// <summary>
    /// Generates test URLs and encodes them in UTF-8.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        string testUrl = GenerateTestUrl(UrlLength);
        _utf8Bytes = Encoding.UTF8.GetBytes(testUrl);
    }

    private string GenerateTestUrl(int length)
    {
        string baseDomain = "https://example.com/search?q=";
        string randomData = new string(Enumerable.Repeat("abcdef1234567890", length / 16)
                                 .SelectMany(s => s)
                                 .Take(length)
                                 .ToArray());
        return $"{baseDomain}{randomData}";
    }

    [Benchmark]
    public void Murmur3_UTF8_CreateHash()
    {
        _ = Murmur3.CreateHash(_utf8Bytes);
    }
    
    [Benvhmark]
    public void FNV1a64_UTF8_CreateHash()
    {
        _ = FNV1a64.CreateHash(_utf8Bytes);
    }
}