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
}
