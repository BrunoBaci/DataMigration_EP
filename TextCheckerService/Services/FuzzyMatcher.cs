using System;
using System.Collections.Generic;
using System.Linq;
using TextCheckerService.Models;

namespace TextCheckerService.Services;

public class FuzzyMatcher
{
    private List<string> _candidates = new();

    public void LoadCandidates(List<string> candidates)
    {
        _candidates = candidates;
    }

    public MatchTextResult Match(string input)
    {
        var bestMatch = _candidates
            .Select(c => new { Candidate = c, Score = Similarity(input, c) })
            .OrderByDescending(x => x.Score)
            .FirstOrDefault();

        return bestMatch == null
            ? new MatchTextResult { BestMatch = null, Score = 0 }
            : new MatchTextResult { BestMatch = bestMatch.Candidate, Score = bestMatch.Score };
    }

    private double Similarity(string a, string b)
    {
        a = a.ToLower().Trim();
        b = b.ToLower().Trim();
        int maxLen = Math.Max(a.Length, b.Length);
        if (maxLen == 0) return 1.0;

        int dist = Levenshtein(a, b);
        return 1.0 - (double)dist / maxLen;
    }

    private int Levenshtein(string a, string b)
    {
        int[,] dp = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++)
            dp[i, 0] = i;
        for (int j = 0; j <= b.Length; j++)
            dp[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                dp[i, j] = Math.Min(Math.Min(
                    dp[i - 1, j] + 1,
                    dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + cost);
            }
        }

        return dp[a.Length, b.Length];
    }
}
