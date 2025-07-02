using System.Collections.Generic;
public class HandInfo // 족보 정보
{
    public string name; // 족보 이름
    public int baseScore; // 족보 점수
    public int multiplier;// 배율

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
        { HandType.MonoRoll, new("MonoRoll", 150, 8) },
        { HandType.FourDice, new("FourDice", 120, 6) },
        { HandType.LargeStraight, new("L.Straight", 100, 5) },
        { HandType.SmallStraight, new("S.Straight", 80, 5) },
        { HandType.FullHouse, new("FullHouse", 75, 5) },
        { HandType.Triple, new("Triple", 70, 4) },
        { HandType.TwoPair, new("TwoPair", 60, 2) },
        { HandType.OnePair, new("OnePair", 40, 2) },
        { HandType.HighDice, new("HighDice", 30, 1) }
    };
}

