﻿using System.Text;
using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class TokenParser : ITokenParser
{
    public IToken GetFirstTokenFromPattern(Stack<char> pattern)
    {
        var topOfStack = pattern.Pop();
        switch (topOfStack)
        {
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

                    return new ReverseGroupToken(group.ToString());
                }
            case '\\':
                {
                    topOfStack = pattern.Pop();
                    switch (topOfStack)
                    {
                        case 'd':
                            return new DigitToken();
                        case 'w':
                            return new LetterToken();
                        default:
                            throw new InvalidDataException();
                    }
                }
            case '$':
                return new EndToken();
            case '+':
            case '*':
            default:
                return new CharacterToken(topOfStack);
        }
    }

    public Queue<IToken> ParseTokens(Stack<char> pattern)
    {
        Stack<IToken> tokens = new Stack<IToken>();

        while (pattern.TryPeek(out var _))
        {
            var token = this.GetFirstTokenFromPattern(pattern);
            tokens.Push(token);
        }

        return new Queue<IToken>(new Queue<IToken>(tokens).Reverse());
    }
}
