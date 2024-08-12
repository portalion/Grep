using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class CharacterToken : IToken
{
    char character;

    public CharacterToken(char character)
    {
        this.character = character;
    }

    public bool MatchFromLeft(string input)
    {
        return input[0] == character;
    }
}

public class DigitToken : IToken
{
    public bool MatchFromLeft(string input)
    {
        return char.IsDigit(input[0]);
    }
}

public class LetterToken : IToken
{
    public bool MatchFromLeft(string input)
    {
        return char.IsLetter(input[0]);
    }
}

public class EndToken : IToken
{
    public bool MatchFromLeft(string input)
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

    public bool MatchFromLeft(string input)
    {
        return group.Contains(input[0]);
    }
}

public class ReverseGroupToken : GroupToken
{
    public ReverseGroupToken(string group)
        :base(group)
    {
    }

    public new bool MatchFromLeft(string input)
    {
        return !base.MatchFromLeft(input);
    }
}
