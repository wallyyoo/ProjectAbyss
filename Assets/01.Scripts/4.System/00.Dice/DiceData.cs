public enum DiceColor
{
    Red,
    Blue,
    Yellow,
    Black
}


[System.Serializable]
public class DiceData
{
    public int value = 1;
    public bool isLocked = false;
    public DiceColor color = DiceColor.Black;// 현재 주사위 색상은 검정으로 고정
}