using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager
{
    /// <summary>
    /// 족보를 강화, 마력 수정 차감
    /// </summary>
    /// <param name="type">강화할 족보 타입</param>
    /// <returns>강화 성공 여부</returns>
    public static bool TryUpgrade(HandType type, Action onSuccess = null, Action onFail = null)
    {
        var progress = PlayerProgressManager.Instance;

        int currentLevel = progress.GetUpgradeLevel(type);
        int maxLevel = DiceTableDatabase.GetMaxLevel(type);

        if (currentLevel >= maxLevel)
        {
            Debug.Log("이미 최대 강화 레벨입니다.");
            onFail?.Invoke();
            return false;
        }

        // 다음 강화에 필요한 데이터
        var nextData = DiceTableDatabase.Get(type, currentLevel + 1);

        if (nextData == null)
        {
            Debug.LogError($"강화 데이터 누락: {type}, 레벨 {currentLevel + 1}");
            onFail?.Invoke();
            return false;
        }

        // 마력 수정 부족하면 실패
        if (!progress.SpendEnchantCore(nextData.manaCount))
        {
            Debug.Log("마력 수정 부족");
            onFail?.Invoke();
            return false;
        }

        // 강화 성공 → 레벨 증가
        progress.UpgradeLevelUp(type);

        // UI 갱신
        UIManager.Instance.UpdateEnchantCore(progress.GetEnchantCore());
        UpgradeDatabase.UpdateUpgradeData(type);
        UIManager.Instance.UpgradeHandUI.Refresh();
        onSuccess?.Invoke();

        return true;
    }

    /// <summary>
    /// 강화 취소, 마력 수정 반환
    /// </summary>
    /// <param name="type">대상 족보 타입</param>
    /// <returns>취소 성공 여부</returns>
    public static bool TryDowngrade(HandType type, Action onSuccess = null, Action onFail = null)
    {
        var progress = PlayerProgressManager.Instance;

        int currentLevel = progress.GetUpgradeLevel(type);

        if (currentLevel <= 0)
        {
            Debug.Log("강화 레벨이 0 이하입니다.");
            onFail?.Invoke();
            return false;
        }

        var currentData = DiceTableDatabase.Get(type, currentLevel);

        if (currentData == null)
        {
            Debug.LogError($"다운그레이드 데이터 누락: {type}, 레벨 {currentLevel}");
            onFail?.Invoke();
            return false;
        }

        // 마력 수정 반환
        int returned = currentData.manaCount;
        progress.AddEnchantCore(returned);

        // 레벨 감소
        progress.UpgradeLevelDown(type);

        // UI 갱신
        UIManager.Instance.UpdateEnchantCore(progress.GetEnchantCore());
        UpgradeDatabase.UpdateUpgradeData(type);
        UIManager.Instance.UpgradeHandUI.Refresh();

        onSuccess?.Invoke();
        return true;
    }

    /// <summary>
    /// 현재 강화 수치 조회
    /// </summary>
    public static int GetCurrentLevel(HandType type)
    {
        return PlayerProgressManager.Instance.GetUpgradeLevel(type);
    }

    /// <summary>
    /// 최대 강화 레벨 조회
    /// </summary>
    public static int GetMaxLevel(HandType type)
    {
        return DiceTableDatabase.GetMaxLevel(type);
    }
}
