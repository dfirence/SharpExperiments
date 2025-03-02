namespace SharpExperiments.Benchmarks;

using System;
using System.Linq;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

class Program
{
    static void Main(string[] args)
    {
        var benchmarks = typeof(Program).Assembly
            .GetTypes()
            .Where(t => t.IsClass && t.GetMethods().Any(m => m.GetCustomAttributes(typeof(BenchmarkAttribute), false).Length > 0))
            .ToList();

        Console.WriteLine("Available Benchmarks:");
        for (int i = 0; i < benchmarks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {benchmarks[i].Name}");
        }

        Console.Write("Enter the number of the benchmark to run (or press Enter to run all): ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int choice) && choice > 0 && choice <= benchmarks.Count)
        {
            Type selectedBenchmark = benchmarks[choice - 1];
            Console.WriteLine($"Running benchmark: {selectedBenchmark.Name}");
            BenchmarkRunner.Run(selectedBenchmark);
        }
        else
        {
            Console.WriteLine("Running all benchmarks...");
            BenchmarkRunner.Run(benchmarks.ToArray());
        }
    }
}
