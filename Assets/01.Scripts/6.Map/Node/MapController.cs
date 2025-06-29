using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 맵 생성부터 클릭 처리, 노드 이동 로직을 총괄
/// </summary>
public class MapController : MonoBehaviour
{
    [SerializeField] private NodeView _nodePrefab;
    [SerializeField] private EdgeView _edgePrefab;

    [SerializeField] private Transform _edges;
    [SerializeField] private Transform _nodes;
    
    [SerializeField] private CameraSwitcher _cameraSwitcher;

    //맵 생성 파라미터
    [SerializeField] private int _columns = 9;
    [SerializeField] private int _rows = 5;
    [SerializeField] private int _roomCount = 12;
    [SerializeField] private float CellSize = 150f;
    
    [SerializeField] private float _battleWeight = 1.0f;
    [SerializeField] private float _shopWeight = 0.2f;
    [SerializeField] private float _rewardWeight = 0.1f;
    [SerializeField] private float _eventWeight = 0.2f;

    //호출 하는 Action
    //public Action OnMapNodeAction = OnMapNode;
    //public Action OffMapNodeAction = OffMapNode;
    
    private MapModel _mapModel;
    private Dictionary<int, NodeView> _nodeViews;
    private int _currentNodeId;
    private HashSet<int> _visitedNodes;
    
    
    private void Start()
    { 
        SaveData save = SaveLoadManager.LoadGame();
        if (save != null)
        {
            _mapModel = new MapModel();
            foreach (var nd in save.Nodes)
            {
                var node = new NodeModel(
                    nd.Id,
                    nd.Type,
                    new Vector2Int(nd.X, nd.Y)
                    );
                node.ConnectedNodeIds.AddRange(nd.ConndectedNodeIds);
                _mapModel.Nodes.Add(node);
            }

            foreach (var ed in save.Edges)
            {
                _mapModel.Edges.Add(new EdgeModel(ed.FromNodeId, ed.ToNodeId));
            }
            _currentNodeId = save.CurrentNodeId;
            _visitedNodes = new HashSet<int>(save.VisitedNodeIds);
        }
        else
        {
            NodeTypeAssigner _nodeTypeAssigner = new NodeTypeAssigner(_battleWeight, _shopWeight, _rewardWeight, _eventWeight);
            BossRoomSelector _bossRoomSelector = new BossRoomSelector();

            //기존 랜덤 맵 노드
            //_mapModel = new GridMapGenerator(_columns,_rows, _roomCount,_nodeTypeAssigner,_bossRoomSelector).Generate(0, 0, 0);
            
            //패턴으로 제작
            //링패턴
            //List<Vector2Int> _pattern = MapPatternLibrary.CreateCircularRing(8,6,3);
            //피라미드
            //List<Vector2Int> _pattern = MapPatternLibrary.CreatePyramid(6);
            //십자가
            List<Vector2Int> _pattern = MapPatternLibrary.CreateCross(6, 3);
            //평행사변형
            //List<Vector2Int> _pattern = MapPatternLibrary.CreateDiagonal(5,7);
            
            _mapModel = new CustomMapGenerator(_pattern, _nodeTypeAssigner,_bossRoomSelector).Generate(0, 0, 0);
            
            
            //수정하지 않는 로직
            _currentNodeId = _mapModel.Nodes[0].Id;
            _visitedNodes = new HashSet<int>{_currentNodeId};

            var toSave = new SaveData
            {
                Nodes = new List<NodeData>(),
                Edges = new List<EdgeData>(),
                CurrentNodeId = _currentNodeId,
                VisitedNodeIds = new List<int>(_visitedNodes)
            };
            foreach (var node in _mapModel.Nodes)
            {
                toSave.Nodes.Add(new NodeData
                {
                    Id = node.Id,
                    Type = node.Type,
                    X = node.GridPos.x,
                    Y = node.GridPos.y, 
                    ConndectedNodeIds = new List<int>(node.ConnectedNodeIds)
                });
            }

            foreach (var ed in _mapModel.Edges)
            {
                toSave.Edges.Add(new EdgeData
                {
                        FromNodeId = ed.FromNodeId,
                        ToNodeId = ed.ToNodeId 
                });
            }
            SaveLoadManager.SaveGame(toSave);
        }
        
        RenderMap();
        UpdateCurrentLocationDisplay();
    }

    /// <summary>
    /// 맵 모델에 따라 뷰를 인스턴스화
    /// </summary>
    private void RenderMap()
    {
        _nodeViews = new Dictionary<int, NodeView>();
        Dictionary<int,Vector2> screenPositions = new Dictionary<int, Vector2>();
        
        
        //Custom노드에서 사용
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;
        
        foreach (NodeModel node in _mapModel.Nodes)
        {
            minX = Mathf.Min(minX, node.GridPos.x);
            maxX = Mathf.Max(maxX, node.GridPos.x);
            minY = Mathf.Min(minY, node.GridPos.y);
            maxY = Mathf.Max(maxY, node.GridPos.y);
        }

        float centerX = (minX + maxX) * 0.5f;
        float centerY = (minY + maxY) * 0.5f;

        foreach (NodeModel node in _mapModel.Nodes)
        {
            float x =(node.GridPos.x - centerX)*CellSize;
            float y = (node.GridPos.y - centerY)*CellSize;
            Vector2 pos = new Vector2(x, y);
            screenPositions[node.Id] = pos;
            
            NodeView view = Instantiate(_nodePrefab, _nodes);
            view.Initialize(node,pos,OnNodeClicked);
            _nodeViews[node.Id] = view;
        }
        
        //랜덤노드에서 사용되던 시작점이 중앙에 위치하던 구조
        //위의 로직을 사용하면 노드 전체의 좌우를 기준으로 중앙점을 찾아 화면중앙에 위치해줌
        // foreach (NodeModel node in _mapModel.Nodes)
        // {
        //     float x = (node.GridPos.x - (_columns - 1) * 0.5f) * CellSize;
        //     float y = (node.GridPos.y - (_rows - 1)*0.5f)* CellSize;
        //     Vector2 pos = new Vector2(x, y);
        //
        //     screenPositions[node.Id] = pos;
        //
        //     NodeView view = Instantiate(_nodePrefab, transform);
        //     view.Initialize(node, pos, OnNodeClicked);
        //     _nodeViews[node.Id] = view;
        // }

        foreach (EdgeModel edge in _mapModel.Edges)
        {
            Vector2 from =screenPositions[edge.FromNodeId];
            Vector2 to = screenPositions[edge.ToNodeId];
            EdgeView edgeView = Instantiate(_edgePrefab, _edges);
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
    private void OnNodeClicked(NodeModel nodeModel)
    {
        Debug.Log($"{nodeModel.Id}노드클릭됨, Type{nodeModel.Type}");
        NodeModel currentNode = _mapModel.Nodes.Find(n=> n.Id == _currentNodeId);
        if (currentNode.ConnectedNodeIds.Contains(nodeModel.Id))
        {
            _visitedNodes.Add(_currentNodeId);
            Debug.Log($"이동가능 : Node{_currentNodeId} -> Node{nodeModel.Id}");
            _currentNodeId = nodeModel.Id;
            UpdateCurrentLocationDisplay();

            var save = new SaveData
            {
                Nodes = new List<NodeData>(),
                Edges = new List<EdgeData>(),
                CurrentNodeId = _currentNodeId,
                VisitedNodeIds = new List<int>(_visitedNodes)
            };
            foreach (var node in _mapModel.Nodes)
            {
                save.Nodes.Add(new NodeData
                {
                    Id = node.Id,
                    Type = node.Type,
                    X = node.GridPos.x,
                    Y = node.GridPos.y, 
                    ConndectedNodeIds = new List<int>(node.ConnectedNodeIds)
                });
            }

            foreach (var ed in _mapModel.Edges)
            {
                save.Edges.Add(new EdgeData
                {
                    FromNodeId = ed.FromNodeId,
                    ToNodeId = ed.ToNodeId 
                });
            }
            SaveLoadManager.SaveGame(save);
            
            _cameraSwitcher.SwitchTo(nodeModel.Type);
            
            //전환되고 꺼짐, 씬 종료 되면 다시 켜짐
            //this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("이동불가");
        }
        //TODO: 현재위치 확인 -> 이동가능 여부 검사 -> 씬전환 or 전투 호출 등
    }
    private void UpdateCurrentLocationDisplay()
    {   
        NodeModel currentNode = _mapModel.Nodes.Find(n => n.Id==_currentNodeId);
        
        foreach (KeyValuePair<int, NodeView> pair in _nodeViews)
        {
            int id = pair.Key;
            NodeView view = pair.Value;
            if (id == _currentNodeId)
            {
                view.SetCurrent(true);
            }
            else if (_visitedNodes.Contains(id))
            {
                view.SetVisited(true);
            }
            else if (currentNode.ConnectedNodeIds.Contains(id))
            {
                view.SetAvailable(true);
            }
            else
            {
                view.SetDefault();
            }
        }
    }

    public void OnMapNode()
    {
        gameObject.SetActive(true);
    }

    public void OffMapNode()
    {
        gameObject.SetActive(false);
    }
}
