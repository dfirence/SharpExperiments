namespace SharpExperiments.REPL;

using System;
using System.Collections.Generic;

public static class KeyHandler
{
    private const string c_helpString = "\nPress CTRL+C again to exit, or ENTER to continue...";
    private static List<string> _commandHistory = new(); // Stores previous commands
    private static int _historyIndex = -1; // Tracks position in history
    public static Func<string>? GetPromptDelegate { get; set; }


    public static string HandleKeyPress(ConsoleKeyInfo key, List<char> inputBuffer, ref int cursorPosition)
    {
        if (key.Key == ConsoleKey.L && key.Modifiers.HasFlag(ConsoleModifiers.Control))
        {
            Console.Clear();
            Console.WriteLine("Screen cleared.");
        }
        else if (key.Key == ConsoleKey.C && key.Modifiers.HasFlag(ConsoleModifiers.Control))
        {
            Console.WriteLine("\nPress CTRL+C again to exit, or ENTER to continue...");
            var confirm = Console.ReadKey(true);
            if (confirm.Key == ConsoleKey.C)
            {
                Environment.Exit(0);
            }
        }
        else if (key.Key == ConsoleKey.UpArrow)
        {
            // Navigate Up in History
            if (_commandHistory.Count > 0 && _historyIndex > 0)
            {
                _historyIndex--;
                ReplaceInputBuffer(inputBuffer, _commandHistory[_historyIndex], ref cursorPosition);
            }
        }
        else if (key.Key == ConsoleKey.DownArrow)
        {
            // Navigate Down in History
            if (_commandHistory.Count > 0 && _historyIndex < _commandHistory.Count - 1)
            {
                _historyIndex++;
                ReplaceInputBuffer(inputBuffer, _commandHistory[_historyIndex], ref cursorPosition);
            }
            else
            {
                ClearInputBuffer(inputBuffer, ref cursorPosition);
            }
        }
        else if (key.Key == ConsoleKey.LeftArrow)
        {
            // Move Left
            if (cursorPosition > 0)
            {
                cursorPosition--;
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }
        else if (key.Key == ConsoleKey.RightArrow)
        {
            // Move Right
            if (cursorPosition < inputBuffer.Count)
            {
                cursorPosition++;
                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
            }
        }
        else if (key.Key == ConsoleKey.Backspace && cursorPosition > 0)
        {
            // Remove character before cursor
            cursorPosition--;
            inputBuffer.RemoveAt(cursorPosition);

            Console.Write("\b \b"); // Erase character

            // Redraw remaining text
            string updatedText = new string(inputBuffer.ToArray()).Substring(cursorPosition);
            Console.Write(updatedText + " ");
            Console.SetCursorPosition(Console.CursorLeft - (updatedText.Length + 1), Console.CursorTop);
        }
        else if (!char.IsControl(key.KeyChar))
        {
            // Insert character at cursor position
            inputBuffer.Insert(cursorPosition, key.KeyChar);
            cursorPosition++;

            // Print updated buffer from cursor position
            string updatedText = new string(inputBuffer.ToArray()).Substring(cursorPosition - 1);
            Console.Write(updatedText);
            Console.SetCursorPosition(Console.CursorLeft - (updatedText.Length - 1), Console.CursorTop);
        }

        return new string(inputBuffer.ToArray());
    }

    /// <summary>
    /// Stores the entered command in history for future recall.
    /// </summary>
    public static void StoreCommand(string command)
    {
        if (!string.IsNullOrWhiteSpace(command))
        {
            _commandHistory.Add(command);
            _historyIndex = _commandHistory.Count; // Reset history position
        }
    }

    /// <summary>
    /// Replaces the current input buffer with a command from history.
    /// </summary>
    /// <summary>
    /// Replaces the current input buffer with a command from history.
    /// </summary>
    private static void ReplaceInputBuffer(List<char> inputBuffer, string newCommand, ref int cursorPosition)
    {
        // Move cursor back to overwrite old text
        Console.SetCursorPosition(0, Console.CursorTop);

        // Clear the line (including prompt + text)
        Console.Write(new string(' ', Console.WindowWidth));

        // Move cursor back again to print the new prompt and command
        Console.SetCursorPosition(0, Console.CursorTop);

        // Print the prompt (without duplication)
        if (GetPromptDelegate != null)
        {
            Console.Write(GetPromptDelegate.Invoke());
        }

        // Insert the command into the input buffer
        inputBuffer.Clear();
        inputBuffer.AddRange(newCommand);
        cursorPosition = newCommand.Length;

        // Print the restored command
        Console.Write(newCommand);
    }

    /// <summary>
    /// Clears the input buffer and resets cursor position.
    /// </summary>
    private static void ClearInputBuffer(List<char> inputBuffer, ref int cursorPosition)
    {
        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");

        inputBuffer.Clear();
        cursorPosition = 0;

        // Call delegate to get the REPL prompt dynamically
        if (GetPromptDelegate != null)
        {
            Console.Write(GetPromptDelegate.Invoke());
        }
    }
}