using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class NodeTypeAssigner : INodeTypeAssigner
{
    private Dictionary<NodeType, float> _weights;

    public NodeTypeAssigner(float battleWeight, float shopWeight, float rewardWeight, float eventWeight)
    {
        _weights = new Dictionary<NodeType, float>
        {
            { NodeType.Battle, battleWeight },
            { NodeType.Shop, shopWeight },
            { NodeType.Reward, rewardWeight },
            { NodeType.Event, eventWeight }
        };
    }
    public NodeType AssignType(int currentRoomCount, int maxRoomCount)
    {
        float totalWeight = _weights.Values.Sum();
        float randomValue = Random.Range(0.0f, totalWeight);
        float cumulative = 0.0f;
        
        foreach (KeyValuePair<NodeType, float> pair in _weights)
        {
            cumulative += pair.Value;
            if (randomValue <= cumulative)
            {
                return pair.Key;
            }
        }
        return NodeType.Battle;
    }
}
