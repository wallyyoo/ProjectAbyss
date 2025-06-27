using System.Collections.Generic;
using System.Linq;

public static class HandEvaluator // 주사위 값 리스트를 받아서 족보를 판별
{
    public static HandType Evaluate(List<int> values)
    {
        values.Sort(); // 오름차순 정렬

        var counts = values.GroupBy(v => v)
                           .Select(g => g.Count())
                           .OrderByDescending(c => c)
                           .ToList(); // 같은 숫자 개수만 추출, 가장 많은 수가 앞에 오게 정렬

        bool isStraight = values.Distinct().Count() == 5 && 
                          (values.Max() - values.Min() == 4); // 라지 스트레이트
        
        bool isSmallStraight = values.Distinct().Count() >= 4 &&
                               new[] { 1, 2, 3, 4 }.All(values.Contains) ||
                               new[] { 2, 3, 4, 5 }.All(values.Contains) ||
                               new[] { 3, 4, 5, 6 }.All(values.Contains); // 스몰 스트레이트 

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
