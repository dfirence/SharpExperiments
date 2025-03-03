// IterationCount=5  WarmupCount=2  
//
// | Method                              | Job      | Runtime  | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
// |------------------------------------ |--------- |--------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
// | String_Equals_Comparison_CaseIgnore | .NET 6.0 | .NET 6.0 |  7.3927 ns | 1.7619 ns | 0.4576 ns |  1.00 |    0.08 |      - |         - |          NA |
// | String_Contains_CaseIgnore          | .NET 6.0 | .NET 6.0 | 12.0934 ns | 0.4222 ns | 0.0653 ns |  1.64 |    0.09 |      - |         - |          NA |
// | String_EndsWith_CaseIgnore          | .NET 6.0 | .NET 6.0 |  7.5482 ns | 0.2201 ns | 0.0341 ns |  1.02 |    0.06 |      - |         - |          NA |
// | String_StartsWith_CaseIgnore        | .NET 6.0 | .NET 6.0 |  5.4833 ns | 2.7244 ns | 0.7075 ns |  0.74 |    0.10 |      - |         - |          NA |
// | String_Substring_Equals_CaseIgnore  | .NET 6.0 | .NET 6.0 | 11.7957 ns | 2.6647 ns | 0.4124 ns |  1.60 |    0.10 | 0.0025 |      32 B |          NA |
// | Span_Contains_CaseIgnore            | .NET 6.0 | .NET 6.0 | 11.6534 ns | 0.1514 ns | 0.0234 ns |  1.58 |    0.09 |      - |         - |          NA |
// | Span_EndsWith_CaseIgnore            | .NET 6.0 | .NET 6.0 |  3.6541 ns | 0.2017 ns | 0.0312 ns |  0.50 |    0.03 |      - |         - |          NA |
// | Span_Equals_Comparison_CaseIgnore   | .NET 6.0 | .NET 6.0 |  4.8420 ns | 0.1609 ns | 0.0418 ns |  0.66 |    0.04 |      - |         - |          NA |
// | Span_StartsWith_CaseIgnore          | .NET 6.0 | .NET 6.0 |  4.0788 ns | 0.3677 ns | 0.0955 ns |  0.55 |    0.03 |      - |         - |          NA |
// | Span_Substring_CaseIgnore           | .NET 6.0 | .NET 6.0 |  4.4063 ns | 0.5949 ns | 0.0921 ns |  0.60 |    0.03 |      - |         - |          NA |
// |                                     |          |          |            |           |           |       |         |        |           |             |
// | String_Equals_Comparison_CaseIgnore | .NET 8.0 | .NET 8.0 |  0.6631 ns | 0.0118 ns | 0.0031 ns |  1.00 |    0.01 |      - |         - |          NA |
// | String_Contains_CaseIgnore          | .NET 8.0 | .NET 8.0 |  7.4270 ns | 0.0274 ns | 0.0042 ns | 11.20 |    0.05 |      - |         - |          NA |
// | String_EndsWith_CaseIgnore          | .NET 8.0 | .NET 8.0 |  3.8921 ns | 0.0984 ns | 0.0152 ns |  5.87 |    0.03 |      - |         - |          NA |
// | String_StartsWith_CaseIgnore        | .NET 8.0 | .NET 8.0 |  0.4122 ns | 0.0293 ns | 0.0076 ns |  0.62 |    0.01 |      - |         - |          NA |
// | String_Substring_Equals_CaseIgnore  | .NET 8.0 | .NET 8.0 |  7.9323 ns | 0.3044 ns | 0.0791 ns | 11.96 |    0.12 | 0.0025 |      32 B |          NA |
// | Span_Contains_CaseIgnore            | .NET 8.0 | .NET 8.0 |  6.3061 ns | 0.4467 ns | 0.1160 ns |  9.51 |    0.16 |      - |         - |          NA |
// | Span_EndsWith_CaseIgnore            | .NET 8.0 | .NET 8.0 |  2.7833 ns | 0.0650 ns | 0.0101 ns |  4.20 |    0.02 |      - |         - |          NA |
// | Span_Equals_Comparison_CaseIgnore   | .NET 8.0 | .NET 8.0 |  0.4124 ns | 0.0266 ns | 0.0041 ns |  0.62 |    0.01 |      - |         - |          NA |
// | Span_StartsWith_CaseIgnore          | .NET 8.0 | .NET 8.0 |  0.4066 ns | 0.0539 ns | 0.0140 ns |  0.61 |    0.02 |      - |         - |          NA |
// | Span_Substring_CaseIgnore           | .NET 8.0 | .NET 8.0 |  0.4404 ns | 0.2716 ns | 0.0705 ns |  0.66 |    0.10 |      - |         - |          NA |

namespace SharpExperiments.Benchmarks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SharpExperiments.Tests.Strings;

[IterationCount(5), WarmupCount(2)]
[SimpleJob(RuntimeMoniker.Net60)] // Run for .NET 6
[SimpleJob(RuntimeMoniker.Net80)] // Run for .NET 8
[MemoryDiagnoser]
public class StdLib_StringComparisonBenchmark
{
    private StringComparisonTests tests = new();

    public StdLib_StringComparisonBenchmark()
    {
    }

    [Benchmark(Baseline = true)]
    public void String_Equals_Comparison_CaseIgnore()
    {
        tests.String_Equals_IgnoreCase_True();
    }

    [Benchmark]
    public void String_Contains_CaseIgnore()
    {
        tests.String_Contains_IgnoreCase_True();
    }

    [Benchmark]
    public void String_EndsWith_CaseIgnore()
    {
        tests.String_EndsWith_IgnoreCase_True();
    }

    [Benchmark]
    public void String_StartsWith_CaseIgnore()
    {
        tests.String_StartsWith_IgnoreCase_True();
    }

    [Benchmark]
    public void String_Substring_Equals_CaseIgnore()
    {
        tests.String_Substring_IgnoreCase_True();
    }

    //--------------------------------------------------
    // Span Benchmarks
    //--------------------------------------------------
    [Benchmark]
    public void Span_Contains_CaseIgnore()
    {
        tests.Span_Contains_IgnoreCase_True();
    }

    [Benchmark]
    public void Span_EndsWith_CaseIgnore()
    {
        tests.Span_EndsWith_IgnoreCase_True();
    }

    [Benchmark]
    public void Span_Equals_Comparison_CaseIgnore()
    {
        tests.Span_Equals_IgnoreCase_True();
    }

    [Benchmark]
    public void Span_StartsWith_CaseIgnore()
    {
        tests.Span_StartsWith_IgnoreCase_True();
    }

    [Benchmark]
    public void Span_Substring_CaseIgnore()
    {
        tests.Span_Substring_IgnoreCase_True();
    }
}
