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

    [SerializeField] private int _columns = 9;
    [SerializeField] private int _rows = 5;
    [SerializeField] private int _roomCount = 12;
    [SerializeField] private float CellSize = 150f;

    

    private void Start()
    {
        Debug.Log("MapController.Start() 호출됨");
        _mapModel = new GridMapGenerator(_columns,_rows, _roomCount).Generate(0, 0, 0);
        Debug.Log($"생성된 노드의 개수: {_mapModel.Nodes.Count}, 간선개수:{_mapModel.Edges.Count}");
        RenderMap();
    }

    /// <summary>
    /// 맵 모델에 따라 뷰를 인스턴스화
    /// </summary>
    private void RenderMap()
    {
        _nodeViews = new Dictionary<int, NodeView>();
        Dictionary<int,Vector2> screenPositions = new Dictionary<int, Vector2>();

        foreach (NodeModel node in _mapModel.Nodes)
        {
            float x = (node.GridPos.x - (_columns - 1) * 0.5f) * CellSize;
            float y = (node.GridPos.y - (_rows - 1)*0.5f)* CellSize;
            Vector2 pos = new Vector2(x, y);

            screenPositions[node.Id] = pos;

            NodeView view = Instantiate(_nodePrefab, transform);
            view.Initialize(node.Id, pos, OnNodeClicked);
            _nodeViews[node.Id] = view;
        }

        foreach (EdgeModel edge in _mapModel.Edges)
        {
            Vector2 from =screenPositions[edge.FromNodeId];
            Vector2 to = screenPositions[edge.ToNodeId];
            EdgeView edgeView = Instantiate(_edgePrefab, transform);
            edgeView.Initialize(from, to);
        }

        foreach (NodeModel node in _mapModel.Nodes)
        {
            NodeView view = _nodeViews[node.Id];
            view.SetConnectionIndicator(_mapModel, screenPositions);
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
