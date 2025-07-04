using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodeClickActionHandler
{
    NodeType NodeType { get; }

    void HandleClick(NodeModel nodeModel);
}
