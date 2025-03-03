namespace SharpExperiments;

using System;
using SharpExperiments.REPL;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0].Equals("--repl", StringComparison.OrdinalIgnoreCase))
        {
            // Start the interactive REPL mode
            REPL.Start();
        }
        else
        {
            Console.WriteLine("SharpExperiments CLI");
            Console.WriteLine("Usage:");
            Console.WriteLine("  --repl     Start the interactive REPL mode.");
        }
    }
}