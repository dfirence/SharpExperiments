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
            REPLConsole.Start();
        }
        else
        {
            Console.WriteLine($@"
            carlos_diaz | @dfirence
            
            SharpExperiments CLI
                Usage:
                    --repl      Start the interactive REPL mode.
            ");
        }
    }
}