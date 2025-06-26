
using UnityEngine;
// 주사위 한 객체에 해당하는 데이터
public enum DiceColor
{   
    Black,
    Yellow,
    Blue,
    Red,
    None
}

public class DiceModel
{
    public int Value { get; private set; }
    public DiceColor Color { get; private set; }

    public void Roll(bool keepColor = true)
    {
        Value = Random.Range(1, 7);
        if (!keepColor)
            Color = (DiceColor)Random.Range(0, 4);
    }

    public void Init()
    {
        Value = 1;
        Color = DiceColor.None;
    }

    public void SetColor(DiceColor color)
    {
        Color = color;
    }
}

