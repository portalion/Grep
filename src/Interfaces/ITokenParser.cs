﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_grep.src.Interfaces;

public interface ITokenParser
{
    Stack<IToken> ParseTokens(Stack<char> pattern);
}
