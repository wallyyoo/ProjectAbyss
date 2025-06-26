using UnityEngine;
using System.Collections.Generic;

public class TestCombatFlow : MonoBehaviour
{
    [Header("참조")]
    public PlayerDamageCalculator damageCalculator;
    public ScoreEffectController scoreEffectController;

    [Header("임시 테스트용 외부 보너스")]
    public int bonusFlatDamage = 0;
    public float bonusMultiplier = 1f;

    void Start()
    {
        // === 1. 테스트용 주사위 값과 색상 ===
        List<int> diceValues = new() { 3, 3, 3, 1, 6 }; // 예: 트리플 족보
        List<DiceColorType> diceColors = new() {
            DiceColorType.Yellow, 
            DiceColorType.Yellow, 
            DiceColorType.Yellow,
            DiceColorType.Yellow, 
            DiceColorType.Yellow
        };

        // === 2. 족보 계산 ===
        HandType hand = HandEvaluator.Evaluate(diceValues);
        HandInfo info = HandDatabase.table[hand]; // 여기!

        int baseScore = info.baseScore;
        int multiplier = info.multiplier;

        Debug.Log($"[Hand] {hand} → BaseScore: {baseScore}, Multiplier: {multiplier}");

        // === 3. 색상 효과 계산 ===
        List<DiceColorEffect> colorEffects = DiceColorEffectCalculator.CalculateEffects(diceColors);

        // === 4. 데미지 계산기 초기화 ===
        damageCalculator.Init(baseScore, multiplier, colorEffects, bonusFlatDamage, bonusMultiplier);

        // === 5. 최종 데이터 확인 ===
        PlayerDamageData playerData = damageCalculator.GetPlayerDamageData();
        Debug.Log(playerData.ToString());

        // === 6. UI에 표시 ===
        if (scoreEffectController != null)
        {
            scoreEffectController.PreviewHand(hand.ToString(), baseScore * multiplier, multiplier);
        }
    }
}