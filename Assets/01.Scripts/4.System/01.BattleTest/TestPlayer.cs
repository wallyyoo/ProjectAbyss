using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public int currentHP = 100;
    public int rerollBonus = 3;

    public void GetDamageFromCalculator(PlayerDamageData data)
    {
        Debug.Log($"[플레이어] 데미지 수신: {data.finalDamage} / 스턴 확률: {data.stunChance * 100}% / 추가 리롤: {data.nextTurnExtraReroll}");

        // 예: 적에게 데미지를 입히는 경우
        ApplyAttack(data.finalDamage);

        // 다음 턴 추가 리롤 반영
        rerollBonus += data.nextTurnExtraReroll;
        Debug.Log($"[플레이어] 다음 턴 리롤 +{data.nextTurnExtraReroll}, 누적 리롤 보너스: {rerollBonus}");
    }

    private void ApplyAttack(int damage)
    {
        Debug.Log($"[플레이어] 적에게 {damage} 데미지를 입힘!");

    }
}
