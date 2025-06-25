using System.Collections.Generic;
using System.Linq;

public static class HandEvaluator
{
    public static HandType Evaluate(List<int> values)
    {
        values.Sort();

        var counts = values.GroupBy(v => v).Select(g => g.Count()).OrderByDescending(c => c).ToList();

        bool isStraight = values.Distinct().Count() == 5 &&
                          (values.Max() - values.Min() == 4);
        
        bool isSmallStraight = values.Distinct().Count() >= 4 &&
                               new[] { 1, 2, 3, 4 }.All(values.Contains) ||
                               new[] { 2, 3, 4, 5 }.All(values.Contains) ||
                               new[] { 3, 4, 5, 6 }.All(values.Contains);

        // 족보판단 순서
        if (values.Distinct().Count() == 1) return HandType.MonoRoll;
        if (counts[0] == 4) return HandType.FourDice;
        if (isStraight) return HandType.LargeStraight;
        if (isSmallStraight) return HandType.SmallStraight;
        if (counts.Contains(3) && counts.Contains(2)) return HandType.FullHouse;
        if (counts[0] == 3) return HandType.Triple;
        if (counts.Count(c => c == 2) == 2) return HandType.TwoPair;
        if (counts[0] == 2) return HandType.OnePair;

        return HandType.HighDice;
    }
}
//