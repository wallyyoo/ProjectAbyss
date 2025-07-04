using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNodeClickActionHandler : INodeClickActionHandler
{
    public NodeType NodeType => NodeType.Move;
    public void HandleClick(NodeModel nodeModel)
    {
        throw new System.NotImplementedException();
    }
}
