﻿namespace SharpExperiements.Benchmarks;
using BenchmarkDotNet.Running;

class Program
{
    static void Main(string[] args)
    {
        // Run benchmarks
        var _ = BenchmarkRunner.Run<SharpExperiementsBenchmarks>();
    }
}