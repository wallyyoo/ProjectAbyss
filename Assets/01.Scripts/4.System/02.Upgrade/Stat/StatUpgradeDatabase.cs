using System;
using System.Collections.Generic;
using UnityEngine;

public static class StatUpgradeDatabase
{
    public static Dictionary<PlayerStatType, BaseStatTableData> table = new();

    public static void Init()
    {
        table.Clear();

        StatTableDatabase.Load();

        foreach (PlayerStatType type in Enum.GetValues(typeof(PlayerStatType)))
        {
            if (type == PlayerStatType.None) // None 타입 건너뛰기
                continue;

            int level = PlayerProgressManager.Instance.GetStatUpgradeLevel(type);
            var data = StatTableDatabase.GetUpgradeData(type, level);

            if (data != null)
                table[type] = data;
            else
                Debug.LogWarning($"StatUpgradeDatabase Init: {type} 레벨 {level} 데이터 없음");
        }
    }

    public static void UpdateUpgradeData(PlayerStatType type)
    {
        if (type == PlayerStatType.None) // None 타입 건너뛰기
            return;

        int level = PlayerProgressManager.Instance.GetStatUpgradeLevel(type);
        var data = StatTableDatabase.GetUpgradeData(type, level);

        if (data != null)
        {
            table[type] = data;
        }
        else
        {
            Debug.LogWarning($"StatUpgradeDatabase Update: {type} 레벨 {level} 데이터 없음");
        }
    }
}