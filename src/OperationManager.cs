using codecrafters_grep.src.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_grep.src;

public class OperationManager
{
    Stack<Operation> operations = new Stack<Operation>();

    public bool IsEmpty => operations.Count == 0;
    public Operation CurrentOperation => operations.Peek();

    public void AddOperation(Operation operation)
    {
        operations.Push(operation);
    }

    public void AddOperation(string input, Stack<IToken> tokens)
    {
        operations.Push(new Operation { Input = input, Tokens = tokens });
    }

    public void RemoveTopOperation()
    {
        operations.Pop();
    }
}
