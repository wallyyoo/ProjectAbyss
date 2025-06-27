using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressManager : Singleton<PlayerProgressManager>
{
    [SerializeField] private PlayerData data;

    public int CurrentHP { get; private set; }
    public int CurrentGold { get; private set; }

    [Header("업그레이드 관련")]
    private int hpUpgradeAmount;
    private int hpUpgradeLevel;

    private Dictionary<HandType, int> handTypeUpgradeBonus = new();

    public int MaxHP => data.MaxHP + (hpUpgradeAmount * hpUpgradeLevel);

    /// <summary>
    /// 초기 데이터(SO)를 기반으로 초기화 및 초기값 세팅, 새게임 시작 같은 부분에서도 사용 가능
    /// </summary>
    public void Init()
    {
        hpUpgradeAmount = 10;
        hpUpgradeLevel = 0;
        CurrentGold = 0;
        CurrentHP = MaxHP;
        handTypeUpgradeBonus.Clear();
    }
}
