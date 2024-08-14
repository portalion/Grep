namespace codecrafters_grep.src.Interfaces;

public interface ITokenParser
{
    Stack<IToken> ParseTokens(Stack<char> pattern);
}
