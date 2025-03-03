namespace SharpExperiments.REPL;

using System;
using System.Collections.Generic;
using SharpExperiments.Hashing;


public static class REPLConsole
{
    private static bool s_isRunning = true;
    private static string s_currentModule = string.Empty;
    private const string c_promptName = "sharpExperiments";
    private const string c_unknownCommand = "Unknown command. Type 'help' for a list of commands.";

    public static void Start()
    {
        Console.Clear();
        ColorPalette.Info("SharpExperiments REPL - Type 'help' for commands, 'exit' to quit.");

        while (s_isRunning)
        {
            // Display prompt
            ColorPalette.Prompt(GetPrompt());

            // Read user input
            string? input = ReadInput();
            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            // Store command in history
            REPLHistory.Add(input);

            // Process the command
            ProcessCommand(input);
        }
    }

    private static string GetPrompt()
    {
        string basePrompt = ColorPalette.LightGray(c_promptName);

        if (!string.IsNullOrEmpty(s_currentModule))
        {
            string moduleSuffix = ColorPalette.BoldBrightGreen($".{s_currentModule}");
            return $"{basePrompt}{moduleSuffix}|> ";
        }

        return $"{basePrompt}|> ";
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
            //----------------------
            // Back Command, exits Module
            //----------------------
            case "back":
                ExitModule();
                break;
            //----------------------
            // Clear REPL Screen
            //----------------------
            case "cls":
            case "clear":
                Console.Clear();
                break;
            //----------------------
            // Exit REPL
            //----------------------
            case "q":
            case "quit":
            case "exit":
                s_isRunning = false;
                Console.WriteLine();
                break;
            //----------------------
            // Show Help
            //----------------------
            case "h":
            case "help":
                PrintHelp();
                break;
            //----------------------
            // Enable MODULE
            //----------------------
            case "use":
                UseModule(args);
                break;
            //----------------------
            // Hashing: Murmur3
            //----------------------
            case "murmur3":
                RunMurmur3(args);
                break;
            default:
                ColorPalette.Error(c_unknownCommand);
                break;
        }
    }

    private static void PrintHelp()
    {
        ColorPalette.Info("\nAvailable Commands:");
        Console.WriteLine($@"
            help            - Show this help message
            clear           - Clear the screen (or use CTRL+L, or use cls)
            exit            - Exit the REPL
            use <module>    - Enter a specific module (example, `use hashing`)
        ");
    }

    private static void ExitModule()
    {
        if (string.IsNullOrEmpty(s_currentModule))
        {
            ColorPalette.Warning("Already at the top level.");
            return;
        }

        s_currentModule = "";
    }

    private static void UseModule(string module)
    {
        if (string.IsNullOrEmpty(module))
        {
            ColorPalette.Error("Usage: use <module>");
            return;
        }

        var validModules = new HashSet<string> { "hashing" };

        if (validModules.Contains(module.ToLower()))
        {
            s_currentModule = module.ToLower();
        }
        else
        {
            ColorPalette.Error($"Module '{module}' not found. Available modules: hashing, benchmarks, bits");
        }
    }

    private static void RunMurmur3(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            ColorPalette.Error("Usage: murmur3 <text> [--seed <number>]");
            return;
        }

        var parts = input.Split(" --seed ", StringSplitOptions.RemoveEmptyEntries);
        string text = parts[0].Trim();
        uint seed = parts.Length > 1 && uint.TryParse(parts[1], out uint s) ? s : 0; // Default seed = 0

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
        (ulong h1, ulong h2) = Murmur3.CreateHash(bytes, seed);

        ColorPalette.Success($"\nMurmur3 Hash of '{text}':");
        Console.WriteLine($@"
        
        h1      : {h1}
        h2      : {h2}
        Seed    : {seed}
        
        ");
    }
}