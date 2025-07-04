using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// 맵 생성부터 클릭 처리, 노드 이동 로직을 총괄
/// </summary>
public class MapController : MonoBehaviour
{
    [Header("Stage Settings SO")]
    [SerializeField] private ChapterSetting[] _chapterSettingList;
    
    [Header("Grid Parameter")]
    [SerializeField] private int _columns;
    [SerializeField] private int _rows;
    [SerializeField] private int _roomCount;
    [SerializeField] private float CellSize;

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
    private int _endNodeId;
    private INodeRevealStrategy _nodeRevealStrategy;
    private StageProgress _stageProgress;
    private StageConfig CurrentStageConfig => _chapterSettingList
                                              .First(c=>c.ChapterNumber == _stageProgress.Chapter)
                                              .Stages[_stageProgress.StageNumber -1]; 
    private ChapterSetting CurrentChapterSetting
    => _chapterSettingList.First(c => c.ChapterNumber == _stageProgress.Chapter);
    
    private readonly Dictionary<NodeType, INodeClickActionHandler> _clickActionHandlers
        = new Dictionary<NodeType, INodeClickActionHandler>();

    private void Awake()
    {
        InitializeClickActionHandlers();
    }


    /// <summary>
    /// 스테이지 시작 시 호출
    /// </summary>
    private void InitializeStage()
    { 
        //저장된 데이터 로드
        SaveData save = SaveLoadManager.LoadGame();
        
        if (save != null) // 저장된게 있을 때
        {
            RestoreMapFromSave(save);
            Debug.Log($"현재 회차: {_currentRunCount} 현재 챕터 : {save.Progress.Chapter}, 현재 스테이지: {save.Progress.StageNumber}");
            //AssignStartAndEndNodes(CurrentStageConfig);
            
        }
        else // 저장된게 없을 때 새로 만들기
        {
            _previousRunCount = _useDebugRunCount
                ? _debugRunCount
                : 0;
            _stageProgress = new StageProgress
            {
                Chapter = 1,
                StageNumber = 1,
            };
            
            //2) MapModel설정
            CreateMapModel();
            
            AssignStartAndEndNodes(CurrentStageConfig);
            //3) 최초 위치, 방문 초기화
            //_currentNodeId = _mapModel.Nodes[0].Id;
            _visitedNodes = new HashSet<int>{_currentNodeId};
            
            SaveGameWithRunCount();
        }
        
        _currentRunCount = _previousRunCount + 1;
        NodeModel _endNode = _mapModel.Nodes.First(n => n.Id == _endNodeId);
        //4) Reveal 전략 생성
        _nodeRevealStrategy = new RunCountRevealStrategy(
            _mapModel,
            _currentRunCount,
            _currentNodeId,
            _visitedNodes
            , _endNode.Type);
        
        // 5)화면 렌더링
        
        RenderMap();
        UpdateCurrentLocationDisplay();
        ApplyHighlights();
        
    }

    private void Start()
    {
        InitializeStage();
        NodeModel currentNode = _mapModel.Nodes.FirstOrDefault(n => n.Id == _currentNodeId);
        if (currentNode != null)
        {
            _cameraSwitcher.SwitchTo(currentNode.Type);
        }
    }

    private void InitializeClickActionHandlers()
    {
        INodeClickActionHandler[] actionHandlers = new INodeClickActionHandler[]
        {
            new BattleNodeClickActionHandler(),
            new ShopNodeClickHandler(),
            new EventNodeClickActionHandler(),
            new MoveNodeClickActionHandler(),
            new RestNodeClickActionHandler()
        };
        foreach (INodeClickActionHandler actionHandler in actionHandlers)
        {
            _clickActionHandlers.Add(actionHandler.NodeType, actionHandler);
        }
    }
    
    /// <summary>
    /// 맵 모델에 따라 뷰를 인스턴스화
    /// </summary>
    private void RenderMap()
    {
        if (_nodeViews != null)
        {
            foreach(var nv in _nodeViews.Values) Destroy(nv.gameObject);
            foreach(var ev in _edgeViews.Values) Destroy(ev.gameObject);
        }
        
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

    private void OnBossCleared()
    {
        NodeModel currentNode = _mapModel.Nodes.Find(n => n.Id == _currentNodeId);
        if (currentNode.Type != NodeType.Boss)
        {
            Debug.LogWarning($"보스 클리어 호출 시점이 Boss스테이지가 아닙니다.");
            return;
        }
        
        AdvanceStage();
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

    /// <summary>
    /// MapType에 따라 Grid 혹은 Custom - Type 생성기 호출
    /// </summary>
    private void CreateMapModel()
    {
        StageConfig cfg = CurrentStageConfig; 
        
        
        IMapGenerator generator = cfg.CreateGenerator(_columns,_rows, _currentRunCount,_stageProgress);
       _mapModel = generator.Generate(0, 0, 0);
       
       //AssignStartAndEndNodes(cfg);
    }

    private void RestoreMapFromSave(SaveData save)
    {
        //RunCount 설정
        _previousRunCount = _useDebugRunCount
            ? _debugRunCount
            : save.RunCount;
        
        _stageProgress = save.Progress;
        
            
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
        _endNodeId = _mapModel.Nodes.First(n => n.Type == CurrentStageConfig.FarthestNode).Id;
    }
    
    /// <summary>
    /// 유효한이동인지 검사 후 로직 수행
    /// </summary>
    private void OnNodeClicked(NodeModel nodeModel)
    {
        Debug.Log($"{nodeModel.Id}노드클릭됨, Type{nodeModel.Type}");
        
        NodeModel currentNode = _mapModel.Nodes.Find(n=> n.Id == _currentNodeId);
        NodeModel endNode =_mapModel.Nodes.Find(n=> n.Id == _endNodeId);
        if (!currentNode.ConnectedNodeIds.Contains(nodeModel.Id))
            return;
        bool isFirstVisited = !_visitedNodes.Contains(nodeModel.Id);
        
        if (nodeModel.Type == NodeType.Move
            && nodeModel.Id == _endNodeId)
        {
            AdvanceStage();
            return;
        }
        
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
            _visitedNodes
            , endNode.Type);
        
        
        UpdateAllNodeIcons();
        UpdateCurrentLocationDisplay();
        ApplyHighlights();
        SaveGameWithRunCount();
        _cameraSwitcher.SwitchTo(nodeModel.Type);
        if (isFirstVisited)
        {
            HandleNodeTypeAction(nodeModel);
        }


        //TODO: 현재위치 확인 -> 이동가능 여부 검사 -> 씬전환 or 전투 호출 등
    }
    
    // private bool IsFarthestNode(int nodeId)
    // {
    //     int startId = _mapModel.Nodes[0].Id;
    //     int farthestId = new FarthestRoomSelector()
    //         .SelectFarthestRoom(_mapModel.Nodes, startId);
    //     return nodeId == farthestId;
    // }
    private void AdvanceStage()
    {
        
        ChapterSetting chapter = CurrentChapterSetting;

        _stageProgress.StageNumber++;

        // 장 안 스테이지 수 초과 → 새 장으로
        if (_stageProgress.StageNumber > chapter.Stages.Length)
        {
            _stageProgress.Chapter++;
            _stageProgress.StageNumber = 1;
        }
        
        CreateMapModel();
        AssignStartAndEndNodes(CurrentStageConfig);

        if(!(_stageProgress.Chapter==2 && CurrentStageConfig.FarthestNode == NodeType.Boss))
            _currentNodeId = _mapModel.Nodes[0].Id;
        
        _visitedNodes = new HashSet<int> { _currentNodeId };

        _nodeRevealStrategy = new RunCountRevealStrategy(
            _mapModel,
            _currentRunCount,
            _currentNodeId,
            _visitedNodes,
            _mapModel.Nodes.First(n => n.Id == _endNodeId).Type
        );
        RenderMap();
        UpdateCurrentLocationDisplay();
        ApplyHighlights();
            
        SaveGameWithRunCount();
    }
    private void AssignStartAndEndNodes(StageConfig setting)
    {
        Debug.Log("보스 분기 진입");
        List<NodeModel> allNodes = _mapModel.Nodes;
        foreach (var node in _mapModel.Nodes)
        {
            if (node.Type == NodeType.Start || node.Type == NodeType.Boss)
                node.Type = NodeType.Unknown;
        }
        if (setting.FarthestNode == NodeType.Boss && _stageProgress.Chapter == 2)
        {
            Debug.Log("보스 스테이지, 챕터 2 진입");
            int minX = allNodes.Min(n => n.GridPos.x);
            int maxX = allNodes.Max(n => n.GridPos.x);
            int minY = allNodes.Min(n => n.GridPos.y);
            int maxY = allNodes.Max(n => n.GridPos.y);  

            var topCandidates = allNodes.Where(n => n.GridPos.y == maxY);
            var bottomCandidates = allNodes.Where(n => n.GridPos.y == minY);
            var leftCandidates = allNodes.Where(n => n.GridPos.x == minX);
            var rightCandidates = allNodes.Where(n => n.GridPos.x == maxX);

            int direction = Random.Range(0, 4);
            NodeModel startNode;
            switch (direction)
            {
                case 0 : startNode = topCandidates.OrderBy(_=>Random.value).First(); break;
                case 1 : startNode = bottomCandidates.OrderBy(_=>Random.value).First(); break;
                case 2 : startNode = leftCandidates.OrderBy(_=>Random.value).First(); break;
                default : startNode = rightCandidates.OrderBy(_=>Random.value).First(); break;
            }

            _currentNodeId = startNode.Id; 
            startNode.Type = NodeType.Start;
            
            var center = new Vector2Int((minX+ maxX)/2, (minY+maxY)/2);
            NodeModel bossNode = allNodes.First(n => n.GridPos == center);
            bossNode.Type = NodeType.Boss;
            _endNodeId = bossNode.Id;
        }



        else
        {
            NodeModel startNode = _mapModel.Nodes[0];
            startNode.Type = NodeType.Start;
            
            
            IFarthestRoomSelector farthestRoomSelector = new FarthestRoomSelector();
            _endNodeId = farthestRoomSelector.SelectFarthestRoom(
                _mapModel.Nodes,
                startNode.Id);
            
            NodeModel endNode = _mapModel.Nodes
                                              .First(n=>n.Id == _endNodeId);
            endNode.Type = setting.FarthestNode;
        }
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
            bool isVisited = _visitedNodes.Contains(nodeId);
            bool isRevealed = _nodeRevealStrategy.ShouldReveal(nodeModel);
            
                
            NodeType displayType = (isVisited||isRevealed)
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
            RunCount = _currentRunCount,
            Progress = _stageProgress
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

    private void HandleNodeTypeAction(NodeModel nodeModel)
    {
        if (_clickActionHandlers.TryGetValue(nodeModel.Type, out INodeClickActionHandler clickActionHandler))
        {
            clickActionHandler.HandleClick(nodeModel);
        }
        else
        {
            Debug.LogWarning($"{nodeModel.Type}타입에 대한 핸들러가 없습니다.");
        }
    }

    [Button("bossclear")]
    private void bossclear()
    {
        OnBossCleared();
    }
}
