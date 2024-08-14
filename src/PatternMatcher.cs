
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
        OperationManager operationManager = new OperationManager();
        if (pattern[0] == '^')
        {
            var patternWithoutFirstCharacter = new Stack<char>(pattern.Substring(1).Reverse());
            var tokens = _parser.ParseTokens(patternWithoutFirstCharacter);
            operationManager.AddOperation(inputLine, tokens);
        }
        else
        {
            var tokens = _parser.ParseTokens(new Stack<char>(pattern.Reverse()));
            for (int i = 0; i < inputLine.Length; i++)
            {
                operationManager.AddOperation(inputLine.Substring(i), tokens.Clone());
            }
        }
        
        while(!operationManager.IsEmpty) 
        {
            var currentOperation = operationManager.CurrentOperation;
            if (currentOperation.TokensAreEmpty) //if tokens are empty it determines that we found pattern
            {
                return true;
            }

            operationManager.RemoveTopOperation();
            if (!currentOperation.FirstToken.IsMatching(currentOperation.Input))
            {
                continue;
            }

            currentOperation.Tokens.Pop();

            currentOperation.FirstToken.AfterMatching(operationManager, currentOperation);
        }

        return false;
    }
}
