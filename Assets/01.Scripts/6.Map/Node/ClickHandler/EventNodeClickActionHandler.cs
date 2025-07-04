using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNodeClickActionHandler : INodeClickActionHandler
{
    public NodeType NodeType => NodeType.Event;
    public void HandleClick(NodeModel nodeModel)
    {
        //EventManager.StartRandomEvent;
    }
}
