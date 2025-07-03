using System.Collections.Generic;

public class HandResult
{
    public HandType Type;
    public List<int> ScoringValues; // 주사위 눈금 값을 계산하기 위한 주사위 값 정보
    public List<int> Indices; // 주사위 효과를 위한 주사위 인덱스 정보

    public HandResult(HandType type, List<int> scoringValues, List<int> indices)
    {
        Type = type;
        ScoringValues = scoringValues;
        Indices = indices;
    }
}
