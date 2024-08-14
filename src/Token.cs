using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

using Operation = (string input, Stack<IToken> tokens);

public abstract class Token : IToken
{
    protected abstract bool Condition(Stack<Operation> operations, Operation currentOperation);
    protected abstract void AfterMatching(Stack<Operation> operations, Operation currentOperation);

    public bool MatchFromLeft(Stack<Operation> operations)
    {
        var currentOperation = operations.Pop();
        if (Condition(operations, currentOperation))
            return false;

        currentOperation.tokens.Pop();

        AfterMatching(operations, currentOperation);

        return true;
    }
}

public class CharacterToken : Token
{
    char character;

    public CharacterToken(char character)
    {
        this.character = character;
    }

    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        currentOperation.input = currentOperation.input.Substring(1);
        operations.Push(currentOperation);
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return currentOperation.input.Length == 0 || currentOperation.input[0] != character;
    }
}

public class DigitToken : Token
{
    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        currentOperation.input = currentOperation.input.Substring(1);
        operations.Push(currentOperation);
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return currentOperation.input.Length == 0 || !char.IsDigit(currentOperation.input[0]);
    }
}

public class LetterToken : Token
{
    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        currentOperation.input = currentOperation.input.Substring(1);
        operations.Push(currentOperation);
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return currentOperation.input.Length == 0 || !char.IsLetter(currentOperation.input[0]);
    }
}

public class EndToken : Token
{
    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        operations.Push(currentOperation); 
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return currentOperation.input.Length != 0;
    }
}

public class GroupToken : Token
{
    string group;
    public GroupToken(string group)
    {
        this.group = group;
    }

    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        currentOperation.input = currentOperation.input.Substring(1);
        operations.Push(currentOperation);
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return currentOperation.input.Length == 0 || !group.Contains(currentOperation.input[0]);
    }
}

public class ReverseGroupToken : Token
{
    string group;
    public ReverseGroupToken(string group)
    {
        this.group = group;
    }

    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        currentOperation.input = currentOperation.input.Substring(1);
        operations.Push(currentOperation);
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return currentOperation.input.Length == 0 || group.Contains(currentOperation.input[0]);
    }
}

public class OneOrMoreToken : Token
{
    IToken tokenToMatch;
    public OneOrMoreToken(IToken token)
    {
        tokenToMatch = token;
    }

    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        var oneOrManyMatchOperationTokens = currentOperation.tokens.Clone();
        oneOrManyMatchOperationTokens.Push(this);
        oneOrManyMatchOperationTokens.Push(tokenToMatch);
        operations.Push((currentOperation.input, oneOrManyMatchOperationTokens));

        var oneMatchOperationTokens = currentOperation.tokens.Clone();
        oneMatchOperationTokens.Push(tokenToMatch);
        operations.Push((currentOperation.input, oneMatchOperationTokens));
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return currentOperation.input.Length == 0;
    }
}

public class ZeroOrOneToken : Token
{
    IToken tokenToMatch;
    public ZeroOrOneToken(IToken token)
    {
        tokenToMatch = token;
    }

    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        var oneMatchOperationTokens = currentOperation.tokens.Clone();
        oneMatchOperationTokens.Push(tokenToMatch);
        operations.Push((currentOperation.input, oneMatchOperationTokens));

        var zeroMatchOperationTokens = currentOperation.tokens.Clone();
        operations.Push((currentOperation.input, zeroMatchOperationTokens));
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return currentOperation.input.Length == 0;
    }
}

public class AlwaysTrueToken : Token
{
    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        currentOperation.input = currentOperation.input.Substring(1);
        operations.Push(currentOperation);
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return false;
    }
}

public class OrToken : Token
{
    List<Stack<IToken>> groups = new();
    public void AddGroup(Stack<IToken> tokens)
    {
        groups.Add(tokens);
    }

    protected override void AfterMatching(Stack<Operation> operations, Operation currentOperation)
    {
        foreach (var group in groups)
        {
            var operationTokens = currentOperation.tokens.Clone();
            foreach (var token in group.Reverse())
            {
                operationTokens.Push(token);
            }
            operations.Push((currentOperation.input, operationTokens));
        }
    }

    protected override bool Condition(Stack<Operation> operations, Operation currentOperation)
    {
        return currentOperation.input.Length == 0;
    }
}