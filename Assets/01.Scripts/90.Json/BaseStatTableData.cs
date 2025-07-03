using Newtonsoft.Json;

public class BaseStatTableData
{
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public PlayerStatType playerStatType;

    public int level;
    public int stats;
    public int add_Stats;
    public int manaCount;
}
