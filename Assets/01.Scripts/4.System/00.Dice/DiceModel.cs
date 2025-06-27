
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
        Value = Random.Range(1, 7); //주사위 값 1~6
        if (!keepColor)
            Color = (DiceColor)Random.Range(0, 4); // 색상 값 1~4
    }

    public void Init()// 초기화 시 
    {
        Value = 1;      // 주사위 값은 1
        Color = DiceColor.None; // 컬러는 없음
    }
    
    // 추후 색상을 고정해야하는 능력이 추가된다면 사용.
   // public void SetColor(DiceColor color)
   // {
   //     Color = color;
   // }
}

