using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_grep.src.Interfaces;

public interface IToken
{
    bool IsMatching(string input);
    void AfterMatching(OperationManager operationManager);
}