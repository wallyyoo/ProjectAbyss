using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class RunCountRevealStrategy : INodeRevealStrategy
{
    private readonly int _runCount;
    private readonly HashSet<int> _revealNodeIds;

    public RunCountRevealStrategy(MapModel mapModel, int runCount)
    {
        _runCount = runCount;
        _revealNodeIds = ComputeRevealedNodes(mapModel);
    }

    private HashSet<int> ComputeRevealedNodes(MapModel mapModel)
    {
        var result = new HashSet<int>();

        if (_runCount <= 1) //1회차 : 모든노드 ? 표시
        {
            return result;
        }
        
        if (_runCount == 2) // 2회차 : 회복 노드 ? 표시 제외
        {
            foreach(var node in mapModel.Nodes)
                if (node.Type == NodeType.Reward) 
                    result.Add(node.Id);
            return result;
        }
        
        if (_runCount == 3) // 3회차 : 상점 노드 ? 표시 제외
        {
            foreach (var node in mapModel.Nodes)
                if (node.Type == NodeType.Reward || node.Type == NodeType.Shop)
                    result.Add(node.Id);
            return result;
        }
        
        if (_runCount == 4 || _runCount == 5)
        {
            float ratio = (_runCount == 4) ? 0.3f : 0.6f;
            var battleNodes = mapModel.Nodes
                                      .Where(n => n.Type == NodeType.Battle)
                                      .OrderBy(n => n.Id)
                                      .ToList();
            int countToReveal = (int)MathF.Ceiling(battleNodes.Count * ratio);
            for(int i = 0; i < countToReveal; i++)
                result.Add(battleNodes[i].Id);
            
            foreach (var node in mapModel.Nodes)
                if (node.Type == NodeType.Reward || node.Type == NodeType.Shop)
                    result.Add(node.Id);
            
            return result;
        }
        
        if (_runCount == 6)
        {
            foreach (var node in mapModel.Nodes)
                if (node.Type == NodeType.Reward
                    || node.Type == NodeType.Shop
                    || node.Type == NodeType.Battle)
                    result.Add(node.Id);
            return result;
        }
        if (_runCount == 7)
        {
            foreach (var node in mapModel.Nodes)
                if (node.Type == NodeType.Reward
                    || node.Type == NodeType.Shop
                    || node.Type == NodeType.Battle)
                    result.Add(node.Id);
            var bossNodes = mapModel.Nodes.First(n => n.Type == NodeType.Battle);
            result.Add(bossNodes.Id);
            return result;
        }

        foreach (var node in mapModel.Nodes)
            result.Add(node.Id);
        return result;
    }

    public bool ShouldReveal(NodeModel nodeModel)
    {
        return _revealNodeIds.Contains(nodeModel.Id);
    }
}
