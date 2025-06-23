using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵 생성부터 클릭 처리, 노드 이동 로직을 총괄
/// </summary>
public class MapController : MonoBehaviour
{
    [SerializeField] private NodeView _nodePrefab;
    [SerializeField] private EdgeView _edgePrefab;

    private MapModel _mapModel;
    private Dictionary<int, NodeView> _nodeViews;

    private const float LayerYStep = -200f;
    private const float NodeXStep = 200f;

    private void Start()
    {
        Debug.Log("MapController.Start() 호출됨");
        IMapGenerator generator = new RandomMapGenerator();
        _mapModel = generator.Generate(depth: 4, minWidth: 3, maxWidth: 5);
        Debug.Log($"생성된 노드의 개수: {_mapModel.Nodes.Count}, 간선개수:{_mapModel.Edges.Count}");
        RenderMap();
    }

    /// <summary>
    /// 맵 모델에 따라 뷰를 인스턴스화
    /// </summary>
    private void RenderMap()
    {
        _nodeViews = new Dictionary<int, NodeView>();
        Dictionary<int, Vector2> positions = new Dictionary<int, Vector2>();
        
        
        Dictionary<int, List<NodeModel>> nodesByLayer = new Dictionary<int, List<NodeModel>>();
        foreach(NodeModel node in _mapModel.Nodes)
        {
            if (!nodesByLayer.ContainsKey(node.Layer))
            {
                nodesByLayer[node.Layer] = new List<NodeModel>();
            }

            nodesByLayer[node.Layer].Add(node);
        }

        foreach (KeyValuePair<int,List<NodeModel>> kvp in nodesByLayer)
        {
            int layerIndex = kvp.Key;
            List<NodeModel> layerNodes = kvp.Value;
            int count = layerNodes.Count;

            for (int i = 0; i < count; i++)
            {
                NodeModel node = layerNodes[i];

                float x = (i - (count - 1) * 0.5f )* NodeXStep;
                float y = layerIndex * LayerYStep;
                Vector2 pos = new  Vector2(x, y);
                positions[node.Id] = pos;
                NodeView view = Instantiate(_nodePrefab, transform);
                view.Initialize(node.Id, pos, OnNodeClicked);
                _nodeViews[node.Id] = view;
            }
        }

        foreach (EdgeModel edge in _mapModel.Edges)
        {
            Vector2 from = positions[edge.FromNodeId];
            Vector2 to = positions[edge.ToNodeId];
            EdgeView edgeView = Instantiate(_edgePrefab, transform);
            edgeView.Initialize(from,to);
        }
    }

    /// <summary>
    /// 유효한이동인지 검사 후 로직 수행
    /// </summary>
    private void OnNodeClicked(int nodeId)
    {
        Debug.Log($"{nodeId}노드클릭됨.");
        //TODO: 현재위치 확인 -> 이동가능 여부 검사 -> 씬전환 or 전투 호출 등
    }
}
