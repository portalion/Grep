using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public abstract class MoveByOneCharacterToken : IToken
{
    public void AfterMatching(OperationManager operationManager)
    {
        var matched = operationManager.CurrentOperation.RemoveFirstInputLetter();
        operationManager.CurrentOperation.AddCharacterToActiveGroups(matched);
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

        var oneOrManyMatchOperation = currentOperation.Clone();
        oneOrManyMatchOperation.AddToken(this);
        oneOrManyMatchOperation.AddToken(tokenToMatch);
        operationManager.AddOperation(oneOrManyMatchOperation);

        var oneMatchOperation = currentOperation.Clone();
        oneMatchOperation.AddToken(tokenToMatch);
        operationManager.AddOperation(oneMatchOperation);
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

        var oneMatchOperation = currentOperation.Clone();
        oneMatchOperation.AddToken(tokenToMatch);
        operationManager.AddOperation(oneMatchOperation);

        var zeroMatchOperation = currentOperation.Clone();
        operationManager.AddOperation(zeroMatchOperation);
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0;
    }
}

public class OrToken : IToken
{
    List<Stack<IToken>> groups = new();
    public OrToken(List<Stack<IToken>> groups)
    {
        this.groups = groups;
    }

    public void AddGroup(Stack<IToken> tokens)
    {
        groups.Add(tokens);
    }

    public void AfterMatching(OperationManager operationManager)
    {
        var currentOperation = operationManager.RemoveTopOperation();
        foreach (var group in groups)
        {
            var operation = currentOperation.Clone();
            foreach (var token in group.Reverse())
            {
                operation.AddToken(token);
            }
            operationManager.AddOperation(operation);
        }
    }

    public bool IsMatching(string input)
    {
        return input.Length != 0;
    }
}

public class StartExpressionToken : IToken
{
    public void AfterMatching(OperationManager operationManager)
    {
        operationManager.CurrentOperation.AddNewGroup();
        operationManager.CurrentOperation.MoveFirstIndexByOne();
    }

    public bool IsMatching(string input)
    {
        return true;
    }
}

public class EndExpressionToken : IToken
{
    public void AfterMatching(OperationManager operationManager)
    {
        operationManager.CurrentOperation.MoveLastIndexByOne();
    }

    public bool IsMatching(string input)
    {
        return true;
    }
}

public class MatchGroupToken : IToken
{
    int groupNumber;
    public MatchGroupToken(int groupNumber)
    {
        this.groupNumber = groupNumber;
    }

    public void AfterMatching(OperationManager operationManager)
    {
        operationManager.CurrentOperation.AddToken(operationManager.CurrentOperation.GetGroupToken(groupNumber));
    }

    public bool IsMatching(string input)
    {
        return true;
    }
}

public class MatchStringToken : IToken
{
    string input;
    public MatchStringToken(string input)
    {
        this.input = input;
    }
    public void AfterMatching(OperationManager operationManager)
    {
        foreach(var c in input.Reverse())
        {
            operationManager.CurrentOperation.AddToken(new CharacterToken(c));
        }
    }

    public bool IsMatching(string input)
    {
        return true;
    }
}