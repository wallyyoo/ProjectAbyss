using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNodeClickHandler : INodeClickActionHandler
{
    public NodeType NodeType => NodeType.Shop;

    public void HandleClick(NodeModel nodeModel)
    {
        //OpenShop
    }
}
