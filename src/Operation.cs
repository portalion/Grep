using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class Operation
{
    Stack<IToken> Tokens { get; set; } = new Stack<IToken>();
    List<string> FoundGroups { get; set; }
    int firstIndex = 0;
    int lastIndex = 0;
    public Operation(string input, Stack<IToken> tokens, List<string>? foundGroups)
    {
        FoundGroups = foundGroups ?? new List<string>();
        Input = input;
        Tokens = tokens;
    }
    public string Input { get; private set; } = string.Empty;
    public bool TokensAreEmpty => Tokens.Count == 0;
    public IToken FirstToken => Tokens.Peek();
    public IToken RemoveFirstToken()
    {
        return Tokens.Pop();
    }
    public char RemoveFirstInputLetter()
    {
        var result = Input[0];
        Input = Input.Substring(1);
        return result;
    }
    public Operation Clone()
    {
        var result = (Operation) MemberwiseClone();
        result.Tokens = Tokens.Clone();
        result.FoundGroups = FoundGroups.Clone();
        return new Operation(Input, Tokens.Clone(), FoundGroups.Clone());
    }
    public void AddToken(IToken token)
    {
        Tokens.Push(token);
    }
    public void AddNewGroup(string initialValue = "")
    {
        FoundGroups.Add(initialValue);
    }
    public void MoveFirstIndexByOne()
    {
        firstIndex++;
    }
    public void MoveLastIndexByOne()
    {
        lastIndex++;
    }
    public void AddCharacterToActiveGroups(char character)
    {
        for (int i = 0; i < FoundGroups.Count; i++)
        {
            if(i >= lastIndex && i <= firstIndex)
            {
                FoundGroups[i] += character;
            }
        }
    }

    public MatchStringToken GetGroupToken(int index)
    { 
        return new(FoundGroups[index - 1]);
    }
}
