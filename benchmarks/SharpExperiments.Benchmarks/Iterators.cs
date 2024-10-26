using BenchmarkDotNet.Attributes;
using SharpExperiments.Arrays;

using SharpExperiments.Iterators;


[SimpleJob(warmupCount: 3, iterationCount: 5)]
[MemoryDiagnoser]
public class IteratorTestBenchmark
{

    [Params(1_000_000)]
    public int Cycles { get; set; } = 0;

    public GetEnumeratorWhile g { get; set; } = new();


    [GlobalSetup]
    public void GlobalSetup()
    {
        g.Setup(Cycles);
    }

    [Benchmark(OperationsPerInvoke = 1000)] // Control number of operations
    public void GetEnumerator_WhileLoop()
    {
        g.RunWhileLoop();
    }

    [Benchmark(OperationsPerInvoke = 1000)]
    public void GetEnumerator_ForLoop()
    {
        g.RunForLoop();
    }

    [Benchmark(OperationsPerInvoke = 1000)]
    public void GetEnumerator_ForEachLoop()
    {
        g.RunForLoop();
    }

    [Benchmark(OperationsPerInvoke = 1000)] // Control number of operations
    public void GetEnumerator_ForEach_String_Loop()
    {
        g.RunForEachStringLoop();
    }
}