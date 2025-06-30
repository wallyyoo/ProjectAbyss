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
}
