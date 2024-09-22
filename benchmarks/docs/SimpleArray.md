## Overview

This shows how to experiment with the benchmarks of DotNetBenchmark.  This shows interesting behaviors of array implementations, some of these
are not optimized on purpose to demonstrate progressive usage of the language as it is learned to be used correctly.

<hr/><br/><br/>

```
BenchmarkDotNet v0.14.0, macOS Sonoma 14.6.1 (23G93) [Darwin 23.6.0]
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.300
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  Job-CXTQPX : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2

IterationCount=5  WarmupCount=3  
```

<br/>

| Method                                 | ArraySize | Mean             | Error           | StdDev         | Gen0   | Gen1   | Gen2   | Allocated |
|--------------------------------------- |---------- |-----------------:|----------------:|---------------:|-------:|-------:|-------:|----------:|
| ForIterArray                           | 10        |         6.955 ns |       1.2445 ns |      0.3232 ns |      - |      - |      - |         - |
| ForIterArrayGenerator                  | 10        |        33.263 ns |       3.4901 ns |      0.9064 ns | 0.0048 |      - |      - |      40 B |
| ForIterArrayAsSpan                     | 10        |         3.068 ns |       0.4275 ns |      0.1110 ns |      - |      - |      - |         - |
| ForIterArrayParallel                   | 10        |       462.726 ns |       8.8845 ns |      2.3073 ns | 0.2890 | 0.0105 | 0.0010 |    2375 B |
| ForIterArrayParallelPartitioner        | 10        |       564.534 ns |      53.1877 ns |      8.2309 ns | 0.3157 | 0.0114 |      - |    2602 B |
| ForIterArrayParallelOptimizedChunkSize | 10        |    27,865.819 ns |   5,550.4735 ns |  1,441.4403 ns | 0.1526 | 0.0610 |      - |    1527 B |
| WhileIterArray                         | 10        |        10.298 ns |       0.2725 ns |      0.0422 ns |      - |      - |      - |         - |
| ForIterArray                           | 100       |        75.685 ns |       6.1008 ns |      0.9441 ns |      - |      - |      - |         - |
| ForIterArrayGenerator                  | 100       |       267.232 ns |      25.3109 ns |      6.5732 ns | 0.0048 |      - |      - |      40 B |
| ForIterArrayAsSpan                     | 100       |        31.407 ns |       3.5121 ns |      0.5435 ns |      - |      - |      - |         - |
| ForIterArrayParallel                   | 100       |       743.519 ns |     441.1597 ns |    114.5678 ns | 0.3281 | 0.0553 | 0.0076 |    2697 B |
| ForIterArrayParallelPartitioner        | 100       |       797.828 ns |     530.5788 ns |     82.1077 ns | 0.3014 | 0.0420 |      - |    2500 B |
| ForIterArrayParallelOptimizedChunkSize | 100       |     1,860.199 ns |   1,874.5032 ns |    486.8025 ns | 0.3433 | 0.1068 | 0.0153 |    3169 B |
| WhileIterArray                         | 100       |        88.320 ns |      16.5165 ns |      4.2893 ns |      - |      - |      - |         - |
| ForIterArray                           | 1000      |       628.407 ns |      25.6966 ns |      3.9766 ns |      - |      - |      - |         - |
| ForIterArrayGenerator                  | 1000      |     3,640.472 ns |     660.5725 ns |    171.5486 ns | 0.0038 |      - |      - |      40 B |
| ForIterArrayAsSpan                     | 1000      |       270.267 ns |     154.9599 ns |     23.9802 ns |      - |      - |      - |         - |
| ForIterArrayParallel                   | 1000      |     1,474.907 ns |     189.3430 ns |     29.3010 ns | 0.2899 | 0.0973 | 0.0019 |    2696 B |
| ForIterArrayParallelPartitioner        | 1000      |       579.269 ns |      23.6624 ns |      6.1450 ns | 0.3204 | 0.0153 |      - |    2666 B |
| ForIterArrayParallelOptimizedChunkSize | 1000      |     2,082.814 ns |   1,189.4226 ns |    184.0645 ns | 0.3815 | 0.1297 | 0.0076 |    3143 B |
| WhileIterArray                         | 1000      |       778.007 ns |      47.4526 ns |      7.3433 ns |      - |      - |      - |         - |
| ForIterArray                           | 10000     |     6,101.415 ns |     320.1887 ns |     83.1520 ns |      - |      - |      - |         - |
| ForIterArrayGenerator                  | 10000     |    20,689.777 ns |   1,130.0644 ns |    293.4741 ns |      - |      - |      - |      40 B |
| ForIterArrayAsSpan                     | 10000     |     2,204.259 ns |     118.2157 ns |     30.7002 ns |      - |      - |      - |         - |
| ForIterArrayParallel                   | 10000     |       793.308 ns |     171.3229 ns |     26.5124 ns | 0.0553 | 0.0267 |      - |     472 B |
| ForIterArrayParallelPartitioner        | 10000     |     1,812.126 ns |     989.8037 ns |    153.1732 ns | 0.3052 | 0.1011 | 0.0076 |    2470 B |
| ForIterArrayParallelOptimizedChunkSize | 10000     |     2,258.302 ns |   1,493.4913 ns |    231.1194 ns | 0.3662 | 0.1183 | 0.0038 |    3145 B |
| WhileIterArray                         | 10000     |     7,744.043 ns |     311.2472 ns |     80.8299 ns |      - |      - |      - |         - |
| ForIterArray                           | 100000    |    60,838.093 ns |   2,075.4743 ns |    538.9941 ns |      - |      - |      - |         - |
| ForIterArrayGenerator                  | 100000    |   201,760.831 ns |  11,468.4423 ns |  1,774.7541 ns |      - |      - |      - |      40 B |
| ForIterArrayAsSpan                     | 100000    |    25,243.457 ns |   6,923.2185 ns |  1,797.9379 ns |      - |      - |      - |         - |
| ForIterArrayParallel                   | 100000    |       770.230 ns |     174.2592 ns |     45.2546 ns | 0.0305 | 0.0172 |      - |     256 B |
| ForIterArrayParallelPartitioner        | 100000    |     3,160.101 ns |     939.5403 ns |    243.9956 ns | 0.1297 | 0.0420 | 0.0019 |    2472 B |
| ForIterArrayParallelOptimizedChunkSize | 100000    |     2,606.029 ns |   1,629.1664 ns |    423.0893 ns | 0.1411 | 0.0477 | 0.0019 |    4249 B |
| WhileIterArray                         | 100000    |    77,693.559 ns |   1,897.5707 ns |    492.7931 ns |      - |      - |      - |         - |
| ForIterArray                           | 1000000   |   668,589.981 ns |  73,659.3800 ns | 19,129.1071 ns |      - |      - |      - |      23 B |
| ForIterArrayGenerator                  | 1000000   | 3,236,088.035 ns | 208,274.6942 ns | 32,230.7394 ns |      - |      - |      - |      43 B |
| ForIterArrayAsSpan                     | 1000000   |   257,149.507 ns |  75,421.2225 ns | 19,586.6520 ns |      - |      - |      - |         - |
| ForIterArrayParallel                   | 1000000   |     1,245.455 ns |     575.7595 ns |    149.5229 ns | 0.0286 | 0.0153 |      - |     238 B |
| ForIterArrayParallelPartitioner        | 1000000   |     1,941.653 ns |   1,072.5581 ns |    165.9796 ns | 0.0553 | 0.0248 |      - |     498 B |
| ForIterArrayParallelOptimizedChunkSize | 1000000   |     2,408.815 ns |     332.3811 ns |     86.3183 ns | 0.0610 | 0.0248 |      - |     509 B |
| WhileIterArray                         | 1000000   |   827,198.718 ns | 131,289.8608 ns | 34,095.5600 ns |      - |      - |      - |       1 B |


```diff
// * Hints *
Outliers
  BenchmarkArrays.ForIterArray: IterationCount=5, WarmupCount=3                           -> 1 outlier  was  detected (7.86 ns)
  BenchmarkArrays.ForIterArrayAsSpan: IterationCount=5, WarmupCount=3                     -> 1 outlier  was  detected (4.41 ns)
  BenchmarkArrays.ForIterArrayParallelPartitioner: IterationCount=5, WarmupCount=3        -> 1 outlier  was  removed (790.18 ns)
  BenchmarkArrays.WhileIterArray: IterationCount=5, WarmupCount=3                         -> 1 outlier  was  removed (12.11 ns)
  BenchmarkArrays.ForIterArray: IterationCount=5, WarmupCount=3                           -> 1 outlier  was  removed (85.66 ns)
  BenchmarkArrays.ForIterArrayAsSpan: IterationCount=5, WarmupCount=3                     -> 1 outlier  was  removed (36.89 ns)
  BenchmarkArrays.ForIterArrayParallelPartitioner: IterationCount=5, WarmupCount=3        -> 1 outlier  was  removed (1.08 us)
  BenchmarkArrays.ForIterArray: IterationCount=5, WarmupCount=3                           -> 1 outlier  was  removed (659.08 ns)
  BenchmarkArrays.ForIterArrayAsSpan: IterationCount=5, WarmupCount=3                     -> 1 outlier  was  removed (400.07 ns)
  BenchmarkArrays.ForIterArrayParallel: IterationCount=5, WarmupCount=3                   -> 1 outlier  was  removed (1.63 us)
  BenchmarkArrays.ForIterArrayParallelPartitioner: IterationCount=5, WarmupCount=3        -> 1 outlier  was  detected (571.37 ns)
  BenchmarkArrays.ForIterArrayParallelOptimizedChunkSize: IterationCount=5, WarmupCount=3 -> 1 outlier  was  removed, 2 outliers were detected (1.81 us, 2.36 us)
  BenchmarkArrays.WhileIterArray: IterationCount=5, WarmupCount=3                         -> 1 outlier  was  removed, 2 outliers were detected (769.42 ns, 793.44 ns)
  BenchmarkArrays.ForIterArrayParallel: IterationCount=5, WarmupCount=3                   -> 1 outlier  was  removed (904.34 ns)
  BenchmarkArrays.ForIterArrayParallelPartitioner: IterationCount=5, WarmupCount=3        -> 1 outlier  was  removed, 2 outliers were detected (1.60 us, 2.28 us)
  BenchmarkArrays.ForIterArrayParallelOptimizedChunkSize: IterationCount=5, WarmupCount=3 -> 1 outlier  was  removed, 2 outliers were detected (1.93 us, 3.11 us)
  BenchmarkArrays.ForIterArrayGenerator: IterationCount=5, WarmupCount=3                  -> 1 outlier  was  removed (212.80 us)
  BenchmarkArrays.ForIterArrayGenerator: IterationCount=5, WarmupCount=3                  -> 1 outlier  was  removed (3.56 ms)
  BenchmarkArrays.ForIterArrayParallelPartitioner: IterationCount=5, WarmupCount=3        -> 1 outlier  was  removed (2.65 us)
```

```diff
// * Legends *
  ArraySize : Value of the 'ArraySize' parameter
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  Gen0      : GC Generation 0 collects per 1000 operations
  Gen1      : GC Generation 1 collects per 1000 operations
  Gen2      : GC Generation 2 collects per 1000 operations
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 ns      : 1 Nanosecond (0.000000001 sec)
```