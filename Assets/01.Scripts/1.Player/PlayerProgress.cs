using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 강화 레벨 기반 데이터를 가져오고 캐싱하는 역할
/// </summary>
public class PlayerProgress : MonoBehaviour
{
    private Dictionary<PlayerStatType, int> statLevels = new();
    private Dictionary<HandType, int> handLevels = new();

    private Dictionary<PlayerStatType, BaseStatTableData> statDataCache = new();
    private Dictionary<HandType, HandTypeUpgradeData> handDataCache = new();

    /// <summary>
    /// DB에서 한 번만 초기 레벨 데이터를 꺼내서 캐싱
    /// </summary>
    public void Init()
    {
        // 모든 스탯 타입 순회
        foreach (PlayerStatType type in System.Enum.GetValues(typeof(PlayerStatType)))
        {
            int lvl = PlayerProgressManager.Instance.GetStatUpgradeLevel(type);
            statLevels[type] = lvl;
            // Debug.Log($"[Progress] {type} 레벨: {lvl}");

            // DB에서 해당 레벨 데이터 조회 후 캐싱
            var data = StatTableDatabase.GetUpgradeData(type, lvl);
            statDataCache[type] = data;
            // Debug.Log($"[Progress] {type} 데이터 캐싱 완료: {(data == null ? "null" : "ok")}");
        }

        // 모든 족보 타입 순회
        foreach (HandType type in System.Enum.GetValues(typeof(HandType)))
        {
            int lvl = PlayerProgressManager.Instance.GetHandTypeUpgradeLevel(type);
            handLevels[type] = lvl;

            // DB에서 해당 레벨 데이터 조회 후 캐싱
            var data = DiceTableDatabase.GetUpgradeData(type, lvl);
            handDataCache[type] = data;
        }
    }

    // public int GetHandTypeAddScore(HandType type)
    // {
    //     if (handDataCache.TryGetValue(type, out var data))
    //         return data?.add_score ?? 0;

    //     return 0;
    // }

    // public int GetHandAddMultiplier(HandType type)
    // {
    //     if (handDataCache.TryGetValue(type, out var data))
    //         return data?.add_multiplier ?? 0;

    //     return 0;
    // }

    /// <summary>
    /// 스탯 강화 레벨을 변경하고, 새 레벨의 데이터를 DB에서 다시 꺼내와 갱신
    /// </summary>
    public void SetStatUpgradeLevel(PlayerStatType type, int level)
    {
        // 레벨 범위 체크
        int max = StatTableDatabase.GetMaxLevel(type);
        statLevels[type] = Mathf.Clamp(level, 0, max);

        // 변경된 레벨의 데이터 재캐싱
        statDataCache[type] = StatTableDatabase.GetUpgradeData(type, statLevels[type]);
    }

    /// <summary>
    /// 족보 강화 레벨을 변경하고, 새 레벨의 데이터를 DB에서 다시 꺼내와 갱신
    /// </summary>
    public void SetHandUpgradeLevel(HandType type, int level)
    {
        int max = DiceTableDatabase.GetMaxLevel(type);
        handLevels[type] = Mathf.Clamp(level, 0, max);

        handDataCache[type] = DiceTableDatabase.GetUpgradeData(type, handLevels[type]);
    }

    /// <summary>
    /// 현재 캐싱된 스탯 강화 데이터를 반환
    /// </summary>
    public BaseStatTableData GetStatData(PlayerStatType type)
    {
        statDataCache.TryGetValue(type, out var data);
        return data;
    }

    /// <summary>
    /// 현재 캐싱된 족보 강화 데이터를 반환
    /// </summary>
    public HandTypeUpgradeData GetHandData(HandType type)
    {
        handDataCache.TryGetValue(type, out var data);
        return data;
    }
}
