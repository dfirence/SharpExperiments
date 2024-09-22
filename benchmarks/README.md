# Overview
The benchmarks project has many benchmark methods and classes imported from the main project (`SharpExperiments`). The whole purpose of this project is to learn the proper usage of the C# langauge, its testing aproaches and benchmarking techniques with tools like `dotNetBenchmark`.

<hr/><br/>

## Running Tests By Filtering

To run specific tests, the dotNetBenchmark allows command-line filtering, for example:

This commandline runs all methods that have the `*IterArray*` string pattern.

```text
dotnet run -c Release --filter *IterArray*
```

<hr/><br/>

## Running Tests With Custom Job Cycles

In `BenchmarkDotNet`, you can configure the number of iterations (also known as "warmup" and "actual run" cycles) by using the `[Job]` attribute with specific configuration settings to control how many times a benchmark is executed. By default, BenchmarkDotNet runs several iterations for both warmup and actual benchmarking to ensure accurate results, but you can reduce these numbers for faster feedback.

[Learn More](https://benchmarkdotnet.org/articles/guides/console-args.html)

```diff
using BenchmarkDotNet.Attributes;

namespace MyBenchmarks
{
+   [SimpleJob(warmupCount: 2, iterationCount: 10)]
    public class MyBenchmarkClass
    {
        // ... other benchmark code below
    }
}
```