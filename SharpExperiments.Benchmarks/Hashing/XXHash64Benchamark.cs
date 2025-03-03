namespace SharpExperiments.Benchmarks.Hashing;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SharpExperiments.Hashing;


//[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public class Hashing_XXHash64_Benchmark
{
    private byte[]? _testData;

    [Params(50, 200, 1000, 5000)] // Test with different string lengths
    public int DataSize;

    [GlobalSetup]
    public void Setup()
    {
        string testString = new string('X', DataSize); // Fill with repeating character
        _testData = Encoding.UTF8.GetBytes(testString); // Convert to bytes
    }

    [Benchmark]
    public ulong XXHash64_CreateHash()
    {
        return XXHash64.CreateHash(_testData!);
    }

    [Benchmark]
    public (ulong, ulong) Murmur3_CreateHash()
    {
        return Murmur3.CreateHash(_testData);
    }
}
