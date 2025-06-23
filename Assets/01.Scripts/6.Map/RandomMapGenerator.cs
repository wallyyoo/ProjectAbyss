using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레이어별로 노드를 생성하고, 인접레이어에 랜덤으로 연결.
/// </summary>
public class RandomMapGenerator : IMapGenerator
{
    public MapModel Generate(int depth, int minwidth, int maxwidth)
    {
        MapModel mapModel = new MapModel();
        int nextNodeId = 0;
        List<List<NodeModel>> layers = new List<List<NodeModel>>();

        for (int layer = 0; layer < depth; layer++)
        {
            int width = Random.Range(minwidth, maxwidth+1);
            List<NodeModel> nodeLayer = new List<NodeModel>();

            for (int i = 0; i < width; i++)
            {
                NodeType type = (layer == depth - 1) ? NodeType.Boss : NodeType.Battle;
                NodeModel node = new NodeModel(nextNodeId++, type, layer);
                mapModel.Nodes.Add(node);
                nodeLayer.Add(node);
            }
            layers.Add(nodeLayer);
        }

        for (int layer = 0; layer < layers.Count-1; layer++)
        {
            List<NodeModel> current = layers[layer];
            List<NodeModel> next = layers[layer + 1];

            foreach (NodeModel fromNode in current)
            {
                int connections = Random.Range(1, Mathf.Min(3, next.Count + 1));
                HashSet<int> used = new HashSet<int>();
                for (int c = 0; c < connections; c++)
                {
                    int idx;
                    do
                    {
                        idx = Random.Range(0, next.Count);
                    } while (used.Contains(idx));
                    used.Add(idx);

                    NodeModel toNode = next[idx];
                    fromNode.ConnectedNodeIds.Add(toNode.Id);
                    mapModel.Edges.Add(new EdgeModel(fromNode.Id,toNode.Id));


                }
            }
        }

        return mapModel;
    }
}
