using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodeRevealStrategy
{
    /// <summary>
    /// 해당 노드를 사전에 공개할지 여부를 결정합니다.
    /// </summary>
    /// <param name="nodeModel"></param>
    bool ShouldReveal(NodeModel nodeModel);

    /// <summary>
    /// 아이콘은 이미 다 보인 상태, 하이라이트 할 ID 리스트
    /// </summary>
    /// <returns></returns>
    IEnumerable<int> GetHighlightedNodeIds();
    
    IEnumerable<(int fromNodeId, int toNodeId)> GetHighlightedEdges();
}
