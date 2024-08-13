using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_grep.src.Interfaces;

public interface ITokenParser
{
    IToken GetFirstTokenFromPattern(Stack<char> pattern);
    Stack<IToken> ParseTokens(Stack<char> pattern);
}
