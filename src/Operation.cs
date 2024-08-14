using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class Operation
{
    Stack<IToken> Tokens { get; set; } = new Stack<IToken>();
    public Operation(string input, Stack<IToken> tokens)
    {
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
    public void RemoveFirstInputLetter()
    {
        Input = Input.Substring(1);
    }
    public Operation Clone()
    { 
        return new Operation(Input, Tokens.Clone());
    }
    public void AddToken(IToken token)
    {
        Tokens.Push(token);
    }
}
