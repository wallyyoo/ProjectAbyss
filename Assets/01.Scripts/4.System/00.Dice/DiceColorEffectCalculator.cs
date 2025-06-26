using System.Collections.Generic;

public static class DiceColorEffectCalculator // 주사위 색상효과 계산기
{
    public static List<DiceColorEffect> CalculateEffects(List<DiceColorType> colors)// 색상효과를 리스트로 반환
    {
        var grouped = new Dictionary<DiceColorType, int>(); // 색상별 개수를 저장하는 딕셔너리
        foreach (var color in colors)
        {
            if (!grouped.ContainsKey(color))
                grouped[color] = 0;
            grouped[color]++;
        }

        var result = new List<DiceColorEffect>(); //색상 효과 리스트
        foreach (var pair in grouped) // 만들어진 딕셔너리 안에 색상 개수를 기반으로 효과 티어 계산
        {
            ColorEffectTier tier = ColorEffectTier.None;
            if (pair.Value >= 5) tier = ColorEffectTier.FiveColor;
            else if (pair.Value == 4) tier = ColorEffectTier.FourColor;
            else if (pair.Value == 3) tier = ColorEffectTier.ThreeColor;

            if (tier != ColorEffectTier.None)
                result.Add(new DiceColorEffect { colorType = pair.Key, tier = tier }); // 색상효과를 리스트에 넣기
        }

        return result;
    }
}
