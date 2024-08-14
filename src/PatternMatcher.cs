
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
            for (int i = inputLine.Length - 1; i >= 0; i--)
            {
                operationManager.AddOperation(inputLine.Substring(i), tokens.Clone());
            }
        }
        
        while(!operationManager.IsEmpty) 
        {
            var currentOperation = operationManager.CurrentOperation;
            
            if (operationManager.FoundMatch) //if tokens are empty it determines that we found pattern
            {
                return true;
            }

            operationManager.RemoveTopOperation();
            var firstToken = currentOperation.RemoveFirstToken();
            
            if (!firstToken.IsMatching(currentOperation.Input))
            {
                continue;
            }

            firstToken.AfterMatching(operationManager, currentOperation);
        }

        return false;
    }
}
