// IterationCount=5  WarmupCount=2  

// | Method                       | Mean      | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
// |----------------------------- |----------:|----------:|----------:|------:|--------:|----------:|------------:|
// | String_Comparison_CaseIgnore | 0.6792 ns | 0.0348 ns | 0.0090 ns |  1.00 |    0.02 |         - |          NA |
// | Span_Comparison_CaseIgnore   | 0.5090 ns | 0.0282 ns | 0.0044 ns |  0.75 |    0.01 |         - |          NA |

namespace SharpExperiments.Benchmarks;
using BenchmarkDotNet.Attributes;
using SharpExperiments.Tests.Strings;

[IterationCount(5), WarmupCount(2)]
[MemoryDiagnoser]
public class StringComparisonBenchmark
{
    private StringComparisonTests tests = new();

    /// <summary>
    /// SharpExperimentsBenchmark constructor
    /// </summary>
    public StringComparisonBenchmark()
    {
    }

    /// <summary>
    /// SharpExperiments method to benchmark
    /// </summary>
    [Benchmark(Baseline = true)]
    public void String_Equals_Comparison_CaseIgnore()
    {
        tests.String_Equals_IgnoreCase_True();
    }

    [Benchmark]
    public void Span_Equals_Comparison_CaseIgnore()
    {
        tests.Spane_Equals_IgnoreCase_True_NoAlloc();
    }
}
