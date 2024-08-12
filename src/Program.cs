using codecrafters_grep.src;
using System;
using System.IO;

if (args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadLine() ?? "";

var matcher = new PatternMatcher(new TokenParser());

if (matcher.MatchPattern(inputLine, pattern))
{
    Environment.Exit(0);
}
else
{
    Environment.Exit(1);
}
