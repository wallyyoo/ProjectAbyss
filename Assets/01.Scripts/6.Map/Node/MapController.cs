using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 맵 생성부터 클릭 처리, 노드 이동 로직을 총괄
/// </summary>
public class MapController : MonoBehaviour
{
    [Header("Map Generator Settings")]
    [SerializeField] private MapType _mapType = MapType.Grid;
    [SerializeField] private PatternType _patternType = PatternType.CircularRing;
    
    
    [Header("Grid Parameter")]
    [SerializeField] private int _columns = 9;
    [SerializeField] private int _rows = 5;
    [SerializeField] private int _roomCount = 12;
    [SerializeField] private float CellSize = 150f;

    [Header("Pattern Parameter")]
    [SerializeField] private int _ringHorizontalRadius;
    [SerializeField] private int _ringVerticalRadius;
    [SerializeField] private int _ringThickness;
    [SerializeField] private int _pyramidLevels;
    [SerializeField] private int _diagonalWidth;
    [SerializeField] private int _diagonalHeight;
    [SerializeField] private int _diagonalOffset;
    [SerializeField] private int _crossArmLength;
    [SerializeField] private int _crossThickness;
    
    
    [Header("Node 확률 가중치")]
    [SerializeField] private float _battleWeight = 1.0f;
    [SerializeField] private float _shopWeight = 0.2f;
    [SerializeField] private float _rewardWeight = 0.1f;
    [SerializeField] private float _eventWeight = 0.2f;
    
    
    
    [Header("Debug")]
    [SerializeField] private bool _useDebugRunCount = false;
    [SerializeField] private int _debugRunCount = 0;
    
    [Header("References")]
    [SerializeField] private NodeView _nodePrefab;
    [SerializeField] private EdgeView _edgePrefab;
    [SerializeField] private Transform _edges;
    [SerializeField] private Transform _nodes;
    [SerializeField] private CameraSwitcher _cameraSwitcher;
    
    private MapModel _mapModel;
    private Dictionary<int, NodeView> _nodeViews;
    private Dictionary<(int,int),EdgeView> _edgeViews; 
    private HashSet<int> _visitedNodes;
    private int _currentNodeId;
    private int _previousRunCount;
    private int _currentRunCount;
    private INodeRevealStrategy _nodeRevealStrategy;
    
    
    private void Start()
    { 
        SaveData save = SaveLoadManager.LoadGame();
        int storedRunCount = (save != null) ? save.RunCount : 0;
        _previousRunCount = (_useDebugRunCount) ? _debugRunCount : storedRunCount;
        
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
            NodeTypeAssigner nodeTypeAssigner = new NodeTypeAssigner(_battleWeight, _shopWeight, _rewardWeight, _eventWeight);
            BossRoomSelector bossRoomSelector = new BossRoomSelector();
            IMapGenerator generator;
            if (_mapType == MapType.Grid)
            {
                generator = new GridMapGenerator(_columns, _rows,_roomCount, nodeTypeAssigner, bossRoomSelector);
            }
            else
            {
                List<Vector2Int> pattern = GetCustomPattern(); 
                generator = new CustomMapGenerator(pattern, nodeTypeAssigner, bossRoomSelector);
            }

            _mapModel = generator.Generate(0, 0, 0);
            // //기존 랜덤 맵 노드
            // //_mapModel = new GridMapGenerator(_columns,_rows, _roomCount,_nodeTypeAssigner,_bossRoomSelector).Generate(0, 0, 0);
            //
            // //패턴으로 제작
            // //링패턴
            // //List<Vector2Int> _pattern = MapPatternLibrary.CreateCircularRing(8,6,3);
            // //피라미드
            // //List<Vector2Int> _pattern = MapPatternLibrary.CreatePyramid(6);
            // //십자가
            //  List<Vector2Int> _pattern = MapPatternLibrary.CreateCross(6, 4);
            // //평행사변형
            // //List<Vector2Int> _pattern = MapPatternLibrary.CreateDiagonal(5,7);
            //
            // _mapModel = new CustomMapGenerator(_pattern, _nodeTypeAssigner,_bossRoomSelector).Generate(0, 0, 0);
            
            
            //수정하지 않는 로직
            _currentNodeId = _mapModel.Nodes[0].Id;
            _visitedNodes = new HashSet<int>{_currentNodeId};

            SaveGameWithRunCount();
        }
        _currentRunCount = _previousRunCount + 1;
        
        _nodeRevealStrategy = new RunCountRevealStrategy(
            _mapModel,
            _currentRunCount,
            _currentNodeId,
            _visitedNodes);
        
        RenderMap();
        UpdateCurrentLocationDisplay();
        ApplyHighlights();
    }

    /// <summary>
    /// 맵 모델에 따라 뷰를 인스턴스화
    /// </summary>
    private void RenderMap()
    {
        _nodeViews = new Dictionary<int, NodeView>();
        _edgeViews = new Dictionary<(int,int),EdgeView>();
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
            bool isReveal = _nodeRevealStrategy.ShouldReveal(node);
            bool isVisited = _visitedNodes.Contains(node.Id);

            NodeType displayType = (isVisited || isReveal)
                ? node.Type
                : NodeType.Unknown;
            view.SetType(displayType);
            _nodeViews[node.Id] = view;
        }
        
        //엣지 생성
        foreach (EdgeModel edge in _mapModel.Edges)
        {
            Vector2 from =screenPositions[edge.FromNodeId];
            Vector2 to = screenPositions[edge.ToNodeId];
            EdgeView edgeView = Instantiate(_edgePrefab, _edges);
            edgeView.Initialize(from, to);
            
            _edgeViews[(edge.FromNodeId, edge.ToNodeId)] = edgeView;
            _edgeViews[(edge.ToNodeId, edge.FromNodeId)] = edgeView;
        }

        foreach (NodeModel node in _mapModel.Nodes)
        {
            NodeView view = _nodeViews[node.Id];
            view.SetConnectionIndicator(_mapModel, screenPositions);
        }
    }

    private void ApplyHighlights()
    {
        foreach(NodeView nv in _nodeViews.Values)
            nv.SetHighlight(false);
        foreach(EdgeView ev in _edgeViews.Values)
            ev.SetHighlight(false);

        foreach (int nodeId in _nodeRevealStrategy.GetHighlightedNodeIds())
        {
            _nodeViews[nodeId].SetHighlight(true);
        }

        foreach ((int from, int to) in _nodeRevealStrategy.GetHighlightedEdges())
        {
            if(_edgeViews.TryGetValue((from,to), out EdgeView edgeView))
                edgeView.SetHighlight(true);
        }
    }

    private List<Vector2Int> GetCustomPattern()
    {
        switch (_patternType)
        {
            case PatternType.CircularRing:
                return MapPatternLibrary.CreateCircularRing(_ringHorizontalRadius, _ringVerticalRadius,_ringThickness);
            case PatternType.Pyramid:
                return MapPatternLibrary.CreatePyramid(_pyramidLevels);;
            case PatternType.Diagonal:
                return MapPatternLibrary.CreateDiagonal(_diagonalWidth, _diagonalHeight,_diagonalOffset);
            case PatternType.Cross:
                return MapPatternLibrary.CreateCross(_crossArmLength,_crossThickness);
            default:
                Debug.LogWarning($"Unknown PatternType{_patternType}, defaulting to Pyramid");
                return MapPatternLibrary.CreatePyramid(_pyramidLevels);;
        }
    }
    
    /// <summary>
    /// 유효한이동인지 검사 후 로직 수행
    /// </summary>
    private void OnNodeClicked(NodeModel nodeModel)
    {
        Debug.Log($"{nodeModel.Id}노드클릭됨, Type{nodeModel.Type}");
        
        NodeModel currentNode = _mapModel.Nodes.Find(n=> n.Id == _currentNodeId);
        if (!currentNode.ConnectedNodeIds.Contains(nodeModel.Id))
            return;
        
        //1)이전 노드 방문 처리
        _visitedNodes.Add(_currentNodeId);
        Debug.Log($"이동가능 : Node{_currentNodeId} -> Node{nodeModel.Id}");
        
        //2)현재 위치 갱신
        _currentNodeId = nodeModel.Id;
        
        //3)새 위치도 즉시 방문 처리
        _visitedNodes.Add(_currentNodeId);
        
        _nodeRevealStrategy = new RunCountRevealStrategy(
            _mapModel,
            _currentRunCount,
            _currentNodeId,
            _visitedNodes);
        
        
        UpdateAllNodeIcons();
        UpdateCurrentLocationDisplay();
        ApplyHighlights();
        SaveGameWithRunCount();
        _cameraSwitcher.SwitchTo(nodeModel.Type);
        
        
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

    private void UpdateAllNodeIcons()
    {
        foreach (KeyValuePair<int, NodeView> pair in _nodeViews)
        {
            int nodeId = pair.Key;
            NodeView nodeView = pair.Value;

            NodeModel nodeModel = _mapModel.Nodes.Find(n => n.Id == nodeId);
            bool isVisitied = _visitedNodes.Contains(nodeId);
            bool isRevealed = _nodeRevealStrategy.ShouldReveal(nodeModel);
            
                
            NodeType displayType = (isVisitied||isRevealed)
                ? nodeModel.Type:NodeType.Unknown;
            nodeView.SetType(displayType);
        }
    }

    private void SaveGameWithRunCount()
    {
        var save = new SaveData
        {
            Nodes = new List<NodeData>(),
            Edges = new List<EdgeData>(),
            CurrentNodeId = _currentNodeId,
            VisitedNodeIds = new List<int>(_visitedNodes),
            RunCount = _currentRunCount
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
    }
}
