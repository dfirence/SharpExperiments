```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.3 (24D60) [Darwin 24.3.0]
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.404
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  Job-VRDDRH : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2

IterationCount=5  WarmupCount=2  

```
| Method                              | Mean      | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------------------ |----------:|----------:|----------:|------:|--------:|----------:|------------:|
| String_Equals_Comparison_CaseIgnore | 0.7013 ns | 0.1859 ns | 0.0483 ns |  1.00 |    0.09 |         - |          NA |
| Span_Equals_Comparison_CaseIgnore   | 0.5112 ns | 0.0431 ns | 0.0112 ns |  0.73 |    0.05 |         - |          NA |
