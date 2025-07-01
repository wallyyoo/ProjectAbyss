using System.Collections.Generic;
using System.Linq;

public static class HandEvaluator // 주사위 값 리스트를 받아서 족보를 판별
{
      public static HandResult Evaluate(List<int> values)
    {
        values.Sort();
        var counts = values.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());

        bool isStraight = values.Distinct().Count() == 5 && values.Max() - values.Min() == 4;
        bool isSmallStraight = values.Distinct().Count() >= 4 &&
            (new[] {1,2,3,4}.All(values.Contains) ||
             new[] {2,3,4,5}.All(values.Contains) ||
             new[] {3,4,5,6}.All(values.Contains));

        if (values.Distinct().Count() == 1)
            return new HandResult(HandType.MonoRoll, values);

        if (counts.Values.Contains(4))
        {
            int val = counts.First(p => p.Value == 4).Key;
            return new HandResult(HandType.FourDice, Enumerable.Repeat(val, 4).ToList());
        }

        if (isStraight)
            return new HandResult(HandType.LargeStraight, values);

        if (isSmallStraight)
            return new HandResult(HandType.SmallStraight, values);

        if (counts.Values.Count(v => v == 3) == 1 && counts.Values.Count(v => v == 2) == 1)
        {
            int triple = counts.First(p => p.Value == 3).Key;
            int pair = counts.First(p => p.Value == 2).Key;
            return new HandResult(HandType.FullHouse, Enumerable.Repeat(triple, 3).Concat(Enumerable.Repeat(pair, 2)).ToList());
        }

        if (counts.Values.Contains(3))
        {
            int val = counts.First(p => p.Value == 3).Key;
            return new HandResult(HandType.Triple, Enumerable.Repeat(val, 3).ToList());
        }

        if (counts.Values.Where(v => v == 2).Count() == 2)
        {
            var pairs = counts.Where(p => p.Value == 2)
                              .SelectMany(p => Enumerable.Repeat(p.Key, 2)).ToList();
            return new HandResult(HandType.TwoPair, pairs);
        }

        if (counts.Values.Contains(2))
        {
            int val = counts.First(p => p.Value == 2).Key;
            return new HandResult(HandType.OnePair, Enumerable.Repeat(val, 2).ToList());
        }

        return new HandResult(HandType.HighDice, new List<int> { values.Max() });
    }
}


