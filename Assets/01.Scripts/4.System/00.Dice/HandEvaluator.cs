using System.Collections.Generic;
using System.Linq;

public static class HandEvaluator // 주사위 값 리스트를 받아서 족보를 판별
{
      public static HandResult Evaluate(List<int> values)
    {
        List<int> original = new(values);
        values.Sort();
        var counts = values.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());

        bool isStraight = values.Distinct().Count() == 5 && values.Max() - values.Min() == 4;
        bool isSmallStraight = values.Distinct().Count() >= 4 &&
            (new[] {1,2,3,4}.All(values.Contains) ||
             new[] {2,3,4,5}.All(values.Contains) ||
             new[] {3,4,5,6}.All(values.Contains));

        if (values.Distinct().Count() == 1)
        {
            int target = values[0];
            var indices = GetIndices(original, target, 5);
            return new HandResult(HandType.MonoRoll, Enumerable.Repeat(target, 5).ToList(), indices);
        }

        if (counts.Values.Contains(4))
        {
            int target = counts.First(p => p.Value == 4).Key;
            var indices = GetIndices(original, target, 4);
            return new HandResult(HandType.FourDice, Enumerable.Repeat(target, 4).ToList(), indices);
        }

        if (isStraight)
        {
            var indices = GetIndices(original, values);
            return new HandResult(HandType.LargeStraight, new List<int>(values), indices);
        }

        if (isSmallStraight)
        {
            var straightSets = new[]
            {
                new List<int>{1,2,3,4},
                new List<int>{2,3,4,5},
                new List<int>{3,4,5,6}
            };

            var matched = straightSets.First(set => set.All(values.Contains));
            var indices = GetIndices(original, matched);
            return new HandResult(HandType.SmallStraight, matched, indices);
        }

        if (counts.Values.Contains(3) && counts.Values.Contains(2))
        {
            int triple = counts.First(p => p.Value == 3).Key;
            int pair = counts.First(p => p.Value == 2).Key;
            var scoringValues = Enumerable.Repeat(triple, 3).Concat(Enumerable.Repeat(pair, 2)).ToList();
            var indices = GetIndices(original, scoringValues);
            return new HandResult(HandType.FullHouse, scoringValues, indices);
        }

        if (counts.Values.Contains(3))
        {
            int target = counts.First(p => p.Value == 3).Key;
            var indices = GetIndices(original, target, 3);
            return new HandResult(HandType.Triple, Enumerable.Repeat(target, 3).ToList(), indices);
        }

        if (counts.Values.Count(v => v == 2) == 2)
        {
            var pairs = counts.Where(p => p.Value == 2)
                              .SelectMany(p => Enumerable.Repeat(p.Key, 2)).ToList();
            var indices = GetIndices(original, pairs);
            return new HandResult(HandType.TwoPair, pairs, indices);
        }

        if (counts.Values.Contains(2))
        {
            int target = counts.First(p => p.Value == 2).Key;
            var indices = GetIndices(original, target, 2);
            return new HandResult(HandType.OnePair, Enumerable.Repeat(target, 2).ToList(), indices);
        }

        
        int high = values.Max();
        var index = original.FindIndex(v => v == high);
        return new HandResult(HandType.HighDice, new List<int> { high }, new List<int> { index });
    }

    private static List<int> GetIndices(List<int> original, int target, int count)//  도우미 메서드: 값이 같은 인덱스를 찾아서 n개만 추출
    {
        return original
               .Select((val, idx) => new { val, idx })
               .Where(x => x.val == target)
               .Take(count)
               .Select(x => x.idx)
               .ToList();
    }
    private static List<int> GetIndices(List<int> original, IEnumerable<int> targets)  //  도우미 메서드: 여러 개의 값 조합 (풀하우스 등)
    {
        var remaining = new List<int>(targets); // 몇 개씩 가져갈지 제어
        return original
               .Select((val, idx) => new { val, idx })
               .Where(x =>
               {
                   if (remaining.Contains(x.val))
                   {
                       remaining.Remove(x.val);
                       return true;
                   }
                   return false;
               })
               .Select(x => x.idx)
               .ToList();
    }
    
}


