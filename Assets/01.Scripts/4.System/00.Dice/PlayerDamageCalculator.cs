using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerDamageCalculator : MonoBehaviour //플레이어 데미지 계산기 
{
    [Header("기본 주사위 능력치")] 
	public int baseScore;
    public int multiplier;

  // [Header("임시 보너스 능력치 조절")] // 테스트용 
  // private int extraFlatBonus;
  // private float bonusMultiplier;

    private List<DiceColorEffect> colorEffects = new(); // 색상효과 리스트

    private PlayerDamageData currentData; // 최종 데미지 데이터 캐싱

    private static readonly Dictionary<(DiceColorType, ColorEffectTier), System.Action<PlayerDamageData>>
        effectActions =
            new() // 색상별 효과 정리 
            {
                // 빨강: 데미지 배수
                { (DiceColorType.Red, ColorEffectTier.ThreeColor), data => data.redMultiplier = 1.1f },
                { (DiceColorType.Red, ColorEffectTier.FourColor), data => data.redMultiplier = 1.2f },
                { (DiceColorType.Red, ColorEffectTier.FiveColor), data => data.redMultiplier = 1.4f },

                // 파랑: 반격 감소
                { (DiceColorType.Blue, ColorEffectTier.ThreeColor), data => data.counterDamageReduction = 0.1f },
                { (DiceColorType.Blue, ColorEffectTier.FourColor), data => data.counterDamageReduction = 0.25f },
                { (DiceColorType.Blue, ColorEffectTier.FiveColor), data => data.counterDamageReduction = 0.5f },

                // 노랑: 다음 턴 리롤 보너스
                { (DiceColorType.Yellow, ColorEffectTier.ThreeColor), data => data.nextTurnExtraReroll = 1 },
                { (DiceColorType.Yellow, ColorEffectTier.FourColor), data => data.nextTurnExtraReroll = 2 },
                { (DiceColorType.Yellow, ColorEffectTier.FiveColor), data => data.nextTurnExtraReroll = 3 },

                // 검정: 기절 확률
                { (DiceColorType.Black, ColorEffectTier.ThreeColor), data => data.stunChance = 0.3f },
                { (DiceColorType.Black, ColorEffectTier.FourColor), data => data.stunChance = 0.6f },
                { (DiceColorType.Black, ColorEffectTier.FiveColor), data => data.stunChance = 0.8f },
            };

    //  외부에서 모든 값을 한 번에 초기화
    public void Init(HandInfo handInfo, HandResult result, List<DiceColorEffect> effects, int flatBonus,
        float multiplierBonus)
    {
        this.baseScore = handInfo.baseScore;
        this.multiplier = handInfo.multiplier;
        this.colorEffects = effects ?? new();
       // this.extraFlatBonus = flatBonus;
       // this.bonusMultiplier = multiplierBonus;

        UpdateDamageData(result.ScoringValues, handInfo.name);
    }

    private void UpdateDamageData(List<int> scoringValues, string handName) // 색상효과 및 보너스 데미지 포함 계산
    {
        PlayerDamageData result = new PlayerDamageData
        {
            redMultiplier = 1f,
            counterDamageReduction = 0f,
            stunChance = 0f,
            nextTurnExtraReroll = 0
        };

        foreach (var effect in colorEffects) // 색상 효과적용
        {
            if (effectActions.TryGetValue((effect.colorType, effect.tier), out var apply))
                apply.Invoke(result);
        }

        // 최종 데미지 계산 공식
        int bonusScore = scoringValues.Sum() * 2;
        int attackScore = (baseScore + bonusScore) * multiplier;
        float finalAttackScore = attackScore* result.redMultiplier;
       // float finalAttackScore = (attackScore + extraFlatBonus) * bonusMultiplier * result.redMultiplier;
        
        result.finalDamage = Mathf.RoundToInt(finalAttackScore);
        
        result.baseScore = baseScore;
        result.bonusScore = bonusScore;
        result.multiplier = multiplier;
        result.handName = handName;
        
        currentData = result;
    }

    public PlayerDamageData GetPlayerDamageData() // 전투 준비 페이즈에서 최종적으로 데미지를 전달할 메서드
    {
        return currentData ?? new PlayerDamageData(); // 혹시 Init 호출 전이라면 빈 값 반환
    }
}