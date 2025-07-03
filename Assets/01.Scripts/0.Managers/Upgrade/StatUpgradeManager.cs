using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatUpgradeManager
{
    /// <summary>
    /// 스탯 강화 시도
    /// </summary>
    /// <param name="type">강화할 스탯 타입</param>
    /// <param name="onSuccess">성공 시 호출 콜백</param>
    /// <param name="onFail">실패 시 호출 콜백</param>
    /// <returns>강화 성공 여부</returns>
    public static bool TryUpgrade(PlayerStatType type, Action onSuccess = null, Action onFail = null)
    {
        var progress = PlayerProgressManager.Instance;

        int currentLevel = progress.GetStatUpgradeLevel(type);
        int maxLevel = StatTableDatabase.GetMaxLevel(type);

        if (currentLevel >= maxLevel)
        {
            Debug.Log("이미 최대 강화 레벨입니다.");
            onFail?.Invoke();
            return false;
        }

        var nextData = StatTableDatabase.GetUpgradeData(type, currentLevel + 1);
        if (nextData == null)
        {
            Debug.LogError($"강화 데이터 누락: {type}, 레벨 {currentLevel + 1}");
            onFail?.Invoke();
            return false;
        }

        if (!progress.SpendEnchantCore(nextData.manaCount))
        {
            Debug.Log("마력 수정 부족");
            onFail?.Invoke();
            return false;
        }

        progress.UpgradeStatLevelUp(type);

        UIManager.Instance.UpdateEnchantCore(progress.GetEnchantCore());
        StatUpgradeDatabase.UpdateUpgradeData(type);
        UIManager.Instance.StatUIManager.Refresh(); // UI 갱신

        onSuccess?.Invoke();
        return true;
    }

    /// <summary>
    /// 스탯 강화 취소(다운그레이드)
    /// </summary>
    public static bool TryDowngrade(PlayerStatType type, Action onSuccess = null, Action onFail = null)
    {
        var progress = PlayerProgressManager.Instance;

        int currentLevel = progress.GetStatUpgradeLevel(type);

        if (currentLevel <= 0)
        {
            Debug.Log("강화 레벨이 0 이하입니다.");
            onFail?.Invoke();
            return false;
        }

        var currentData = StatTableDatabase.GetUpgradeData(type, currentLevel);

        if (currentData == null)
        {
            Debug.LogError($"다운그레이드 데이터 누락: {type}, 레벨 {currentLevel}");
            onFail?.Invoke();
            return false;
        }

        progress.AddEnchantCore(currentData.manaCount);
        progress.UpgradeStatLevelDown(type);

        UIManager.Instance.UpdateEnchantCore(progress.GetEnchantCore());
        StatUpgradeDatabase.UpdateUpgradeData(type);
        UIManager.Instance.StatUIManager.Refresh();

        onSuccess?.Invoke();
        return true;
    }
}

