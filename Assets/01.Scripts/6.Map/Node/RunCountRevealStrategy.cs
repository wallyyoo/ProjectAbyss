using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class RunCountRevealStrategy : INodeRevealStrategy
{
    private readonly int _runCount;
    private readonly HashSet<int> _revealNodeIds;
    private readonly List<int> _highlightNodeIds = new();
    private readonly List<(int,int)> _highlighEdges = new();

    public RunCountRevealStrategy(MapModel mapModel, int runCount)
    {
        _runCount = runCount;
        _revealNodeIds = ComputeRevealedNodes(mapModel);
        if (_runCount >= 8)
            ComputeHighlightPath(mapModel);
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
            var bossNodes = mapModel.Nodes.FirstOrDefault(n => n.Type == NodeType.Boss);
            result.Add(bossNodes.Id);
            return result;
        }

        if (_runCount >= 8)
        {
            foreach (var node in mapModel.Nodes)
                result.Add(node.Id);
        }
        return result;
    }

    private void ComputeHighlightPath(MapModel mapModel)
    {
        int startId = mapModel.Nodes.First(n => n.Type == NodeType.Start).Id;
        int bossId = mapModel.Nodes.First(n => n.Type == NodeType.Boss).Id;
        
        //BFS 최단경로
        var parent = new Dictionary<int, int>();
        var queue = new Queue<int>();
        var visited = new HashSet<int> { startId };
        queue.Enqueue(startId);
        
        while (queue.Count > 0)
        {
            int cur = queue.Dequeue();
            if (cur == bossId) break;
            var node = mapModel.Nodes.Find(n => n.Id == cur);
            foreach (int nb in node.ConnectedNodeIds)
            {
                if (visited.Add(nb))
                {
                    parent[nb] = cur;
                    queue.Enqueue(nb);
                }
            }
        }

        var path = new List<int>();
        if (startId == bossId || parent.ContainsKey(bossId))
        {
            int crawl = bossId;
            path.Add(crawl);
            while (crawl != startId)
            {
                crawl = parent[crawl];
                path.Add(crawl);
            }
            
            path.Reverse();
        }

        _highlightNodeIds.AddRange(path);
        for(int i = 0; i< path.Count - 1; i++)
             _highlighEdges.Add((path[i], path[i + 1]));
    }
    public bool ShouldReveal(NodeModel nodeModel) => _revealNodeIds.Contains(nodeModel.Id);
    
    public IEnumerable<int> GetHighlightedNodeIds() => _highlightNodeIds;

    public IEnumerable<(int, int)> GetHighlightedEdges() => _highlighEdges;
}
