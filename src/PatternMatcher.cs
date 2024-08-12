
using System.Linq;
using System.Text;
using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class PatternMatcher : IPatternMatcher
{
    ITokenParser _parser;
    public PatternMatcher(ITokenParser parser)
    {
        _parser = parser;
    }

    

    public bool MatchPattern(string inputLine, string pattern)
    {
        //int currentStart = 0;

        //if (pattern[0] == '^')
        //{
        //    return Match(inputLine.Substring(currentStart), new Queue<char>(pattern.Substring(1)));
        //}

        //do
        //{
        //    var patternInStack = new Stack<char>(pattern).Reverse();
        //    if (Match(inputLine.Substring(currentStart), patternInStack))
        //        return true;
        //    currentStart++;
        //} while (currentStart < inputLine.Length);

        return false;
    }
}
