namespace SharpExperiments.REPL;

using System;
using System.Collections.Generic;

public static class REPLHistory
{
    private static readonly List<string> History = new();
    private static int _currentIndex = -1;

    public static void Add(string command)
    {
        if (History.Count >= 10)
        {
            History.RemoveAt(0);
        }

        History.Add(command);
        _currentIndex = History.Count;
    }

    public static string GetPrevious()
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
        }

        return History[_currentIndex];
    }

    public static string GetNext()
    {
        if (_currentIndex < History.Count - 1)
        {
            _currentIndex++;
        }

        return History[_currentIndex];
    }
}