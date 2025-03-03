namespace SharpExperiments.REPL;

using System;
using System.Collections.Generic;

public static class REPLConsole
{
    private static bool _isRunning = true;
    private static string _currentModule = string.Empty;

    public static void Start()
    {
        Console.Clear();
        ColorPalette.Info("SharpExperiments REPL - Type 'help' for commands, 'exit' to quit.");

        while (_isRunning)
        {
            // Display prompt
            ColorPalette.Prompt(GetPrompt());

            // Read user input
            string? input = ReadInput();
            if (string.IsNullOrWhiteSpace(input)) continue;

            // Store command in history
            REPLHistory.Add(input);

            // Process the command
            ProcessCommand(input);
        }
    }

    private static string GetPrompt()
    {
        return string.IsNullOrEmpty(_currentModule) ? "sharpExperiments|> " : $"sharpExperiments.{_currentModule}|> ";
    }

    private static string ReadInput()
    {
        var inputBuffer = new List<char>();

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return new string(inputBuffer.ToArray());
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                string prevCommand = REPLHistory.GetPrevious();
                if (!string.IsNullOrEmpty(prevCommand))
                {
                    Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
                    ColorPalette.Prompt(GetPrompt());
                    Console.Write(prevCommand);
                    inputBuffer.Clear();
                    inputBuffer.AddRange(prevCommand);
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                string nextCommand = REPLHistory.GetNext();
                if (!string.IsNullOrEmpty(nextCommand))
                {
                    Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
                    ColorPalette.Prompt(GetPrompt());
                    Console.Write(nextCommand);
                    inputBuffer.Clear();
                    inputBuffer.AddRange(nextCommand);
                }
            }
            else if (key.Key == ConsoleKey.L && key.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                Console.Clear();
                ColorPalette.Info("Screen cleared.");
                ColorPalette.Prompt(GetPrompt());
            }
            else if (key.Key == ConsoleKey.C && key.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                KeyHandler.HandleKeyPress(key);
            }
            else if (key.Key == ConsoleKey.Backspace && inputBuffer.Count > 0)
            {
                inputBuffer.RemoveAt(inputBuffer.Count - 1);
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                inputBuffer.Add(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }
    }

    private static void ProcessCommand(string input)
    {
        var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts[0].ToLower();
        string args = parts.Length > 1 ? parts[1] : "";

        switch (command)
        {
            case "help":
                PrintHelp();
                break;
            case "exit":
                _isRunning = false;
                Console.WriteLine();
                break;
            case "clear":
                Console.Clear();
                break;
            case "use":
                UseModule(args);
                break;
            case "murmur3":
                RunMurmur3(args);
                break;
            default:
                ColorPalette.Error("Unknown command. Type 'help' for a list of commands.");
                break;
        }
    }

    private static void PrintHelp()
    {
        ColorPalette.Info("Available Commands:");
        Console.WriteLine("  help         - Show this help message");
        Console.WriteLine("  use <module> - Enter a specific module (hashing, benchmarks, bits)");
        Console.WriteLine("  exit         - Exit the REPL");
        Console.WriteLine("  clear        - Clear the screen (or use CTRL+L)");
    }

    private static void UseModule(string module)
    {
        if (string.IsNullOrEmpty(module))
        {
            ColorPalette.Error("Usage: use <module>");
            return;
        }

        var validModules = new HashSet<string> { "hashing", "benchmarks", "bits" };

        if (validModules.Contains(module.ToLower()))
        {
            _currentModule = module.ToLower();
        }
        else
        {
            ColorPalette.Error($"Module '{module}' not found. Available modules: hashing, benchmarks, bits");
        }
    }
    
    private static void RunMurmur3(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            ColorPalette.Error("Usage: murmur3 <text>");
            return;
        }

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
        (ulong h1, ulong h2) = Murmur3.CreateHash(bytes, 0);

        ColorPalette.Success($"Murmur3 Hash of '{text}':");
        Console.WriteLine($"  h1: {h1}");
        Console.WriteLine($"  h2: {h2}");
    }
}