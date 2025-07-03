using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PlayerProgressManager : Singleton<PlayerProgressManager>
{
    private int gold;
    private int enchantCore;

    private Dictionary<HandType, int> handTypeUpgradeLevels = new();
    private Dictionary<PlayerStatType, int> statUpgradeLevels = new();

    public PlayerProgress Progress { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Progress = FindObjectOfType<PlayerProgress>();
        if (Progress == null)
        {
            GameObject go = new GameObject("PlayerProgress");
            Progress = go.AddComponent<PlayerProgress>();
            go.transform.SetParent(transform);
        }
        Progress.Init();
    }

    public void SyncUpgradeLevels()
    {
        // 스탯
        foreach (PlayerStatType stat in Enum.GetValues(typeof(PlayerStatType)))
        {
            int lvl = GetStatUpgradeLevel(stat);
            Progress.SetStatUpgradeLevel(stat, lvl);
        }

        // 족보
        foreach (HandType hand in Enum.GetValues(typeof(HandType)))
        {
            int lvl = GetHandTypeUpgradeLevel(hand);
            Progress.SetHandUpgradeLevel(hand, lvl);
        }
    }

    // =========== 재화 관련 ===========
    [Button("골드 추가")]
    public void AddGold(int amount = 1000)
    {
        if (amount < 0)
        {
            Debug.Log("골드 음수 추가 방지!");
            return;
        }

        gold += amount;
        UIManager.Instance.UpdateGold(gold);
    }

    [Button("골드 소모")]
    public bool SpendGold(int amount = 1000)
    {
        if (gold < amount) return false;

        gold -= amount;
        UIManager.Instance.UpdateGold(gold);

        return true;
    }

    public int GetGold() => gold;

    [Button("마력 수정 추가")]
    public void AddEnchantCore(int amount = 1000)
    {
        if (amount < 0)
        {
            Debug.Log("마력 수정 음수 추가 방지!");
            return;
        }

        enchantCore += amount;
        UIManager.Instance.UpdateEnchantCore(enchantCore);
    }

    [Button("마력 수정 소모")]
    public bool SpendEnchantCore(int amount = 1000)
    {
        if (enchantCore < amount) return false;

        enchantCore -= amount;
        UIManager.Instance.UpdateEnchantCore(enchantCore);

        return true;
    }

    public int GetEnchantCore() => enchantCore;

    // =========== 강화 관련 (족보) ===========
    public int GetHandTypeUpgradeLevel(HandType type)
    {
        return handTypeUpgradeLevels.GetValueOrDefault(type, 0);
    }

    public void UpgradeHandTypeLevelUp(HandType type)
    {
        int current = GetHandTypeUpgradeLevel(type);
        int max = DiceTableDatabase.GetMaxLevel(type);
        handTypeUpgradeLevels[type] = Mathf.Clamp(current + 1, 0, max);

        SyncUpgradeLevels();
    }

    public void UpgradeHandTypeLevelDown(HandType type)
    {
        int current = GetHandTypeUpgradeLevel(type);
        handTypeUpgradeLevels[type] = Mathf.Max(current - 1, 0);

        SyncUpgradeLevels();
    }

    public void SetUpgradeHandTypeLevel(HandType type, int level)
    {
        int max = DiceTableDatabase.GetMaxLevel(type);
        handTypeUpgradeLevels[type] = Mathf.Clamp(level, 0, max);
    }

    // =========== 강화 관련 (스탯) ===========
    public int GetStatUpgradeLevel(PlayerStatType type)
    {
        return statUpgradeLevels.GetValueOrDefault(type, 0);
    }

    public void UpgradeStatLevelUp(PlayerStatType type)
    {
        int current = GetStatUpgradeLevel(type);
        int max = StatTableDatabase.GetMaxLevel(type);
        statUpgradeLevels[type] = Mathf.Clamp(current + 1, 0, max);

        SyncUpgradeLevels();
    }

    public void UpgradeStatLevelDown(PlayerStatType type)
    {
        int current = GetStatUpgradeLevel(type);
        statUpgradeLevels[type] = Mathf.Max(current - 1, 0);

        SyncUpgradeLevels();
    }

    public void SetUpgradeStatLevel(PlayerStatType type, int level)
    {
        int max = StatTableDatabase.GetMaxLevel(type);
        statUpgradeLevels[type] = Mathf.Clamp(level, 0, max);
    }
}
