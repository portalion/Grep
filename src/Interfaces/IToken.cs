namespace codecrafters_grep.src.Interfaces;

public interface IToken
{
    bool IsMatching(string input);
    void AfterMatching(OperationManager operationManager);
}