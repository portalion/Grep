using System;
using System.IO;

static bool MatchPattern(string inputLine, string pattern)
{
    if (pattern == "\\d")
    {
        return inputLine.Any(c => char.IsDigit(c));
    }
    if (pattern.Length == 1)
    {
        return inputLine.Contains(pattern);
    }
    else
    {
        throw new ArgumentException($"Unhandled pattern: {pattern}");
    }
}

if (args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd();

if (MatchPattern(inputLine, pattern))
{
    Environment.Exit(0);
}
else
{
    Environment.Exit(1);
}
