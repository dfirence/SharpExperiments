namespace SharpExperiments.REPL;

using System;

/// <summary>
/// Provides a colored String.
/// 
/// <para>**References:**</para>
/// - [AnsiColors](https://gist.github.com/JBlond/2fea43a3049b38287e5e9cefc87b2124)
/// </summary>
public static class ColorPalette
{
    public static string LightGray(string text) => $"\u001b[37m{text}\u001b[0m";
    public static string BoldBrightGreen(string text) => $"\u001b[1;92m{text}\u001b[0m";

    public static void Prompt(string text)
    {
        Console.Write(text);
    }

    public static void Info(string text)
    {
        Console.WriteLine($"\u001b[36m{text}\u001b[0m"); // Cyan
    }

    public static void Success(string text)
    {
        Console.WriteLine($"\u001b[32m{text}\u001b[0m"); // Green
    }

    public static void Warning(string text)
    {
        Console.WriteLine($"\u001b[33m{text}\u001b[0m"); // Yellow
    }

    public static void Error(string text)
    {
        Console.WriteLine($"\u001b[31m{text}\u001b[0m"); // Red
    }
}