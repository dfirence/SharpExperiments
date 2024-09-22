using BenchmarkDotNet.Running;


namespace SharpExperiments.Benchmarks
{

    class Program
    {
        static void Main(string[] args)
            => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }

}