using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레이어별로 노드를 생성하고, 인접레이어에 랜덤으로 연결.
/// </summary>
public class GridMapGenerator : IMapGenerator
{
    private readonly int _columns;
    private readonly int _rows;
    private readonly int _roomCount;
    private readonly INodeTypeAssigner _nodeTypeAssigner;
    private readonly IBossRoomSelector _bossRoomSelector;

    public GridMapGenerator(int columns, int rows, int roomCount, INodeTypeAssigner nodeTypeAssigner, IBossRoomSelector bossRoomSelector)
    {
        _columns = columns;
        _rows = rows;
        _roomCount = roomCount;
        _nodeTypeAssigner = nodeTypeAssigner;
        _bossRoomSelector = bossRoomSelector;
    }
    

    public MapModel Generate(int unusedDepth, int unused1, int unused2)
    {
        MapModel mapModel = new MapModel();
        bool[,] occupied = new bool[_columns, _rows];

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        int nextId = 0;
        
        //시작점
        Vector2Int start = new Vector2Int(_columns / 2, _rows / 2);
        queue.Enqueue(start);
        occupied[start.x, start.y] = true;
        NodeModel startNode = new NodeModel(nextId++, NodeType.Start, start);
        mapModel.Nodes.Add(startNode);

        while (mapModel.Nodes.Count < _roomCount && queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            Vector2Int[] dirs =
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)

            };
            foreach (Vector2Int dir in dirs)
            {
                if (mapModel.Nodes.Count >= _roomCount) break;
                
                Vector2Int neighbor = current + dir;

                if (neighbor.x < 0 || neighbor.x >= _columns || neighbor.y < 0 || neighbor.y >= _rows)
                    continue;

                if (occupied[neighbor.x, neighbor.y]) continue;
                
                //인접 방이 2개이상이면 방 생성하지 않는로직 좀 더 선형적인 구성 가능
                // int adjacentCount = 0;
                //
                //  foreach (Vector2Int d2 in dirs)
                //  {
                //      Vector2Int adj = neighbor + d2;
                //      if(adj.x>= 0 && adj.x < _columns && adj.y >= 0 && adj.y < _rows)
                //      {
                //          if (occupied[adj.x, adj.y]) adjacentCount++;
                //      }
                //  }
                //
                //  if (adjacentCount > 1) continue;

                if (Random.value < 0.3f) continue;
                
                NodeType assignedType = _nodeTypeAssigner.AssignType(mapModel.Nodes.Count,_roomCount);
                
                
                occupied[neighbor.x, neighbor.y] = true;
                NodeModel node = new NodeModel(nextId++, assignedType, neighbor);
                mapModel.Nodes.Add(node);
                queue.Enqueue(neighbor);
                
                // mapModel.Edges.Add(new EdgeModel(
                //     mapModel.Nodes.Find(n=>n.GridPos == current).Id,
                //     node.Id
                //     ));

                var edge = new EdgeModel(mapModel.Nodes.Find(n=>n.GridPos == current).Id, node.Id);
                mapModel.Edges.Add(edge);
                var fromNode = mapModel.Nodes.Find(n=>n.GridPos == current);
                var toNode = node;
                fromNode.ConnectedNodeIds.Add(toNode.Id);
                toNode.ConnectedNodeIds.Add(fromNode.Id);


            }
        }
        int startNodeId = mapModel.Nodes[0].Id;
        int bossNodeId = _bossRoomSelector.SelectBoss(mapModel.Nodes,startNodeId);
        var bossNode = mapModel.Nodes.Find(n => n.Id == bossNodeId);
        bossNode.Type = NodeType.Boss;
        return mapModel;
    }
}
