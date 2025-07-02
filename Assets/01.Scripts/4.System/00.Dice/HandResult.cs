using System.Collections.Generic;

public class HandResult
{
    public HandType Type;
    public List<int> ScoringValues;

    public HandResult(HandType type, List<int> scoringValues)
    {
        Type = type;
        ScoringValues = scoringValues;
    }
}
