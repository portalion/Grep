﻿using codecrafters_grep.src.Interfaces;

namespace codecrafters_grep.src;

public class OperationManager
{
    Stack<Operation> operations = new Stack<Operation>();

    public bool IsEmpty => operations.Count == 0;
    public Operation CurrentOperation => operations.Peek();
    public bool FoundMatch => CurrentOperation.TokensAreEmpty;

    public void AddOperation(Operation operation)
    {
        operations.Push(operation);
    }

    public void AddOperation(string input, Stack<IToken> tokens)
    {
        operations.Push(new Operation(input, tokens, null));
    }

    public Operation RemoveTopOperation()
    {
        return operations.Pop();
    }
}
