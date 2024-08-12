
using System.Linq;
using System.Text;

namespace codecrafters_grep.src;

public class PatternMatcher : IPatternMatcher
{
    bool MatchDigit(char characterToCheck)
    {
        return char.IsDigit(characterToCheck);
    }

    bool MatchLetter(char characterToCheck)
    {
        return char.IsLetter(characterToCheck);
    }

    bool MatchGroup(char characterToCheck, string pattern)
    {
        foreach (char c in pattern)
        {
            if (characterToCheck == c) return true;
        }
        return false;
    }


    bool Match(string input, Queue<char> pattern)
    {
        int currentIndex = 0;
        while(pattern.TryDequeue(out var character))
        {
            if (currentIndex >= input.Length)
                return false;

            switch(character)
            {
                case '[':
                    {
                        var first = pattern.Peek();
                        var reverse = false;
                        StringBuilder group = new StringBuilder();
                        if (first == '^')
                        {
                            reverse = true;
                            pattern.Dequeue();
                        }
                        while((first = pattern.Dequeue()) != ']')
                        {
                            group.Append(first);
                        }

                        if (!reverse && !MatchGroup(input[currentIndex], group.ToString()))
                            return false;
                        else if (reverse && MatchGroup(input[currentIndex], group.ToString()))
                            return false;
                    }
                    break;
                case '\\':
                    {
                        var first = pattern.Dequeue();
                        switch(first)
                        {
                            case 'd':
                                if (!MatchDigit(input[currentIndex]))
                                    return false;
                                break;
                            case 'w':
                                if (!MatchLetter(input[currentIndex]))
                                    return false;
                                break;
                        }
                    }
                    break;
                default:
                    if (input[currentIndex] != character)
                        return false;
                    break;
            }
            currentIndex++;
        }
        return true;
    }
    public bool MatchPattern(string inputLine, string pattern)
    {
        int currentStart = 0;
        do
        {
            var patternInQueue = new Queue<char>(pattern);
            if (Match(inputLine.Substring(currentStart), patternInQueue))
                return true;
            currentStart++;
        } while (currentStart < inputLine.Length);

        return false;
    }
}
