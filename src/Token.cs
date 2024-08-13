using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class CharacterToken : IToken
{
    char character;

    public CharacterToken(char character)
    {
        this.character = character;
    }

    public bool MatchFromLeft(Stack<(string input, Stack<IToken> tokens)> operations)
    {
        var currentOperation = operations.Pop();
        if (currentOperation.input.Length == 0 || currentOperation.input[0] != character)
            return false;

        currentOperation.tokens.Pop();
        currentOperation.input = currentOperation.input.Substring(1);

        operations.Push(currentOperation);
        return true;
    }
}

public class DigitToken : IToken
{
    public bool MatchFromLeft(Stack<(string input, Stack<IToken> tokens)> operations)
    {
        var currentOperation = operations.Pop();
        if (currentOperation.input.Length == 0 || !char.IsDigit(currentOperation.input[0]))
            return false;

        currentOperation.tokens.Pop();
        currentOperation.input = currentOperation.input.Substring(1);

        operations.Push(currentOperation);
        return true;
    }
}

public class LetterToken : IToken
{
    public bool MatchFromLeft(Stack<(string input, Stack<IToken> tokens)> operations)
    {
        var currentOperation = operations.Pop();

        if (currentOperation.input.Length == 0 || !char.IsLetter(currentOperation.input[0]))
            return false;

        currentOperation.tokens.Pop();
        currentOperation.input = currentOperation.input.Substring(1);
        operations.Push(currentOperation);
        return true;
    }
}

public class EndToken : IToken
{
    public bool MatchFromLeft(Stack<(string input, Stack<IToken> tokens)> operations)
    {
        var currentOperation = operations.Pop();
        if (currentOperation.input.Length != 0)
            return false;

        currentOperation.tokens.Pop();
        operations.Push(currentOperation);
        return true;
    }
}

public class GroupToken : IToken
{
    string group;
    public GroupToken(string group)
    {
        this.group = group;
    }

    public bool MatchFromLeft(Stack<(string input, Stack<IToken> tokens)> operations)
    {
        var currentOperation = operations.Pop();
        if (currentOperation.input.Length == 0 || !group.Contains(currentOperation.input[0]))
            return false;

        currentOperation.tokens.Pop();
        currentOperation.input = currentOperation.input.Substring(1);

        operations.Push(currentOperation);
        return true;
    }
}

public class ReverseGroupToken : IToken
{
    string group;
    public ReverseGroupToken(string group)
    {
        this.group = group;
    }

    public bool MatchFromLeft(Stack<(string input, Stack<IToken> tokens)> operations)
    {
        var currentOperation = operations.Pop();
        if (currentOperation.input.Length == 0 || group.Contains(currentOperation.input[0]))
            return false;

        currentOperation.tokens.Pop();
        currentOperation.input = currentOperation.input.Substring(1);

        operations.Push(currentOperation);
        return true;
    }
}

public class OneOrMoreToken : IToken
{
    IToken tokenToMatch;
    public OneOrMoreToken(IToken token)
    {
        tokenToMatch = token;
    }

    public bool MatchFromLeft(Stack<(string input, Stack<IToken> tokens)> operations)
    {
        var currentOperation = operations.Pop();
        if (currentOperation.input.Length == 0)
            return false;

        currentOperation.tokens.Pop();
        var oneOrManyMatchOperationTokens = currentOperation.tokens.Clone();
        oneOrManyMatchOperationTokens.Push(this);
        oneOrManyMatchOperationTokens.Push(tokenToMatch);
        operations.Push((currentOperation.input, oneOrManyMatchOperationTokens));

        var oneMatchOperationTokens = currentOperation.tokens.Clone();
        oneOrManyMatchOperationTokens.Push(tokenToMatch);
        operations.Push((currentOperation.input, oneMatchOperationTokens));
        return true;
    }
}