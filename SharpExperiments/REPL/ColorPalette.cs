namespace SharpExperiments.REPL;

using System;

public static class ColorPalette
{
    public static void Print(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void PrintInline(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ResetColor();
    }

    public static void Success(string message) => Print(message, ConsoleColor.Green);
    public static void Error(string message) => Print(message, ConsoleColor.Red);
    public static void Info(string message) => Print(message, ConsoleColor.Cyan);
    public static void Debug(string message) => Print(message, ConsoleColor.Gray);
    public static void Prompt(string message) => PrintInline(message, ConsoleColor.White);
    public static void Warning(string message) => PrintInline(message, ConsoleColor.Yellow);
}