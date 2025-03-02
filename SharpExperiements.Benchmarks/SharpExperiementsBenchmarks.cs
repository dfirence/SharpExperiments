using BenchmarkDotNet.Attributes;
using SharpExperiements;  // Reference your main project here

namespace SharpExperiements.Benchmarks
{
    [MemoryDiagnoser]
    public class SharpExperiementsBenchmarks
    {

        /// <summary>
        /// SharpExperiementsBenchmark constructor
        /// </summary>
        public SharpExperiementsBenchmarks()
        {
        }

        /// <summary>
        /// SharpExperiements method to benchmark
        /// </summary>
        [Benchmark]
        public void BenchmarkMethod1()
        {
        }

        /// <summary>
        /// SharpExperiements method to benchmark
        /// </summary>
        [Benchmark]
        public void BenchmarkMethod2()
        {
        }
    }
}
