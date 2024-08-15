using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class Operation
{
    Stack<IToken> Tokens { get; set; } = new Stack<IToken>();
    List<string> FoundGroups { get; set; }
    List<int> ActiveGroups = new List<int>();
    int latestGroupIndex = 0;
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
        result.ActiveGroups = ActiveGroups.Clone();
        return result;
    }
    public void AddToken(IToken token)
    {
        Tokens.Push(token);
    }
    public void AddNewGroup(string initialValue = "")
    {
        FoundGroups.Add(initialValue);
        latestGroupIndex++;
    }
    public void MoveFirstIndexByOne()
    {
        ActiveGroups.Add(latestGroupIndex);
    }
    public void MoveLastIndexByOne()
    {
        ActiveGroups.RemoveAt(ActiveGroups.Count - 1);
    }
    public void AddCharacterToActiveGroups(char character)
    {
        foreach (var index in ActiveGroups)
        {
            FoundGroups[index - 1] += character;
        }
    }

    public MatchStringToken GetGroupToken(int index)
    { 
        return new(FoundGroups[index - 1]);
    }
}
