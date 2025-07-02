using System.Collections.Generic;
using Unity.VisualScripting;

public class HandInfo // 족보 정보
{
    public string name; // 족보 이름
    public int baseScore; // 족보 점수
    public int multiplier;// 배율
    public string description;// 족보 설명

    public HandInfo(string name, int baseScore, int multiplier, string description)
    {
        this.name = name;
        this.baseScore = baseScore;
        this.multiplier = multiplier;
        this.description = description;
    }

    public HandInfo(string name, int baseScore, int multiplier)
    {
        this.name = name;
        this.baseScore = baseScore;
        this.multiplier = multiplier;
    }
}

public static class HandDatabase
{
    public static readonly Dictionary<HandType, HandInfo> table = new()
    {
        { HandType.MonoRoll, new("MonoRoll", 200, 8, "같은 숫자 5개 전체 동일") },
        { HandType.FourDice, new("FourDice", 120, 6, "같은 숫자 4개") },
        { HandType.LargeStraight, new("LargeStraight", 100, 5, "연속된 숫자 5개") },
        { HandType.SmallStraight, new("SmallStraight", 80, 5, "연속된 숫자 4개") },
        { HandType.FullHouse, new("FullHouse", 75, 4, "같은 숫자 3개 + 같은 숫자 2개") },
        { HandType.Triple, new("Triple", 70, 4, "같은 숫자 3개") },
        { HandType.TwoPair, new("TwoPair", 60, 2, "같은 숫자 2쌍") },
        { HandType.OnePair, new("OnePair", 40, 2, "같은 숫자 2개") },
        { HandType.HighDice, new("HighDice", 30, 1, "가장 높은 숫자 1개 기준 데미지 부여") }
    };
}

