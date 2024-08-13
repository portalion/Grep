using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_grep.src.Interfaces;

public interface IToken
{
    bool MatchFromLeft(Stack<(string input, Stack<IToken> tokens)> operations);
}