using System.Collections.Generic;

public static class DiceColorEffecter // 주사위 색상별 효과 분석
{
    public static List<DiceColorEffect> Analyze(List<DiceModel> diceList) 
    {
        Dictionary<DiceColor, int> colorCounts = new(); // 색상이 몇개 나왔는지 세기 위한 딕셔너리

        // 색상별 개수 세기
        foreach (var dice in diceList)
        {
            if (dice.Color == DiceColor.None) continue; // 주사위 색이 없으면 건너뛰기

            if (!colorCounts.ContainsKey(dice.Color))
                colorCounts[dice.Color] = 0;

            colorCounts[dice.Color]++;
        }

        List<DiceColorEffect> result = new();

        foreach (var (color, count) in colorCounts) // 나온 색상의 수에 따라 효과 판정
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
                DiceColor.Yello => DiceColorType.Yellow,
                DiceColor.Black => DiceColorType.Black,
                _ => throw new System.Exception($"지원하지 않는 색상: {color}")
            };

            result.Add(new DiceColorEffect(type, tier)); //결과값에 색상의 종류, 티어 추가
        }

        return result; // 결과 반환
    }
}