namespace SharpExperiments.REPL;
using System;
using System.Collections.Generic;
using SharpExperiments.BloomFilters;
using SharpExperiments.Hashing;

public static class REPLConsole
{
    private static bool s_isRunning = true;
    private static string s_currentModule = string.Empty;

    private static StandardBloomFilter<string>? bloomFilter = null; // Maintain Bloom Filter instance

    private const string c_promptName = "sharpExperiments";
    private const string c_unknownCommand = "Unknown command. Type 'help' for a list of commands.";
    public static void Start()
    {
        Console.Clear();
        ColorPalette.Info("SharpExperiments REPL - Type 'help' for commands, 'exit' to quit.");

        // Set up the delegate for dynamic prompt retrieval
        KeyHandler.GetPromptDelegate = GetPrompt;

        while (s_isRunning)
        {
            Console.Write(GetPrompt());
            string? input = ReadInput();
            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            REPLHistory.Add(input);
            ProcessCommand(input);
        }
    }

    private static string GetPrompt()
    {
        string basePrompt = ColorPalette.LightGray(c_promptName);

        if (!string.IsNullOrEmpty(s_currentModule))
        {
            string moduleSuffix = ColorPalette.BoldBrightGreen($"{s_currentModule}");
            return $"{basePrompt}|[ {moduleSuffix} ]|>> ";
        }

        return $"{basePrompt} |>> ";
    }

    private static string ReadInput()
    {
        var inputBuffer = new List<char>();
        int cursorPosition = 0;

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                string finalCommand = new string(inputBuffer.ToArray());

                // Store command in history
                KeyHandler.StoreCommand(finalCommand);

                return finalCommand;
            }

            // Delegate key handling
            KeyHandler.HandleKeyPress(key, inputBuffer, ref cursorPosition);
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
            // Global Commands
            //----------------------
            case "back":
                ExitModule();
                break;
            case "cls":
            case "clear":
                Console.Clear();
                break;
            case "q":
            case "quit":
            case "exit":
                s_isRunning = false;
                Console.WriteLine();
                break;
            case "h":
            case "help":
            case "?":
                PrintHelp();
                break;
            case "use":
                UseModule(args);
                break;

            //----------------------
            // Module-Specific Commands (No Prefix Required)
            //----------------------
            default:
                if (s_currentModule == "bloom")
                {
                    HandleBloomCommand(command, args);
                }
                else if (s_currentModule == "hashing")
                {
                    HandleHashingCommand(command, args);
                }
                else
                {
                    ColorPalette.Error(c_unknownCommand);
                }
                break;
        }
    }

    private static void HandleBloomCommand(string command, string args)
    {
        switch (command)
        {
            case "create":
                CreateBloomFilter(args);
                break;
            case "add":
                AddItemToBloom(args);
                break;
            case "fptest":
                RunFalsePositiveTest();
                break;
            case "maybe":
                CheckMembership(args);
                break;
            case "hash":
                ShowHash(args);
                break;
            case "show":
                if (args.Equals("array", StringComparison.OrdinalIgnoreCase))
                {
                    bloomFilter?.ShowArrayGrid();
                }
                else if (args.Equals("config", StringComparison.OrdinalIgnoreCase))
                {
                    bloomFilter?.ShowConfiguration();
                }
                else
                {
                    goto default;
                }
                break;
            case "count":
                ShowInsertedCount();
                break;
            default:
                ColorPalette.Error("Unknown Bloom Filter command. Type `?` for available commands.");
                break;
        }
    }

    private static void HandleHashingCommand(string command, string args)
    {
        switch (command)
        {
            case "murmur3":
                RunMurmur3(args);
                break;
            default:
                ColorPalette.Error("Unknown Hashing command. Type `?` for available commands.");
                break;
        }
    }


    private static void PrintHelp()
    {
        if (s_currentModule == "bloom")
        {
            ColorPalette.Info("\nBloom Filter Commands:");
            Console.WriteLine($@"
            create <n> <p>    - Create a new Bloom Filter (expected elements & FP rate)
            add <value>       - Add a value to the Bloom Filter
            maybe <value>     - Check if a value **might** be in the Bloom Filter
            hash <value>      - Show hashes for a given value
            show array        - Display the Bloom Filter bit array
            show config       - Show the Bloom Filter configuration
            count             - Show number of inserted elements
            back              - Exit the Bloom Filter module
            ?                 - Show this help menu");
        }
        else if (s_currentModule == "hashing")
        {
            ColorPalette.Info("\nHashing Commands:");
            Console.WriteLine($@"
            murmur3 <text> [--seed <number>] - Compute a Murmur3 hash
            back                             - Exit the Hashing module
            ?                                - Show this help menu");
        }
        else
        {
            ColorPalette.Info("\nGlobal Commands:");
            Console.WriteLine($@"
            help            - Show this help message
            clear           - Clear the screen (or use CTRL+L, or use cls)
            exit            - Exit the REPL
            use <module>    - Enter a specific module (example, `use hashing`)
            ?               - Show this help menu");
        }
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

        var validModules = new HashSet<string> { "hashing", "bloom" };

        if (validModules.Contains(module.ToLower()))
        {
            s_currentModule = module.ToLower();
        }
        else
        {
            ColorPalette.Error($"Module '{module}' not found. Available modules: hashing, benchmarks, bits");
        }
    }

    private static void AddItemToBloom(string parameters)
    {
        if (bloomFilter == null)
        {
            ColorPalette.Error("Error: No Bloom filter created. Use 'bloom create <n> <p>' first.");
            return;
        }

        if (string.IsNullOrEmpty(parameters))
        {
            ColorPalette.Error("Usage: bloom add <item>");
            return;
        }
        // Use Stopwatch.GetTimestamp() for nanosecond precision
        long startTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        bloomFilter.Add(parameters.Trim());
        // Use Stopwatch.GetTimestamp() for nanosecond precision
        long endTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        // Convert elapsed time to nanoseconds
        double elapsedNanoseconds = (endTimestamp - startTimestamp) * (1_000_000_000.0 / System.Diagnostics.Stopwatch.Frequency);
        ColorPalette.Success($"\nAdded '{parameters}' to Bloom Filter: {elapsedNanoseconds:F2} ns");
    }

    private static void CheckMembership(string parameters)
    {
        if (bloomFilter == null)
        {
            ColorPalette.Error("Error: No Bloom filter created. Use 'bloom create <n> <p>' first.");
            return;
        }

        if (string.IsNullOrEmpty(parameters))
        {
            ColorPalette.Error("Usage: bloom maybe <value>");
            return;
        }

        bool result = bloomFilter.MightContain(parameters.Trim());

        ColorPalette.Info($"\n🔎 Checking membership for \"{parameters}\":");
        Console.WriteLine(result ? $"✅ {parameters} is **possibly present** in the filter." : $"❌ {parameters} is **definitely NOT present**.");
    }

    private static void CreateBloomFilter(string parameters)
    {
        var parts = parameters.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 || !int.TryParse(parts[0], out int n) || !double.TryParse(parts[1], out double p))
        {
            ColorPalette.Error("Usage: bloom create <expectedElements> <falsePositiveRate>");
            return;
        }

        bloomFilter = new StandardBloomFilter<string>(n, p);
        ColorPalette.Success($"\nCreated Bloom Filter with n={n}, p={p}");
        bloomFilter.ShowConfiguration();
    }

    private static void ShowInsertedCount()
    {
        if (bloomFilter == null)
        {
            ColorPalette.Error("Error: No Bloom filter created. Use 'bloom create <n> <p>' first.");
            return;
        }

        ColorPalette.Info($"\nBloom Filter Items: {bloomFilter.GetCurrentFilterSize()}");
    }

    private static void ShowHash(string parameters)
    {
        if (bloomFilter == null)
        {
            ColorPalette.Error("Error: No Bloom filter created. Use 'bloom create <n> <p>' first.");
            return;
        }

        if (string.IsNullOrEmpty(parameters))
        {
            ColorPalette.Error("Usage: bloom hash <value>");
            return;
        }

        Span<long> hashValues = stackalloc long[bloomFilter.GetCurrentHashCount()];
        Murmur3.CreateHashes(parameters.Trim(), hashValues);

        Console.WriteLine("\nHash values for \"" + parameters + "\":");
        var hashes = hashValues.ToArray();
        for (int n = 0; n < hashes.Length; n++)
        {
            Console.WriteLine($"  -> h{n + 1,-3} : {hashes[n]}");
        }
        Console.WriteLine("\n");
    }

    private static void RunFalsePositiveTest()
    {
        if (bloomFilter == null)
        {
            ColorPalette.Error("Error: No Bloom filter created. Use 'bloom create <n> <p>' first.");
            return;
        }

        int expectedElements = bloomFilter.GetAllocatedFilterSize();
        double expectedFpRate = bloomFilter.GetFalsePositiveRate();
        double observedFpRate = 0.0;
        
        var testFilter = new StandardBloomFilter<string>(expectedElements, expectedFpRate);

        // Reduce number of lookups for large filters
        int totalLookups = expectedElements > 5000 ? 250_000 : 1_000_000;

        // Adjust FP check frequency dynamically
        int checkInterval = Math.Max(10, expectedElements / 50);
        
        // Limit insertions (prevents excessive runtime)
        int maxInsertions = (int)(expectedElements * 1.5);
        int effectiveLimit = 0;
        int falsePositives = 0;
        
        Console.WriteLine($@"
            
            🔵 Running optimized false positive analysis
            ========================================================================
            
            📌 Simulated Filter Capacity    {expectedElements:N0}
            🔹 Expected FP Rate             {expectedFpRate * 100:F1}%
            
            ========================================================================");
        
        // Move stackalloc OUTSIDE loop
        Span<long> lookupHashes = stackalloc long[testFilter.GetCurrentHashCount()];

        // Precompute hash values for lookups
        string[] precomputedHashes = new string[totalLookups];
        for (int i = 0; i < totalLookups; i++)
        {
            precomputedHashes[i] = Murmur3.GetStringHash($"lookup_{i}");
        }

        // Start time tracking (ensures timeout)
        var startTime = DateTime.UtcNow;
        
        // Prevent excessive runtime
        TimeSpan maxDuration = TimeSpan.FromMinutes(3);

        // Insert elements and track false positives
        for (int inserted = 0; inserted < maxInsertions; inserted++)
        {
            if (DateTime.UtcNow - startTime > maxDuration)
            {
                Console.WriteLine("\n⚠️ Timeout reached. Stopping test.");
                break;
            }

            string testValue = $"test_{Guid.NewGuid()}";
            testFilter.Add(testValue);

            // Run FP check at dynamic intervals using precomputed hashes
            if (inserted % checkInterval == 0)
            {
                falsePositives = 0;

                for (int i = 0; i < totalLookups; i++)
                {
                    if (testFilter.MightContain(precomputedHashes[i]))
                    {
                        falsePositives++;
                    }
                }

                observedFpRate = (double)falsePositives / totalLookups;

                if (observedFpRate > expectedFpRate)
                {
                    effectiveLimit = inserted;
                    break;
                }
            }

            // Stop at max insertions
            if (inserted >= maxInsertions - 1)
            {
                effectiveLimit = inserted;
                Console.WriteLine("\n⚠️ Max insertions reached. Stopping test.");
                break;
            }
        }

        Console.WriteLine($@"
            
            📊 Optimized Results
            ========================================================================
            
            False Positives     {falsePositives:N0} out of {totalLookups:N0} Lookups
            Inserted Items      {testFilter.GetCurrentFilterSize():N0}
            Expected FP Rate    {expectedFpRate * 100:F1}%
            Observed FP Rate    {observedFpRate * 100:F1}%
            
            🔥 {Math.Max(1, effectiveLimit):N0} Max items before exceeding expected false positive rate
            
            ========================================================================");
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

        // Use Stopwatch.GetTimestamp() for nanosecond precision
        long startTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();

        (ulong h1, ulong h2) = Murmur3.HashItem(text, seed);

        long endTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();

        // Convert elapsed time to nanoseconds
        double elapsedNanoseconds = (endTimestamp - startTimestamp) * (1_000_000_000.0 / System.Diagnostics.Stopwatch.Frequency);

        if (h1 == uint.MinValue && h2 == uint.MinValue)
        {
            ColorPalette.Error($"Error, to fix later| Default Value From Null Return");
            return;
        }

        ColorPalette.Info($"\nMurmur3 Hash of '{text}':");
        Console.WriteLine($@"
            h1                              :   {h1}
            h2                              :   {h2}
            Seed                            :   {seed}
            Input Bytes Count               :   {System.Text.Encoding.UTF8.GetByteCount(text)}
            Execution Time (ns)             :   {elapsedNanoseconds:F2} ns
            Final Hash (ulong XOR)          :   {h1 ^ h2}
            Final Hash (Hex from `h1`,`h2`) :   {h1:X16}{h2:X16}");
    }
}