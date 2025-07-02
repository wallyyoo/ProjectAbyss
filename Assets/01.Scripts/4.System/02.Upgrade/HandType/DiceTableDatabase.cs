using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class DiceTableDatabase
{
    private static Dictionary<(HandType, int), DiceTableData> table;

    public static void Load()
    {
        if (table != null) return;

        // Json파일 불러오기
        TextAsset jsonFile = Resources.Load<TextAsset>("DiceTableData");

        // 역직렬화
        var list = JsonConvert.DeserializeObject<List<DiceTableData>>(jsonFile.text);

        // 초기화
        table = new();

        // 리스트에 있는 각 데이터 항목을 추가
        foreach (var data in list)
            table[(data.handType, data.level)] = data;
        // 족보, 레벨로 접근
    }

    /// <summary>
    /// 데이터 반환용
    /// </summary>
    /// <param name="type">족보</param>
    /// <param name="level">강화 레벨</param>
    /// <returns></returns>
    public static DiceTableData Get(HandType type, int level)
    {
        if (table == null) Load();
        return table.TryGetValue((type, level), out var data) ? data : null;
    }

    /// <summary>
    /// 최대 레벨 반환
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static int GetMaxLevel(HandType type)
    {
        if (table == null) Load();

        // 딕셔너리에서 족보의 최대 강화 레벨 값을 반환
        return table.Keys.Where(k => k.Item1 == type).Max(k => k.Item2);
    }

    public static HandTypeUpgradeData GetUpgradeData(HandType type, int level)
    {
        if (table == null) Load();

        if (table.TryGetValue((type, level), out var data))
        {
            return new HandTypeUpgradeData
            {
                level = data.level,
                add_score = data.add_score,
                add_multiplier = data.add_multiplier,
                manaCount = data.manaCount
            };
        }

        return null;
    }
}