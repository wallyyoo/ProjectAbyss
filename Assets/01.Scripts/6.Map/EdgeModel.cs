using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 두 노드를 연결하는 간선 모델
/// </summary>
public class EdgeModel
{
    public int FromNodeId { get; }
    public int ToNodeId { get; }

    public EdgeModel(int fromNodeId, int toNodeId)
    {
        FromNodeId = fromNodeId;
        ToNodeId = toNodeId;
    }
}
