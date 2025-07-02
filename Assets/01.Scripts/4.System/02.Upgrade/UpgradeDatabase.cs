
using System;
using System.Collections.Generic;

public static class UpgradeDatabase
{
    public static Dictionary<HandType, HandTypeUpgradeData> table = new();

    public static void Init()
    {
        table.Clear();

        DiceTableDatabase.Load();

        foreach (HandType type in Enum.GetValues(typeof(HandType)))
        {
            int level = PlayerProgressManager.Instance.GetUpgradeLevel(type);
            var data = DiceTableDatabase.GetUpgradeData(type, level);

            if (data != null) table[type] = data;
        }
    }

    public static void UpdateUpgradeData(HandType type)
    {
        int level = PlayerProgressManager.Instance.GetUpgradeLevel(type);
        var data = DiceTableDatabase.GetUpgradeData(type, level);

        if (data != null)
        {
            table[type] = data;
        }
        else
        {
            UnityEngine.Debug.LogWarning($"UpgradeDatabase: {type} 레벨 {level} 데이터 없음");
        }
    }
}
