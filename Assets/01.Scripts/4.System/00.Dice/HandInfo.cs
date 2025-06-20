using System.Collections.Generic;
public class HandInfo
{
    public string name;
    public int baseScore;
    public int multiplier;

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
        { HandType.MonoRoll, new("모노롤", 150, 8) },
        { HandType.FourDice, new("포 다이스", 120, 6) },
        { HandType.LargeStraight, new("라지 스트레이트", 100, 5) },
        { HandType.SmallStraight, new("스몰 스트레이트", 80, 5) },
        { HandType.FullHouse, new("풀하우스", 75, 5) },
        { HandType.Triple, new("트리플", 70, 4) },
        { HandType.TwoPair, new("투 페어", 60, 2) },
        { HandType.OnePair, new("원 페어", 40, 2) },
        { HandType.HighDice, new("하이다이스", 30, 1) }
    };
}

