using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomMapGenerator : IMapGenerator
{
    private readonly INodeTypeAssigner _nodeTypeAssigner;
    private readonly IBossRoomSelector _bossRoomSelector;
    private readonly List<Vector2Int> _gridPositions;
    
    System.Random _random = new System.Random();
    public CustomMapGenerator(List<Vector2Int> gridPositions, INodeTypeAssigner nodeTypeAssigner,
        IBossRoomSelector bossRoomSelector)
    {
        _gridPositions = gridPositions;
        _nodeTypeAssigner = nodeTypeAssigner;
        _bossRoomSelector = bossRoomSelector;
    }

    public MapModel Generate(int unsuedDepth, int unused1, int unused2)
    {
        MapModel mapModel = new MapModel();
        Dictionary<Vector2Int, NodeModel> positionNodeMap = new Dictionary<Vector2Int, NodeModel>();
        int nextId = 0;
        
        // 1) 노드 생성
        foreach (Vector2Int gridPosition in _gridPositions)
        {
            NodeType assignedType = _nodeTypeAssigner.AssignType(mapModel.Nodes.Count, _gridPositions.Count);
            NodeModel nodeModel = new NodeModel(nextId, assignedType, gridPosition);
            
            mapModel.Nodes.Add(nodeModel);
            positionNodeMap.Add(gridPosition, nodeModel);
            nextId++;
        }
        
        // 2) 외각 노드 필터링: 전체 외곽의 최소 50%는 남기고 나머지 랜덤 제거
        FilterOutNodes(mapModel, positionNodeMap, keepPercent: 50);
        
        // 3)남은 노드들로 인접 리스트 구성
        Dictionary<NodeModel, List<NodeModel>> adjacency =
            BuildAdjacency(positionNodeMap);
        
        // 4) 랜덤 스패닝 트리 생성 ->연결 보장
        List<EdgeModel> edges = BuildRandomSpanningTree(adjacency, mapModel);
        
        // 5) Dead-End(Leaf) 제한: Leaf 수가 3개 초과하면 서로 연결 추가
        AddEdgesToLimitDeadEnds(adjacency, mapModel, edges, maxLeaves:3);
        
        // 6) 상하 좌우 밀도 차 조정
        BalanceDensity(mapModel, maxDensityDiff:3);
        
        // 7) MapModel에 엣지 등록 및 ConnectedNodeIds 설정
        foreach (EdgeModel edge in edges)
        {
            mapModel.Edges.Add(edge);

            NodeModel a = mapModel.Nodes
                                  .Find(n => n.Id == edge.FromNodeId);
            NodeModel b = mapModel.Nodes
                                  .Find(n => n.Id == edge.ToNodeId);

            a.ConnectedNodeIds.Add(b.Id);
            b.ConnectedNodeIds.Add(a.Id);
        }
        
        
        // Vector2Int[] dirs =
        // {
        //     new Vector2Int(1, 0),
        //     new Vector2Int(-1, 0),
        //     new Vector2Int(0, 1),
        //     new Vector2Int(0, -1)
        //
        // };
        // foreach (KeyValuePair<Vector2Int, NodeModel> entry in positionNodeMap)
        // {
        //     Vector2Int currentPosition = entry.Key;
        //     NodeModel currentNode = entry.Value;
        //
        //     foreach (Vector2Int dir in dirs)
        //     {
        //         Vector2Int neighborPosition = currentPosition + dir;
        //         if (positionNodeMap.ContainsKey(neighborPosition))
        //         {
        //             NodeModel neighborNode = positionNodeMap[neighborPosition];
        //             if (currentNode.Id < neighborNode.Id)
        //             {
        //                 currentNode.ConnectedNodeIds.Add(neighborNode.Id);
        //                 neighborNode.ConnectedNodeIds.Add(currentNode.Id);
        //                 mapModel.Edges.Add(
        //                     new EdgeModel(currentNode.Id, neighborNode.Id));
        //                 
        //             }
        //         }
        //     }
        // }
        int startNodeId = mapModel.Nodes[0].Id;
        mapModel.Nodes.Find(n => n.Id == startNodeId).Type = NodeType.Start;
        int bossNodeId = _bossRoomSelector.SelectBoss(mapModel.Nodes, startNodeId);
        mapModel.Nodes.Find(n=>n.Id==bossNodeId).Type = NodeType.Boss;
        
        return mapModel;
    }

    private void FilterOutNodes(MapModel model, Dictionary<Vector2Int, NodeModel> posMap, int keepPercent)
    {
        int maxDist2 = posMap.Keys.Max(p=>p.x*p.x +p.y*p.y);

        List<NodeModel> outerNodes = model.Nodes
                                          .Where(n => n.GridPos.x * n.GridPos.x
                                              + n.GridPos.y * n.GridPos.y == maxDist2)
                                          .ToList();
        int minKeep = outerNodes.Count * keepPercent / 100;
        int removeCount = outerNodes.Count - minKeep;
        
        outerNodes
            .OrderBy(_ => _random.Next())
            .Take(removeCount)
            .ToList()
            .ForEach(n => RemoveNode(n,model,posMap));
    }
    
    //노드 제거 유틸
    private void RemoveNode(
        NodeModel node,
        MapModel model,
        Dictionary<Vector2Int, NodeModel> posMap)
    {
        model.Nodes.Remove(node);
        posMap.Remove(node.GridPos);
    }

    private Dictionary<NodeModel, List<NodeModel>> BuildAdjacency(
        Dictionary<Vector2Int, NodeModel> posMap)
    {
        Vector2Int[] dirs = new Vector2Int[]
        {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1)
        };
        var adjacency = new Dictionary<NodeModel, List<NodeModel>>();
        foreach (NodeModel node in posMap.Values)
        {
            adjacency[node] = new List<NodeModel>();
            foreach (Vector2Int dir in dirs)
            {
                Vector2Int np = node.GridPos + dir;
                if (posMap.ContainsKey(np))
                {
                    adjacency[node].Add(posMap[np]);
                }
            }
        }

        return adjacency;
    }
    
    //4) 랜덤 스패닝 트리(DFS기반)
    private List<EdgeModel> BuildRandomSpanningTree(
        Dictionary<NodeModel, List<NodeModel>> adj, MapModel model)
    {
        var edges = new List<EdgeModel>();
        var visited = new HashSet<NodeModel>();
        var stack = new Stack<NodeModel>();
        

        NodeModel start = model.Nodes.Find(n => n.Id == 0);
        visited.Add(start);
        stack.Push(start);

        while (stack.Count > 0)
        {
            NodeModel current = stack.Pop();
            
            List<NodeModel> neighbors = adj[current]
                .OrderBy(_ => _random.Next()).ToList();

            foreach (NodeModel neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    edges.Add(
                        new EdgeModel(current.Id, neighbor.Id));
                    stack.Push(neighbor);
                }
            }
        }

        return edges;
    }
    
    //5)Dead-End 제한을 위해 Leaf노드 간 추가 엣지
    private void AddEdgesToLimitDeadEnds(
        Dictionary<NodeModel, List<NodeModel>> adj,
        MapModel model,
        List<EdgeModel> edges,
        int maxLeaves)
    {
        Dictionary<int, int> degree = model.Nodes.ToDictionary(n => n.Id, n => 0);
        foreach (EdgeModel e in edges)
        {
            degree[e.FromNodeId]++;
            degree[e.ToNodeId]++;
        }

        List<int> leaves = degree.Where(kv => kv.Value == 1).Select(kv => kv.Key).ToList();

        while (leaves.Count > maxLeaves)
        {
            int leafId = leaves[_random.Next(leaves.Count)];
            NodeModel leaf = model.Nodes.Find(n => n.Id == leafId);

            var candidates = adj[leaf]
                .Where(n => degree[n.Id] > 0 
                    && !edges.Any(e =>
                    (e.FromNodeId == leafId && e.ToNodeId == n.Id)
                    || (e.FromNodeId == n.Id && e.ToNodeId == leafId)))
                .ToList();
            if (candidates.Count == 0) break;

            NodeModel target = candidates[_random.Next(candidates.Count)];
            
            edges.Add(new EdgeModel(leafId,target.Id));
            degree[leafId]++;
            degree[target.Id]++;
            
            leaves = degree
                .Where(kv => kv.Value ==1)
                .Select(kv=>kv.Key).ToList();
        }
    }

    private void BalanceDensity(MapModel model, int maxDensityDiff)
    {
        int LeftCount() => model.Nodes.Count(n => n.GridPos.x < 0);
        int RightCount() => model.Nodes.Count(n => n.GridPos.x > 0);
        int UpCount() => model.Nodes.Count(n => n.GridPos.y> 0);
        int DownCount() => model.Nodes.Count(n => n.GridPos.y<0);
        
        while(Math.Abs(LeftCount() - RightCount())>maxDensityDiff
            ||Math.Abs(UpCount()-DownCount())>maxDensityDiff)
        {
            bool fixX = Math.Abs(LeftCount()- RightCount())>maxDensityDiff;
            bool fisY = !fixX;

            List<NodeModel> candidates = model.Nodes
                .Where(n =>
                {
                    bool isLeaf = n.ConnectedNodeIds.Count == 1;
                    if (!isLeaf) return false;
                    if (fixX)
                    {
                        return (LeftCount() > RightCount()
                            ? n.GridPos.x < 0
                            : n.GridPos.x > 0);
                    }
                    else
                    {
                        return (UpCount() > DownCount()
                            ? n.GridPos.y > 0
                            : n.GridPos.y < 0);
                    }
                })
                .ToList();

            if (candidates.Count == 0) break;

            NodeModel removeNode = candidates[_random.Next(candidates.Count)];

            model.Nodes.Remove(removeNode);

            foreach (int neighborId in removeNode.ConnectedNodeIds)
            {
                NodeModel neighbor = model.Nodes.Find(n => n.Id == neighborId);
                neighbor.ConnectedNodeIds.Remove(removeNode.Id);
                model.Edges.RemoveAll(e =>
                    (e.FromNodeId == removeNode.Id && e.ToNodeId == neighborId)
                    || (e.FromNodeId == neighborId && e.ToNodeId == removeNode.Id));
            }
        }
    }
}
