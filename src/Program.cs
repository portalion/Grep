using System;
using System.IO;

static bool MatchDigit(string input)
{
    return input.Any(c => char.IsDigit(c));
}

static bool MatchLetter(string input)
{
    return input.Any(c => char.IsLetter(c));
}

static bool MatchGroup(string input, string pattern)
{
    return input.Contains(pattern);
}

static bool MatchPattern(string inputLine, string pattern)
{
    if (pattern.StartsWith("[") && pattern.EndsWith("]"))
    {
        return MatchGroup(inputLine, pattern.Substring(1, pattern.Length - 2));
    }

    if (pattern == @"\d")
    {
        return MatchDigit(inputLine);
    }

    if (pattern == @"\w")
    {
        return MatchLetter(inputLine);
    }

    if (pattern.Length == 1)
    {
        return inputLine.Contains(pattern);
    }

    throw new ArgumentException($"Unhandled pattern: {pattern}");
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
