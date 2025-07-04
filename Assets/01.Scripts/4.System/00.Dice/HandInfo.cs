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

}

public static class HandDatabase
{
    public static readonly Dictionary<HandType, HandInfo> table = new()
    {
        { HandType.MonoRoll, new("MonoRoll", 150, 8, "모두 같은 숫자로 구성된 조합") },
        { HandType.FourDice, new("FourDice", 120, 6, "같은 숫자가 4개인 조합") },
        { HandType.LargeStraight, new("LargeStraight", 100, 5, "연속된 숫자 5개가 있는 조합") },
        { HandType.SmallStraight, new("SmallStraight", 80, 5, "연속된 숫자 4개가 있는 조합") },
        { HandType.FullHouse, new("FullHouse", 75, 5, "같은 숫자 3개 + 2개로 구성된 조합") },
        { HandType.Triple, new("Triple", 70, 4, "같은 숫자가 3개인 조합") },
        { HandType.TwoPair, new("TwoPair", 60, 2, "같은 숫자가 두 쌍인 조합") },
        { HandType.OnePair, new("OnePair", 40, 2, "같은 숫자가 한 쌍인 조합") },
        { HandType.HighDice, new("HighDice", 30, 1, "가장 높은 숫자로 점수를 계산") }
    };
}

