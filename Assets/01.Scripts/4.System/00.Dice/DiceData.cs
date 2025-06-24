public enum DiceColor
{
    None,
    Red,
    Blue,
    Yellow,
    Black
}

public enum ColorBuffGrade
{
    None,
    ThreeColor,  
    FourColor,  
    FiveColor 
}

public class ColorBuffResult
{
    public DiceColor color;
    public ColorBuffGrade grade;
}

[System.Serializable]
public class DiceData
{
    public int value = 1;
    public DiceColor color = DiceColor.Black;
}