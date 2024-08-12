namespace codecrafters_grep.src;

public interface IPatternMatcher
{
    bool MatchPattern(string input, string  pattern);
}
