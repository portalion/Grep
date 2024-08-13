
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
        Stack<(string input, Stack<IToken> tokens)> operations = new Stack<(string input, Stack<IToken> tokens)>();
        if (pattern[0] == '^')
        {
            (string input, Stack<IToken> tokens) operation = new();
            operation.input = inputLine;
            operation.tokens = _parser.ParseTokens(new Stack<char>(pattern.Substring(1).Reverse()));
            operations.Push(operation);
        }
        else
        {
            var tokens = _parser.ParseTokens(new Stack<char>(pattern.Reverse()));
            for (int i = 0; i <  inputLine.Length; i++)
            {
                (string input, Stack<IToken> tokens) operation = new();
                operation.input = inputLine.Substring(i);
                operation.tokens = tokens.Clone();
                operations.Push(operation);
            }
        }
        
        while(operations.TryPeek(out var operation)) 
        {
            if(operation.tokens.TryPeek(out var firstToken)) //if tokens are empty it determines that we found pattern
            {
                firstToken.MatchFromLeft(operations);
            }
            else
            {
                return true;
            }
        }

        return false;
    }
}
