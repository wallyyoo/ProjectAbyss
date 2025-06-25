public class HandResult
{
    public HandType type;
    public int baseHandTypeScore;
    public int multiplier;
    
    public int FinalScore => baseHandTypeScore * multiplier;
}
//