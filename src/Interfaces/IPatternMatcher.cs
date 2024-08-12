namespace codecrafters_grep.src.Interfaces;

public interface IPatternMatcher
{
    bool MatchPattern(string input, string pattern);
}
