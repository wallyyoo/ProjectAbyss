using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private int baseHP;

    private int hpUpgrade;
    private int damageUpgrade;

    private Dictionary<HandType, int> handTypeUpgradeBonus = new(); // 핸드

    public int MaxHP => baseHP + hpUpgrade; // 고정 수치 상승할 것인지 

    public void Init(PlayerData data)
    {
        baseHP = data.MaxHP;
    }

    public void SetHandTypeUpgrade(HandType handType, int bonus)
    {
        handTypeUpgradeBonus[handType] = bonus;
    }

    public int GetHandTypeUpgrade(HandType handType)
    {
        if (handTypeUpgradeBonus.TryGetValue(handType, out int bonus))
            return bonus;
        return 0;
    }
}
