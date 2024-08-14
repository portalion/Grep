using codecrafters_grep.src.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_grep.src;

public class Operation
{
    public string Input { get; set; } = string.Empty;
    public Stack<IToken> Tokens { get; set; } = new Stack<IToken>();
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
        var result = new Operation();
        result.Input = Input;
        result.Tokens = Tokens.Clone();
        return result;
    }

    public void AddToken(IToken token)
    {
        Tokens.Push(token);
    }
}
