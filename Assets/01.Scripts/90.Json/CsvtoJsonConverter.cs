using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JsonConverter : MonoBehaviour
{
    public TextAsset csvFile; // 에디터에서 CSV 드래그
    public string outputFileName = "DiceTableData.json";

    void Start()
    {
        List<DiceTableData> dataList = ParseCSV(csvFile.text);

        // JSON 직렬화 옵션 (enum은 문자열로)
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented, // 들여쓰기
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };

        string json = JsonConvert.SerializeObject(dataList, settings);

        string path = Path.Combine(Application.dataPath, "Resources", outputFileName);
        File.WriteAllText(path, json);
        Debug.Log($"JSON 파일 저장 완료: {path}");

        // 필요하면 JSON 파일로 저장
        File.WriteAllText(Application.dataPath + "/DiceTableData.json", json);
    }

    List<DiceTableData> ParseCSV(string csvText)
    {
        var list = new List<DiceTableData>();
        string[] lines = csvText.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] cols = line.Split(','); // csv 파일 구분
            if (cols.Length < 8) continue;

            list.Add(new DiceTableData
            {
                handType = ParseHandType(cols[0]),
                level = int.Parse(cols[1]),
                score = int.Parse(cols[2]),
                multiplier = int.Parse(cols[3]),
                total = int.Parse(cols[4]),
                manaCount = int.Parse(cols[5]),
                add_score = int.Parse(cols[6]),
                add_multiplier = int.Parse(cols[7])
            });
        }

        return list;
    }

    HandType ParseHandType(string raw)
    {
        return raw.Trim() switch
        {
            "Mono Roll" => HandType.MonoRoll,
            "Four Dice" => HandType.FourDice,
            "Large_Straight" => HandType.LargeStraight,
            "Small_Straight" => HandType.SmallStraight,
            "Full House" => HandType.FullHouse,
            "Triple" => HandType.Triple,
            "Two Pair" => HandType.TwoPair,
            "One Pair" => HandType.OnePair,
            "High Dice" => HandType.HighDice,
            _ => throw new System.Exception($"이상한게 들어 있음 {raw}")
        };
    }
}
