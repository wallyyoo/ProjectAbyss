using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class BaseStatJsonConverter : MonoBehaviour
{
    public TextAsset csvFile; // 에디터에서 CSV 드래그
    public string outputFileName = "StatTableData.json";

    void Start()
    {
        List<BaseStatTableData> dataList = ParseCSV(csvFile.text);

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
        File.WriteAllText(Application.dataPath + "/StatTableData.json", json);
    }

    List<BaseStatTableData> ParseCSV(string csvText)
    {
        var list = new List<BaseStatTableData>();
        string[] lines = csvText.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] cols = line.Split(','); // csv 파일 구분
            if (cols.Length < 5) continue;

            list.Add(new BaseStatTableData
            {
                playerStatType = ParseStatType(cols[0]),
                level = int.Parse(cols[1]),
                stats = int.Parse(cols[2]),
                add_Stats = int.Parse(cols[3]),
                manaCount = int.Parse(cols[4]),
            });
        }

        return list;
    }

    PlayerStatType ParseStatType(string raw)
    {
        return raw.Trim() switch
        {
            "hp" => PlayerStatType.MaxHP,
            _ => throw new System.Exception($"이상한게 들어 있음 {raw}")
        };
    }
}
