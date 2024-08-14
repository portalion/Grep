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
            if (operationManager.FoundMatch)
            {
                return true;
            }

            var firstToken = operationManager.CurrentOperation.RemoveFirstToken();
            
            if (!firstToken.IsMatching(operationManager.CurrentOperation.Input))
            {
                operationManager.RemoveTopOperation();
                continue;
            }

            firstToken.AfterMatching(operationManager);
        }

        return false;
    }
}
