
public class PlayerDamageData
{
    public int finalDamage;     //최종 데미지
    public float redMultiplier; //빨간색 주사위 : 데미지 증가
    public float counterDamageReduction;    //파란색 주사위 : 받는 데미지감소
    public float stunChance;    //검은색 주사위 : 적 스턴 확률
    public int nextTurnExtraReroll; //노란색 주사위 : 다음턴 리롤 횟수 증가

    
    public override string ToString()
    {
        return $"[Damage: {finalDamage}] [Red:데미지x{ redMultiplier}] [blue:받는 피해감소{counterDamageReduction}] [black:스턴확률{stunChance}] [yellow:리롤갯수 {nextTurnExtraReroll}]";
    }
}
// playerDamageData = damageCalculator.GetPlayerDamageData(); 으로 데미지 가져오기