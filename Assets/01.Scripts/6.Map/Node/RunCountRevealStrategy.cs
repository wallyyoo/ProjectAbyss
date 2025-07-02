using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunCountRevealStrategy : INodeRevealStrategy
{
    private readonly HashSet<int> _revealNodeIds = new HashSet<int>();
    private readonly List<int> _highlightedNodeIds = new List<int>();
    private readonly List<(int,int)> _highlightedEdges = new List<(int,int)>();
    private readonly NodeType _targetType;

    public RunCountRevealStrategy(
        MapModel mapModel,
        int runCount,
        int currentNodeId,
        HashSet<int> visitedNodeIds,
        NodeType targetType
        )
        
    {
        _targetType = targetType;
        foreach (int visited in visitedNodeIds)
        {
            _revealNodeIds.Add(visited);
        }
        
        NodeModel currentNode = mapModel
            .Nodes
            .First(n => n.Id == currentNodeId);
        List<NodeModel> neighbors = mapModel
                                    .Nodes
                                    .Where(n => currentNode.ConnectedNodeIds.Contains(n.Id)).ToList();
       
       
        
        if (runCount >= 2) // 2회차 : 회복 노드 ? 표시 제외
        {
            foreach (NodeModel neighbor in neighbors)
            {
                
                if (neighbor.Type == NodeType.Rest) 
                    _revealNodeIds.Add(neighbor.Id);
            }
            
        }
        
        if (runCount >= 3) // 3회차 : 상점 노드 ? 표시 제외
        {
            foreach (NodeModel neighbor in neighbors)
            {
                
                if (neighbor.Type == NodeType.Shop) 
                    _revealNodeIds.Add(neighbor.Id);
            }
        }
        
        if (runCount >= 4)
        {
            List<NodeModel> battleNeighbors = neighbors
                                              .Where(n => n.Type == NodeType.Battle)
                                              .ToList();
            int toRevealCount;
            if (runCount == 4)
            {
                toRevealCount = Mathf.CeilToInt(battleNeighbors.Count()*0.3f);
            }
            else if (runCount == 5)
            {
                toRevealCount = Mathf.FloorToInt(battleNeighbors.Count()*0.6f);
            }
            else
            {
                toRevealCount = battleNeighbors.Count();
            }
            System.Random _random = new System.Random();
            List<NodeModel> shuffled = battleNeighbors
                .OrderBy(_ => _random.Next())
                .ToList();
            for (int i = 0; i < toRevealCount && i < shuffled.Count; i++)
            {
                _revealNodeIds.Add(shuffled[i].Id);
            }
        }
        
        if (runCount >= 7)
        {
            
            NodeModel targetNode = mapModel.Nodes
                                          .FirstOrDefault(n => n.Type == _targetType);
            if(targetNode != null)
                _revealNodeIds.Add(targetNode.Id);
        }

        if (runCount >= 8)
        {
            ComputeHighlightPath(mapModel);
        }
        
    }



    private void ComputeHighlightPath(MapModel mapModel)
    {
        NodeModel startNode = mapModel.Nodes.FirstOrDefault(n => n.Type == NodeType.Start);
        if (startNode == null)
        {
            Debug.LogError("ComputHighlighPath 실패: Start노드가 없음");
            return;
        }
        
        NodeModel targetNode = mapModel.Nodes.FirstOrDefault(n=>n.Type == _targetType);
        if (targetNode == null)
        {
            Debug.LogError($"ComputHighlighPath 실패: {_targetType}노드가 없음");
            return;
        }
        int startId = startNode.Id;
        int targetId = targetNode.Id;
        //BFS 최단경로
        Queue<int> queue = new Queue<int>();
        Dictionary<int, int> parent = new Dictionary<int, int>();
        HashSet<int> visited = new HashSet<int> { startId };

        queue.Enqueue(startId);

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            if (current == targetId) break;

            NodeModel node = mapModel.Nodes.First(n => n.Id == current);
            foreach (int neighborId in node.ConnectedNodeIds)
            {
                if (visited.Add(neighborId))
                {
                    parent[neighborId] = current;
                    queue.Enqueue(neighborId);
                }
            }
        }

        List<int> path = new List<int>();
        if (parent.ContainsKey(targetId) || startId == targetId)
        {
            int crawl = targetId;
            while (true)
            {
                path.Add(crawl);
                if (crawl == startId) break;
                crawl = parent[crawl];
            }
            path.Reverse();
        }

        _highlightedNodeIds.AddRange(path);
        for (int i = 0; i < path.Count - 1; i++)
        {
            _highlightedEdges.Add((path[i], path[i+1]));
        }
    }

    public bool ShouldReveal(NodeModel nodeModel) => _revealNodeIds.Contains(nodeModel.Id);
    
    public IEnumerable<int> GetHighlightedNodeIds() => _highlightedNodeIds;

    public IEnumerable<(int, int)> GetHighlightedEdges() => _highlightedEdges;
}
