using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public abstract class MoveByOneCharacterToken : IToken
{
    public void AfterMatching(OperationManager operationManager)
    {
        operationManager.CurrentOperation.RemoveFirstInputLetter();
    }

    public abstract bool IsMatching(string input);
}

public class CharacterToken : MoveByOneCharacterToken
{
    char character;

    public CharacterToken(char character)
    {
        this.character = character;
    }

    public override bool IsMatching(string input)
    {
        return input.Length != 0 && input[0] == character;
    }
}

public class DigitToken : MoveByOneCharacterToken
{
    public override bool IsMatching(string input)
    {
        return input.Length != 0 && char.IsDigit(input[0]);
    }
}

public class LetterToken : MoveByOneCharacterToken
{
    public override bool IsMatching(string input)
    {
        return input.Length != 0 && char.IsLetter(input[0]);
    }
}

public class EndToken : IToken
{
    public void AfterMatching(OperationManager operationManager)
    {
    }

    public bool IsMatching(string input)
    {
        return input.Length == 0;
    }
}

public class GroupToken : MoveByOneCharacterToken
{
    string group;
    public GroupToken(string group)
    {
        this.group = group;
    }

    public override bool IsMatching(string input)
    {
        return input.Length != 0 && group.Contains(input[0]);
    }
}

public class ReverseGroupToken : MoveByOneCharacterToken
{
    string group;
    public ReverseGroupToken(string group)
    {
        this.group = group;
    }

    public override bool IsMatching(string input)
    {
        return input.Length != 0 && !group.Contains(input[0]);
    }
}

public class AlwaysTrueToken : MoveByOneCharacterToken
{
    public override bool IsMatching(string input)
    {
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

    public void AfterMatching(OperationManager operationManager)
    {
        var currentOperation = operationManager.RemoveTopOperation();
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
    public void AfterMatching(OperationManager operationManager)
    {
        var currentOperation = operationManager.RemoveTopOperation();
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

public class OrToken : IToken
{
    List<Stack<IToken>> groups = new();
    public void AddGroup(Stack<IToken> tokens)
    {
        groups.Add(tokens);
    }

    public void AfterMatching(OperationManager operationManager)
    {
        var currentOperation = operationManager.RemoveTopOperation();
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