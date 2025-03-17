namespace SharpExperiments;

using System;
using SharpExperiments.REPL;
using SharpExperiments.ML.Classifiers;

public class Program
{
    private static string s_helpView = $@"
    carlos_diaz | @dfirence
            
        SharpExperiments CLI
            
        Usage:
                --repl      Start the interactive REPL mode.
                --mlc       Run ML Classifier Example";
    
    public static void Main(string[] args)
    {
        if (args.Length < 0)
        {
            Console.WriteLine(s_helpView);
            return;
        }
        
        if (args[0].Equals("--repl", StringComparison.OrdinalIgnoreCase))
        {
            // Start the interactive REPL mode
            REPLConsole.Start();
            return;
        }
        
        if (args[0].Equals("--mlc", StringComparison.OrdinalIgnoreCase))
        {
            EDRBinaryClassifier.TrainAndRun();
        }
    }
}