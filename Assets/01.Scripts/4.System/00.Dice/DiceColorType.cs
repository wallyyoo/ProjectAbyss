public enum DiceColorType
{
    Red,    // 데미지 증가
    Blue,   // 반격 데미지 감소
    Yellow, // 리롤 추가
    Black   // 기절
}

public enum ColorEffectTier
{
    None,
    ThreeColor,
    FourColor,
    FiveColor
}

public struct DiceColorEffect
{
    public DiceColorType colorType;
    public ColorEffectTier tier;

    public DiceColorEffect(DiceColorType type, ColorEffectTier tier)
    {
        this.colorType = type;
        this.tier = tier;
    }
}