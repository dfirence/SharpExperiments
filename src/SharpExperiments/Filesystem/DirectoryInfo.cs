/// # Overview
/// 
/// The C# Directory class is a convenient and efficient way to manipulate or interact with
/// Folder Objects on the filesystem. The Benchmarks below, while they may seem silly to most
/// are interesting to me so I can learn how to work with efficient (performant) methods provided
/// by the language, versus, areas where I may have to implement my own methods.
/// 
/// ## Benchmarks
/// Something is weird when you run many methods by default in the Benchmarked Class (running all tests).
/// The benchmarked methods have an increase in execution time when you do this, where as running these
/// individually has a different result, meaning, each method is measured more accurately resulting in faster
/// measurements when you do it individually.
/// 
/// ## Running All
/// 
/// IterationCount=15  WarmupCount=5  
/// | Method                               | Mean         | Error        | StdDev       | Gen0   | Allocated |
/// |------------------------------------- |-------------:|-------------:|-------------:|-------:|----------:|
/// | GetCurrentDirectory                  | 17,222.93 ns |   969.621 ns |   906.984 ns | 0.0305 |     368 B |
/// | GetCurrentDirectoryByDotNotation     | 18,482.80 ns | 1,580.346 ns | 1,478.256 ns | 0.1221 |    1104 B |
/// | GetCurrentDirectoryInfo              | 17,930.70 ns | 1,296.748 ns | 1,149.533 ns | 0.0610 |     568 B |
/// | GetCurrentDirectoryInfoByDotNotation | 18,998.24 ns | 1,705.889 ns | 1,595.690 ns | 0.1526 |    1304 B |
/// | GetSystemDirectoryInfo               |     71.66 ns |     7.870 ns |     7.362 ns | 0.0267 |     224 B |
///
/// ## Running Individually
/// Conclusion, notice the slight increase on the methods with *DotNotation, which makes sense because we
/// allocated a string to later pass into the native methods.
/// 
/// IterationCount=15  WarmupCount=5  
/// | Method                                | Mean     | Error    | StdDev   | Gen0   | Allocated |
/// |-------------------------------------  |---------:|---------:|---------:|-------:|----------:|
/// | GetCurrentDirectory                   | 17.17 us | 0.247 us | 0.231 us | 0.0305 |     368 B |
/// | GetCurrentDirectoryByDotNotation      | 17.44 us | 0.173 us | 0.145 us | 0.1221 |    1104 B |
/// | GetCurrentDirectoryInfo               | 17.38 us | 0.208 us | 0.185 us | 0.0610 |     568 B |
/// | GetCurrentDirectoryInfoByDotNotation  | 17.70 us | 0.172 us | 0.152 us | 0.1526 |    1304 B |
/// | GetSystemDirectoryInfo                | 63.65 ns | 2.261 ns | 1.765 ns | 0.0268 |     224 B |
/// | GetDirectoryCurrentDirectories        | 87.92 us | 5.667 us | 5.301 us | 0.8545 |   7.68 KB |
/// | GetDirectoryCurrentFiles              | 87.33 us | 3.787 us | 3.162 us | 2.3193 |  19.92 KB |
/// | GetDirectoryFilesByFilterWhereToArray | 42.79 us | 2.103 us | 1.968 us | 0.1831 |   1.76 KB |
/// | GetDirectoryFilesByFilterWhereToList  | 43.81 us | 2.196 us | 1.946 us | 0.1831 |   1.79 KB |
/// 
namespace SharpExperiments.DirectoryInfo
{
    using System;
    using System.IO;

    public static class DirectoryHelper
    {
        public static string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
        public static string GetCurrentDirectoryByDotNotation()
        {
            return Path.GetFullPath(".");
        }
        public static DirectoryInfo GetCurrentDirectoryInfo()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory());
        }
        public static DirectoryInfo GetCurrentDirectoryInfoByDotNotation()
        {
            return new DirectoryInfo(
                Path.GetFullPath(".")
            );
        }
        public static DirectoryInfo? GetSystemDirectoryInfo()
        {
            var os = Environment.OSVersion.Platform.ToString();

            string? path;
            if (os == "Unix")
            {
                path = "/etc";
            }
            else
            {
                path = @"C:\Windows\System32";
            }
            try
            {
                return new DirectoryInfo(Path.GetFullPath(path));
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// This method returns boolean, but the focus was to see how the `Directory.GetDirectories()`
        /// method by returning an array of strings allocates under the hood. The benchmark shows it
        /// allocated 7.68 KB for the current directory of this project, yielding insightful allo behavior.
        /// 
        /// | Method         | Mean     | Error    | StdDev   | Gen0   | Allocated |
        /// |--------------- |---------:|---------:|---------:|-------:|----------:|
        /// | GetDirectories | 87.92 us | 5.667 us | 5.301 us | 0.8545 |   7.68 KB |
        /// 
        /// !note:
        /// 
        /// Knowing about the allocation, and where target folders can have many subfolders, this may be
        /// a good API where `ArrayPool<T>` OR lazi evaluation can be performed.
        /// 
        /// </summary>
        /// <returns>bool</returns>
        public static bool GetDirectoryCurrentDirectories()
        {
            return Directory.GetDirectories(Directory.GetCurrentDirectory()).Length > 0 ? true : false;
        }
        public static bool GetDirectoryCurrentFiles()
        {
            return Directory.GetFiles(Directory.GetCurrentDirectory()).Length > 0 ? true : false;
        }

        /// <summary>
        /// | Method                                | Mean     | Error    | StdDev   | Gen0   | Allocated |
        /// |-------------------------------------- |---------:|---------:|---------:|-------:|----------:|
        /// | GetDirectoryFilesByFilterWhereToArray | 43.94 us | 2.151 us | 2.012 us | 0.1831 |   1.76 KB |
        /// </summary>
        /// <returns>boolean</returns>
        public static bool GetDirectoryFilesByFilterWhereToArray()
        {
            string? cwd = Directory.GetParent(
                Directory.GetCurrentDirectory()
            )?.ToString();

            cwd = Directory.GetParent(cwd ?? string.Empty)?.ToString();
            return Directory.GetFiles(cwd ?? string.Empty)
                            .Where(e => e.Contains(".json"))
                            .ToArray().Length > 0
                            ? true
                            : false;
        }

        /// <summary>
        /// 
        /// | Method                               | Mean     | Error    | StdDev   | Gen0   | Allocated |
        /// |------------------------------------- |---------:|---------:|---------:|-------:|----------:|
        /// | GetDirectoryFilesByFilterWhereToList | 40.60 us | 0.513 us | 0.428 us | 0.1831 |   1.79 KB |
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool GetDirectoryFilesByFilterWhereToList()
        {
            var cwd = Directory.GetParent(
                Directory.GetCurrentDirectory()
            )?.ToString();

            cwd = Directory.GetParent(cwd ?? string.Empty)
                          ?.ToString();
            return Directory.GetFiles(cwd ?? string.Empty)
                            .Where(e => e.Contains(".json"))
                            .ToList().Count > 0
                            ? true
                            : false;
        }
    }
}