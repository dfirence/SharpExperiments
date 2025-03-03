namespace SharpExperiments.REPL;

using System;

public static class KeyHandler
{
    private const string c_helpString = "\nPress CTRL+C again to exit, or ENTER to continue...";
    public static void HandleKeyPress(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.L && key.Modifiers.HasFlag(ConsoleModifiers.Control))
        {
            Console.Clear();
            Console.WriteLine("Screen cleared.");
        }
        else if (key.Key == ConsoleKey.C && key.Modifiers.HasFlag(ConsoleModifiers.Control))
        {
            Console.WriteLine(c_helpString);
            var confirm = Console.ReadKey(true);
            if (confirm.Key == ConsoleKey.C)
            {
                Environment.Exit(0);
            }
        }
    }
}