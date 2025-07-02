using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 맵 정보(Node+Edge)와 진행상황(Current + Visited)을 함께 저장하기 위한 데이터
/// </summary>
[Serializable]
public class SaveData
{
    public List<NodeData> Nodes;
    public List<EdgeData> Edges;
    public int CurrentNodeId;
    public List<int> VisitedNodeIds;
    public int RunCount;

    public StageProgress Progress;
}

[Serializable]
public class NodeData
{
    public int Id;
    public NodeType Type;
    public int X;
    public int Y;
    public List<int> ConndectedNodeIds;
}

[Serializable]
public class EdgeData
{
    public int FromNodeId;
    public int ToNodeId;
}