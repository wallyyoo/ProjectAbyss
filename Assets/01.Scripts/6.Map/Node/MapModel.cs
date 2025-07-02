using System.Collections.Generic;

/// <summary>
/// 맵 전체 그래프를 관리하는 모델
/// </summary>
public class MapModel
{
    public List<NodeModel> Nodes { get; }
    public List<EdgeModel> Edges { get; }

    public MapModel()
    {
        Nodes = new List<NodeModel>();
        Edges = new List<EdgeModel>();
    }
}
