using System.Collections.Concurrent;

namespace SharpExperiments.Arrays
{
    public class SimpleArray
    {
        private int[]? _array { get; set; }

        public SimpleArray()
        {
        }
        public int? GetSize()
        {
            return _array?.Length;
        }
        public bool IsNull()
        {
            return _array == null;
        }
        public void CreateArray(int size)
        {
            _array = new int[size];
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = i + 1;
            }
        }

        public void ForIterArray()
        {
            if (IsNull())
                return;

            for (int i = 0; i < _array?.Length; i++)
            {
                if (i % 2 == 0)
                    continue;
            }
        }

        /// <summary>
        /// Span based iteration for each item.
        /// </summary>
        public void ForIterArrayAsSpan()
        {
            if (IsNull())
                return;

            ReadOnlySpan<int> ros = _array.AsSpan<int>();
            for (int i = 0; i < ros.Length; i++)
            {
                if (ros[i] % 2 == 0)
                {
                }
            }
        }

        /// <summary>
        /// A Generator method with `yield return`. Notice that to leverage
        /// a generator, you must changethe method's return type to be IEnumerable<T>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> ForIterArrayGenerator()
        {
            for (int i = 0; i < _array?.Length; i++)
            {
                yield return _array[i];
            }
        }
        public void WhileIterArray()
        {
            int? i = _array?.Length;
            while (i != 0)
            {
                if (i == 0)
                    break;

                if (i % 2 == 0)
                    i -= 2;
            }
        }
        //------------------------------- Async -------------------------------//
        public async Task ForIterArrayParallel()
        {
            if (_array == null)
                return;

            await Task.Run(() =>
            {
                _ = Parallel.For(0, _array.Length, i =>
                {
                    if (_array[i] % 2 == 0)
                    {
                    }
                });
            });
        }

        /// <summary>
        /// 
        /// Using Partitions with `Partitioner.Create` in this iteration.
        /// When running the benchmarks, compare this against the ForIterArrayParallel
        /// method. The focus is to determine if partitions do help speed up or constrain
        /// with the overhead from setting up tasks.
        /// 
        /// # Conclusion:
        /// The ForIterArrayParallelPartitioner method performs well for larger arrays,
        /// but parallelism introduces significant overhead for smaller arrays, making it inefficient for them.
        /// 
        /// Memory allocation decreases significantly as the array size increases,
        /// indicating that partitioning optimizes memory usage for larger datasets.
        /// Fine-tuning the chunk size and avoiding parallelism for small arrays would likely improve overall performance,
        /// especially for smaller workloads.
        /// 
        /// IterationCount=5  WarmupCount=3  
        /// 
        /// | Method                          | ArraySize | Mean       | Error       | StdDev    | Gen0   | Gen1   | Gen2   | Allocated |
        /// |-------------------------------- |---------- |-----------:|------------:|----------:|-------:|-------:|-------:|----------:|
        /// | ForIterArrayParallelPartitioner | 10        |   517.2 ns |    22.34 ns |   3.46 ns | 0.3176 | 0.0114 |      - |    2617 B |
        /// | ForIterArrayParallelPartitioner | 100       |   569.1 ns |   174.80 ns |  45.40 ns | 0.3166 | 0.0114 |      - |    2639 B |
        /// | ForIterArrayParallelPartitioner | 1000      |   524.0 ns |    12.55 ns |   3.26 ns | 0.3128 | 0.0191 |      - |    2581 B |
        /// | ForIterArrayParallelPartitioner | 10000     | 1,588.0 ns |   822.94 ns | 213.71 ns | 0.3052 | 0.1183 | 0.0076 |    2475 B |
        /// | ForIterArrayParallelPartitioner | 100000    | 3,529.8 ns | 1,470.86 ns | 381.98 ns | 0.1545 | 0.0515 | 0.0019 |    2472 B |
        /// | ForIterArrayParallelPartitioner | 1000000   | 1,551.2 ns |   852.28 ns | 131.89 ns | 0.0515 | 0.0248 |      - |     473 B |
        ///
        /// </summary>
        /// <returns></returns>
        public async Task ForIterArrayParallelPartitioner()
        {
            if (_array == null)
                return;

            int chunkSize = 0;

            if (_array.Length == 10) chunkSize = 2;

            else
            {
                chunkSize = _array.Length / 5;
            }
            var partitions = Partitioner.Create(0, _array.Length, chunkSize);
            await Task.Run(() =>
            {
                Parallel.ForEach(partitions, range =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        if (_array[i] % 2 == 0)
                        {
                        }
                    }
                });
            });
        }

        /// <summary>
        /// 
        /// Now compare this with the ParallelPartitioner, where use explicit configuration for
        /// the parellel work based on the available computing environment (CPU).
        /// 
        /// This test focuses on chunksize increasing and assigning a batch of elements in the
        /// array to a thread scheduled on the processor count. Exploring if the decrease of
        /// threads optimizes the processing of large arrays significantly.
        /// 
        /// # Conclusion
        /// For small arrays (like size 10), there’s a bit more overhead due to parallelism compared to the actual work,
        /// as evidenced by the 26.785 µs execution time. However, this overhead quickly decreases for larger arrays.
        /// For arrays of size 100,000 and 1,000,000, the method performs extremely well, staying under 4 µs for both,
        /// which demonstrates the efficiency of the parallel processing with optimized chunk sizes.
        /// 
        /// important!  Using `1.5` to calculate chunkSize works very well across mjority of workloads.
        /// 
        /// Smaller chunk size (1.5) appears to be more efficient for most workloads, especially 
        /// for medium-to-large arrays (100 to 1,000,000 elements), where it reduces the overhead of managing
        /// larger partitions while still balancing the workload effectively across threads.
        /// 
        /// The slight performance regression for very small arrays (10 elements) suggests 
        /// that parallelization might not be ideal for such small workloads, and a sequential approach could be more efficient in those cases.
        /// 
        /// IterationCount=5  WarmupCount=3  
        /// | Method                                 | ArraySize | Mean        | Error      | StdDev   | Gen0   | Gen1   | Gen2   | Allocated |
        /// |--------------------------------------- |---------- |------------:|-----------:|---------:|-------:|-------:|-------:|----------:|
        /// | ForIterArrayParallelOptimizedChunkSize | 10        | 25,269.2 ns | 1,690.0 ns | 261.5 ns | 0.1526 | 0.0610 |      - |    1527 B |
        /// | ForIterArrayParallelOptimizedChunkSize | 100       |  1,068.2 ns | 1,714.0 ns | 445.1 ns | 0.3738 | 0.1335 | 0.0114 |    2955 B |
        /// | ForIterArrayParallelOptimizedChunkSize | 1000      |    887.1 ns |   861.6 ns | 133.3 ns | 0.3738 | 0.0954 | 0.0114 |    2955 B |
        /// | ForIterArrayParallelOptimizedChunkSize | 10000     |  2,221.3 ns |   933.9 ns | 242.5 ns | 0.3777 | 0.1221 | 0.0076 |    2956 B |
        /// | ForIterArrayParallelOptimizedChunkSize | 100000    |  2,777.3 ns |   689.9 ns | 106.8 ns | 0.1259 | 0.0477 | 0.0019 |    2477 B |
        /// | ForIterArrayParallelOptimizedChunkSize | 1000000   |  1,923.9 ns |   758.5 ns | 117.4 ns | 0.0534 | 0.0248 |      - |     451 B |
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ForIterArrayParallelOptimizedChunkSize()
        {
            if (_array == null)
                return;

            // Get the number of logical cores available on the system
            int processorCount = Environment.ProcessorCount;

            // Calculate the optimal chunk size (1-2 chunks per core)
            int optimalChunks = processorCount * 2;  // Adjust this based on experimentation
            int chunkSize = _array.Length / optimalChunks;

            var partitions = Partitioner.Create(0, _array.Length, chunkSize);
            await Task.Run(() =>
            {
                Parallel.ForEach(partitions, range =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        if (_array[i] % 2 == 0)
                        {
                        }
                    }
                });
            });
        }

    }
}