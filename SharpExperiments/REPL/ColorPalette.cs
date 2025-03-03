namespace SharpExperiments.REPL;
using System;

public static class ColorPalette
{
    public static string DimGray(string text) => $"\u001b[90m{text}\u001b[0m"; // ANSI for Dim Gray
    public static string BoldBrightGreen(string text) => $"\u001b[1;92m{text}\u001b[0m"; // ANSI for Bright Green (Bold)
    
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