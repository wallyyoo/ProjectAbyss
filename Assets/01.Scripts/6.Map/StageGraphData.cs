using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGraphData
{
    public List<StageNodeData> Nodes { get; }

    public StageGraphData()
    {
        Nodes = new List<StageNodeData>();
    }

    public void AddNode(StageNodeData node)
    {
        if (node != null)
        {
            Nodes.Add(node);
        }
    }
}
