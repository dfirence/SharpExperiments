namespace SharpExperiments.Benchmarks.Hashing;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SharpExperiments.Hashing;
using System;
using System.Security.Cryptography;

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public class Hashing_Murmur3_256KB_Benchmark
{
    private byte[]? _data256KB;

    /// <summary>
    /// Initializes a 256 KB block with random bytes before benchmarking.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _data256KB = new byte[256 * 1024]; // 256 KB block
        RandomNumberGenerator.Fill(_data256KB); // Fill with random data
    }

    /// <summary>
    /// Benchmarks Murmur3 hashing on a 256 KB block.
    /// </summary>
    [Benchmark]
    public void Murmur3_256KB_Hash()
    {
        _ = Murmur3.CreateHash(_data256KB);
    }
    
    [Benchmark]
    public void Afnv1_256KB_Hash()
    {
        _ = FNV1a64.CreateHash(_data256KB);
    }
}