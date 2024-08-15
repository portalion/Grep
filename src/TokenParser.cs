using System.Text;
using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class TokenParser : ITokenParser
{
    List<Stack<IToken>> ParseExpression(Stack<char> pattern)
    {
        List<Stack<IToken>> expressions = [new Stack<IToken>()];
        while (pattern.TryPeek(out var _))
        {
            var tokens = expressions.Last();
            var topOfPattern = pattern.Pop();
            switch (topOfPattern)
            {
                case '(':
                    {
                        tokens.Push(new StartExpressionToken());
                        tokens.Push(new OrToken(ParseExpression(pattern)));
                        tokens.Push(new EndExpressionToken());
                    }
                    break;
                case ')':
                    expressions[expressions.Count - 1] = new Stack<IToken>(expressions.Last());
                    return expressions;
                case '|':
                    expressions[expressions.Count - 1] = new(tokens);
                    expressions.Add(new Stack<IToken>());
                    break;
                case '[':
                    {
                        StringBuilder group = new StringBuilder();
                        while ((topOfPattern = pattern.Pop()) != ']')
                        {
                            group.Append(topOfPattern);
                        }

                        if (group[0] == '^')
                        {
                            tokens.Push( new ReverseGroupToken(group.ToString().Substring(1)));
                        }
                        else
                        {
                            tokens.Push( new GroupToken(group.ToString()));
                        }
                    }
                    break;
                case '\\':
                    {
                        var prev = topOfPattern;
                        topOfPattern = pattern.Peek();

                        if(char.IsDigit(topOfPattern))
                        {
                            StringBuilder groupNumber = new();
                            while(char.IsDigit(pattern.Peek()))
                            {
                                groupNumber.Append(pattern.Pop());
                            }
                            tokens.Push(new MatchGroupToken(int.Parse(groupNumber.ToString())));
                            break;
                        }

                        pattern.Pop();
                        switch (topOfPattern)
                        {
                            case 'd':
                                tokens.Push( new DigitToken());
                                break;
                            case 'w':
                                tokens.Push( new LetterToken());
                                break;
                            default:
                                pattern.Push(topOfPattern);
                                topOfPattern = prev;
                                break;
                        }
                    }
                    break;
                case '$':
                    tokens.Push( new EndToken());
                    break;
                case '+':
                    {
                        var previousToken = tokens.Pop();
                        tokens.Push( new OneOrMoreToken(previousToken));
                    }
                    break;
                case '?':
                    {
                        var previousToken = tokens.Pop();
                        tokens.Push( new ZeroOrOneToken(previousToken));
                    }
                    break;
                case '.':
                    tokens.Push(new AlwaysTrueToken());
                    break;
                default:
                    tokens.Push(new CharacterToken(topOfPattern));
                    break;
            }
        }

        expressions[expressions.Count - 1] = new Stack<IToken>(expressions.Last());
        return expressions;
    }

    public Stack<IToken> ParseTokens(Stack<char> pattern)
    {
        return ParseExpression(pattern).First();
    }
}
