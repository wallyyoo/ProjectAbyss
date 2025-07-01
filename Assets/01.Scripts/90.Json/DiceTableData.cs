using UnityEngine;
using Newtonsoft.Json;
using System;

public class DiceTableData
{
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public HandType handType;

    public int level;
    public int score;
    public int multiplier;
    public int total;
    public int manaCount;
    public int add_score;
    public int add_multiplier;
}
