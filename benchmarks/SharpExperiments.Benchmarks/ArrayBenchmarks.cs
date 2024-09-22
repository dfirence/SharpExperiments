/************************************************************************************
 * Copyright(c) dfirence@                                                           *
 *                                                                                  *
 * This program benchmarks array operations in C# using the code                    *
 * located in SharpExperiments.Arrays. The `SimpleArray` class is an `int[]` array  *
 * where exponential array size is provided - i.e., 10, 100, 1000, 10000, etc.      *
 *                                                                                  *
 ************************************************************************************/
using BenchmarkDotNet.Attributes;
using SharpExperiments.Arrays;


namespace SharpExperiments.Benchmarks
{

    [SimpleJob(warmupCount: 3, iterationCount: 5)]
    [MemoryDiagnoser]
    public class BenchmarkArrays
    {
        /// <summary>
        /// Used in benchmarks as dynamic parameter that is provided
        /// to the target method initializaing the array.
        /// </summary>
        [Params(10, 100, 1000, 10000, 100000, 1000000)]
        public int ArraySize;

        private SimpleArray? _array { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _array = new();
            _array.CreateArray(ArraySize);
        }
        //------------------------- Benchmark Methods -------------------------//

        /// <summary>
        /// Greedy iteration with For Loop
        /// </summary>
        [Benchmark]
        public void ForIterArray()
        {
            _array?.ForIterArray();
        }

        /// <summary>
        /// Iteration with Generator
        /// </summary>
        [Benchmark]
        public void ForIterArrayGenerator()
        {
            var g = _array?.ForIterArrayGenerator();
            foreach (var i in g)
            {
            }
        }

        [Benchmark]
        public void ForIterArrayAsSpan()
        {
            _array?.ForIterArrayAsSpan();
        }

        [Benchmark]
        public void ForIterArrayParallel()
        {
            _array?.ForIterArrayParallel();
        }

        [Benchmark]
        public void ForIterArrayParallelPartitioner()
        {
            _array?.ForIterArrayParallelPartitioner();
        }

        [Benchmark]
        public void ForIterArrayParallelOptimizedChunkSize()
        {
            _array?.ForIterArrayParallelOptimizedChunkSize();
        }
        /// <summary>
        /// Greedy Iteraton with While Loop
        /// </summary>
        [Benchmark]
        public void WhileIterArray()
        {
            _array?.WhileIterArray();
        }
    }
}