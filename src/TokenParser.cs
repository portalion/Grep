using System.Text;
using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class TokenParser : ITokenParser
{
    IToken GetFirstTokenFromPattern(Stack<char> pattern, Stack<IToken> previousTokens)
    {
        var topOfStack = pattern.Pop();
        switch (topOfStack)
        {
            case '(':
                {
                    OrToken result = new OrToken();
                    char currentlyOnTop;
                    var currentTokens = new Stack<IToken>();
                    while((currentlyOnTop = pattern.Peek()) != ')')
                    {
                        if(currentlyOnTop == '|')
                        {
                            pattern.Pop();
                            result.AddGroup(new Stack<IToken>(currentTokens.Clone()));
                            currentTokens.Clear();
                        }
                        var token = GetFirstTokenFromPattern(pattern, currentTokens);
                        currentTokens.Push(token);
                    }
                    result.AddGroup(new Stack<IToken>(currentTokens.Clone()));
                    pattern.Pop();
                    return result;
                }
            case '[':
                {
                    StringBuilder group = new StringBuilder();
                    while ((topOfStack = pattern.Pop()) != ']')
                    {
                        group.Append(topOfStack);
                    }

                    if (group[0] == '^')
                    {
                        return new ReverseGroupToken(group.ToString().Substring(1));
                    }

                    return new GroupToken(group.ToString());
                }
            case '\\':
                {
                    var prev = topOfStack;
                    topOfStack = pattern.Pop();
                    switch (topOfStack)
                    {
                        case 'd':
                            return new DigitToken();
                        case 'w':
                            return new LetterToken();
                        default:
                            pattern.Push(topOfStack);
                            topOfStack = prev;
                            break;
                    }
                }
                goto default;
            case '$':
                return new EndToken();
            case '+':
                {
                    var previousToken = previousTokens.Pop();
                    return new OneOrMoreToken(previousToken);
                }
            case '?':
                {
                    var previousToken = previousTokens.Pop();
                    return new ZeroOrOneToken(previousToken);
                }
            case '.':
                return new AlwaysTrueToken();
            default:
                return new CharacterToken(topOfStack);
        }
    }

    public Stack<IToken> ParseTokens(Stack<char> pattern)
    {
        var result = new Stack<IToken>();
        while (pattern.TryPeek(out var _))
        {
            var token = this.GetFirstTokenFromPattern(pattern, result);
            result.Push(token);
        }

        return new (result);
    }
}
