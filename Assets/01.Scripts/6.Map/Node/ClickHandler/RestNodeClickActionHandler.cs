using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestNodeClickActionHandler : INodeClickActionHandler
{
    public NodeType NodeType => NodeType.Rest;
    public void HandleClick(NodeModel nodeModel)
    {
        //StatusManager.Heal;
    }
}
