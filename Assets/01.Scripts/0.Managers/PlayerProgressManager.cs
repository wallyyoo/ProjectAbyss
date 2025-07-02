using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PlayerProgressManager : Singleton<PlayerProgressManager>
{
    private int gold;
    private int enchantCore;

    private Dictionary<HandType, int> upgradeLevels = new();

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

    // =========== 강화 관련 ===========
    public int GetUpgradeLevel(HandType type)
    {
        return upgradeLevels.GetValueOrDefault(type, 0);
    }

    public void UpgradeLevelUp(HandType type)
    {
        int current = GetUpgradeLevel(type);
        int max = DiceTableDatabase.GetMaxLevel(type);
        upgradeLevels[type] = Mathf.Clamp(current + 1, 0, max);
    }

    public void UpgradeLevelDown(HandType type)
    {
        int current = GetUpgradeLevel(type);
        upgradeLevels[type] = Mathf.Max(current - 1, 0);
    }

    public void SetUpgradeLevel(HandType type, int level)
    {
        int max = DiceTableDatabase.GetMaxLevel(type);
        upgradeLevels[type] = Mathf.Clamp(level, 0, max);
    }
}
