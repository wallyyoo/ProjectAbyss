using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 노드의 종류(전투, 상점, 보상 등)
/// </summary>
public enum NodeType
{
    Battle,
    Shop,
    Reward,
    Event,
    Start,
    Boss
}
public class NodeModel
{
    public int Id { get; }
    public NodeType Type { get; set; }
    public Vector2Int GridPos { get; }
    public List<int> ConnectedNodeIds { get; }

    public NodeModel(int id, NodeType type, Vector2Int gridPos)
    {
        Id = id;
        Type = type;
        GridPos = gridPos;
        ConnectedNodeIds = new List<int>();
    }
}
