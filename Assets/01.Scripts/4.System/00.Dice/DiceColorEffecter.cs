using System.Collections.Generic;

public static class DiceColorEffecter
{
    public static List<DiceColorEffect> Analyze(List<DiceModel> diceList)
    {
        Dictionary<DiceColor, int> colorCounts = new();

        // 색상별 개수 세기
        foreach (var dice in diceList)
        {
            if (dice.Color == DiceColor.None) continue;

            if (!colorCounts.ContainsKey(dice.Color))
                colorCounts[dice.Color] = 0;

            colorCounts[dice.Color]++;
        }

        List<DiceColorEffect> result = new();

        foreach (var (color, count) in colorCounts)
        {
            if (count < 3) continue; // 3개 미만이면 효과 없음

            ColorEffectTier tier = count switch
            {
                >= 5 => ColorEffectTier.FiveColor,
                4 => ColorEffectTier.FourColor,
                3 => ColorEffectTier.ThreeColor,
                _ => ColorEffectTier.None
            };
            
            DiceColorType type = color switch
            {
                DiceColor.Red => DiceColorType.Red,
                DiceColor.Blue => DiceColorType.Blue,
                DiceColor.Yellow => DiceColorType.Yellow,
                DiceColor.Black => DiceColorType.Black,
                _ => throw new System.Exception($"지원하지 않는 색상: {color}")
            };

            result.Add(new DiceColorEffect(type, tier));
        }

        return result;
    }
}