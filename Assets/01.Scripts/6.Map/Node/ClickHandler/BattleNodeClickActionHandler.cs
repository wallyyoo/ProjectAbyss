using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNodeClickActionHandler : INodeClickActionHandler
{
    public NodeType NodeType => NodeType.Battle;

    public void HandleClick(NodeModel nodeModel)
    {
        Debug.Log("BattleNodeClicked & Play Battle");
        TurnManager.Instance.SetTurnPhase(TurnPhase.Ready);
    }
}
