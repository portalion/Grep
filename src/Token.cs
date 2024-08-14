using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class CharacterToken : IToken
{
    char character;

    public CharacterToken(char character)
    {
        this.character = character;
    }

    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        currentOperation.Input = currentOperation.Input.Substring(1);
        operationManager.AddOperation(currentOperation);
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0 && input[0] == character;
    }
}

public class DigitToken : IToken
{
    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        currentOperation.Input = currentOperation.Input.Substring(1);
        operationManager.AddOperation(currentOperation);
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0 && char.IsDigit(input[0]);
    }
}

public class LetterToken : IToken
{
    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        currentOperation.Input = currentOperation.Input.Substring(1);
        operationManager.AddOperation(currentOperation);
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0 && char.IsLetter(input[0]);
    }
}

public class EndToken : IToken
{
    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        operationManager.AddOperation(currentOperation);
    }

    public bool IsMatching(string input)
    {
        return input.Length == 0;
    }
}

public class GroupToken : IToken
{
    string group;
    public GroupToken(string group)
    {
        this.group = group;
    }

    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        currentOperation.Input = currentOperation.Input.Substring(1);
        operationManager.AddOperation(currentOperation);
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0 && group.Contains(input[0]);
    }
}

public class ReverseGroupToken : IToken
{
    string group;
    public ReverseGroupToken(string group)
    {
        this.group = group;
    }

    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        currentOperation.Input = currentOperation.Input.Substring(1);
        operationManager.AddOperation(currentOperation);
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0 && !group.Contains(input[0]);
    }
}

public class OneOrMoreToken : IToken
{
    IToken tokenToMatch;
    public OneOrMoreToken(IToken token)
    {
        tokenToMatch = token;
    }

    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        var oneOrManyMatchOperationTokens = currentOperation.Tokens.Clone();
        oneOrManyMatchOperationTokens.Push(this);
        oneOrManyMatchOperationTokens.Push(tokenToMatch);
        operationManager.AddOperation(currentOperation.Input, oneOrManyMatchOperationTokens);

        var oneMatchOperationTokens = currentOperation.Tokens.Clone();
        oneMatchOperationTokens.Push(tokenToMatch);
        operationManager.AddOperation(currentOperation.Input, oneMatchOperationTokens);
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0;
    }
}

public class ZeroOrOneToken : IToken
{
    IToken tokenToMatch;
    public ZeroOrOneToken(IToken token)
    {
        tokenToMatch = token;
    }
    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        var oneMatchOperationTokens = currentOperation.Tokens.Clone();
        oneMatchOperationTokens.Push(tokenToMatch);
        operationManager.AddOperation(currentOperation.Input, oneMatchOperationTokens);
        var zeroMatchOperationTokens = currentOperation.Tokens.Clone();
        operationManager.AddOperation(currentOperation.Input, zeroMatchOperationTokens);
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0;
    }
}

public class AlwaysTrueToken : IToken
{
    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        currentOperation.Input = currentOperation.Input.Substring(1);
        operationManager.AddOperation(currentOperation);
    }

    public bool IsMatching(string input)
    {
        return true;
    }
}

public class OrToken : IToken
{
    List<Stack<IToken>> groups = new();
    public void AddGroup(Stack<IToken> tokens)
    {
        groups.Add(tokens);
    }

    public void AfterMatching(OperationManager operationManager, Operation currentOperation)
    {
        foreach (var group in groups)
        {
            var operationTokens = currentOperation.Tokens.Clone();
            foreach (var token in group.Reverse())
            {
                operationTokens.Push(token);
            }
            operationManager.AddOperation(currentOperation.Input, operationTokens);
        }
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0;
    }
}