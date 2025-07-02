using System.Collections.Generic;
using UnityEngine;
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
