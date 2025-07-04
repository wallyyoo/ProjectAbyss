using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNodeClickHandler : INodeClickActionHandler
{
    public NodeType NodeType => NodeType.Battle;

    public void HandleClick(NodeModel nodeModel)
    {
        //StartBattle
    }
}
