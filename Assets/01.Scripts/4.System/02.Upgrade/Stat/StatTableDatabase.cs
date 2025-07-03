using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class StatTableDatabase
{
    private static Dictionary<(PlayerStatType, int), BaseStatTableData> table;

    public static void Load()
    {      
        if (table != null) return;

        TextAsset jsonFile = Resources.Load<TextAsset>("StatTableData"); // Resources 폴더 안
        var list = JsonConvert.DeserializeObject<List<BaseStatTableData>>(jsonFile.text);

        table = new();

        foreach (var data in list)
            table[(data.playerStatType, data.level)] = data;
    }

    public static BaseStatTableData Get(PlayerStatType type, int level)
    {
        if (table == null) Load();
        return table.TryGetValue((type, level), out var data) ? data : null;
    }

    public static int GetMaxLevel(PlayerStatType type)
    {
        if (table == null) Load();
        return table.Keys.Where(k => k.Item1 == type).Max(k => k.Item2);
    }

    public static BaseStatTableData GetUpgradeData(PlayerStatType type, int level)
    {
        if (table == null) Load();

        return Get(type, level);
    }
}
