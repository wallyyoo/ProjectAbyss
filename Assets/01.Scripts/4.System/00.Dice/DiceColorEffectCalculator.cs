using System.Collections.Generic;

public static class DiceColorEffectCalculator
{
    public static List<DiceColorEffect> CalculateEffects(List<DiceColorType> colors)
    {
        var grouped = new Dictionary<DiceColorType, int>();
        foreach (var color in colors)
        {
            if (!grouped.ContainsKey(color))
                grouped[color] = 0;
            grouped[color]++;
        }

        var result = new List<DiceColorEffect>();
        foreach (var pair in grouped)
        {
            ColorEffectTier tier = ColorEffectTier.None;
            if (pair.Value >= 5) tier = ColorEffectTier.FiveColor;
            else if (pair.Value == 4) tier = ColorEffectTier.FourColor;
            else if (pair.Value == 3) tier = ColorEffectTier.ThreeColor;

            if (tier != ColorEffectTier.None)
                result.Add(new DiceColorEffect { colorType = pair.Key, tier = tier });
        }

        return result;
    }
}
