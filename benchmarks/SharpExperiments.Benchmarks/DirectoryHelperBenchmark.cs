using BenchmarkDotNet.Attributes;
using SharpExperiments.DirectoryInfo;

namespace SharpExperiments.Benchmarks
{
    [SimpleJob(warmupCount: 5, iterationCount: 25)]
    [MemoryDiagnoser]
    public class BenchmarkDirectoryInfoHelper
    {
        [Benchmark]
        public void GetCurrentDirectory()
        {
            var _ = DirectoryHelper.GetCurrentDirectory();
        }

        [Benchmark]
        public void GetCurrentDirectoryByDotNotation()
        {
            var _ = DirectoryHelper.GetCurrentDirectoryByDotNotation();
        }

        [Benchmark]
        public void GetCurrentDirectoryInfo()
        {
            var _ = DirectoryHelper.GetCurrentDirectoryInfo();
        }

        [Benchmark]
        public void GetCurrentDirectoryInfoByDotNotation()
        {
            var _ = DirectoryHelper.GetCurrentDirectoryInfoByDotNotation();
        }

        [Benchmark]
        public void GetSystemDirectoryInfo()
        {
            var _ = DirectoryHelper.GetSystemDirectoryInfo();
        }

        [Benchmark]
        public void GetDirectoryCurrentDirectories()
        {
            var _ = DirectoryHelper.GetDirectoryCurrentDirectories();
        }

        [Benchmark]
        public void GetDirectoryCurrentFiles()
        {
            var _ = DirectoryHelper.GetDirectoryCurrentFiles();
        }

        [Benchmark]
        public void GetDirectoryFilesByFilterWhereToArray()
        {
            var _ = DirectoryHelper.GetDirectoryFilesByFilterWhereToArray();
        }

        [Benchmark]
        public void GetDirectoryFilesByFilterWhereToList()
        {
            var _ = DirectoryHelper.GetDirectoryFilesByFilterWhereToList();
        }
    }
}