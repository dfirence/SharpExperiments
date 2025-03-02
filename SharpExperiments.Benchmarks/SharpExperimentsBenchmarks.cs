using BenchmarkDotNet.Attributes;
using SharpExperiments;  // Reference your main project here

namespace SharpExperiments.Benchmarks
{
    [MemoryDiagnoser]
    public class SharpExperimentsBenchmarks
    {

        /// <summary>
        /// SharpExperimentsBenchmark constructor
        /// </summary>
        public SharpExperimentsBenchmarks()
        {
        }

        /// <summary>
        /// SharpExperiments method to benchmark
        /// </summary>
        [Benchmark]
        public void BenchmarkMethod1()
        {
        }

        /// <summary>
        /// SharpExperiments method to benchmark
        /// </summary>
        [Benchmark]
        public void BenchmarkMethod2()
        {
        }
    }
}
