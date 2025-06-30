using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


/// <summary>
/// 화면에 표시되는 노드 뷰. 클릭 이벤트를 MapController에 전달.
/// </summary>
public class NodeView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _button;
    [SerializeField] private Image _backgroundImage; //방 이미지
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _indicatorPrefab; // 이동가능 연결 부분 표시
    
    [Header("Type Icons")]
    [SerializeField] private Sprite _battleSprite;
    [SerializeField] private Sprite _shopSprite;
    [SerializeField] private Sprite _rewardSprite;
    [SerializeField] private Sprite _eventSprite;
    [SerializeField] private Sprite _startSprite;
    [SerializeField] private Sprite _bossSprite;
    [SerializeField] private Sprite _UnknownSprite;
    
    [Header("Type Color")]
    [SerializeField] private Color _defaultColor = Color.gray;
    [SerializeField] private Color _currentColor = Color.green;
    [SerializeField] private Color _availableColor = Color.yellow;
    [SerializeField] private Color _visitedColor = Color.white;

    [Header("Highlight")] [SerializeField] private GameObject _highlightBorder;
    
    private NodeModel _nodeModel;
    private Action<NodeModel> _onClick;
    private List<Image> _indicators = new List<Image>();
    

    /// <summary>
    /// 뷰 초기화
    /// </summary>
    public void Initialize(NodeModel nodeModel, Vector2 pos, Action<NodeModel> onClick)
    {
        _nodeModel = nodeModel;
        _onClick = onClick;
        transform.localPosition = pos;
        _button.onClick.AddListener(() => _onClick?.Invoke(_nodeModel));
        SetType(_nodeModel.Type);
        SetDefault();
    }

    public void SetConnectionIndicator(MapModel mapModel, Dictionary<int, Vector2> positions)
    {
        foreach (Image img in _indicators) Destroy(img.gameObject);
        _indicators.Clear();
        

        foreach (int neighborId in _nodeModel.ConnectedNodeIds)
        {
            Vector2 myPos = positions[_nodeModel.Id];
            Vector2 otherPos = positions[neighborId];
            Vector2 dir = (otherPos - myPos).normalized;
            
            Image indicator = Instantiate(_indicatorPrefab, transform);
            indicator.rectTransform.anchoredPosition = dir * (_backgroundImage.rectTransform.sizeDelta.x * 0.5f);
            _indicators.Add(indicator);
        }
    }

    public void SetCurrent(bool isCurrent)
    {
        _backgroundImage.color = isCurrent ? _currentColor : _defaultColor;
    }

    public void SetAvailable(bool isAvailable)
    {
        _backgroundImage.color = isAvailable ? _availableColor : _defaultColor;
    }

    public void SetVisited(bool isVisited)
    {
        _backgroundImage.color = isVisited ? _visitedColor : _defaultColor;
    }

    public void SetDefault()
    {
        _backgroundImage.color = _defaultColor;
    }

    public void SetType(NodeType type)
    {
        if (type == NodeType.Unknown)
        {
            _iconImage.sprite = _UnknownSprite;
        }
        else
        {
            _iconImage.sprite = SetupTypeDisplay(type);
        }
    }
    public Sprite SetupTypeDisplay(NodeType type)
    {
        switch (type)
        {
            case NodeType.Battle:
                _iconImage.sprite = _battleSprite;
                break;
            case NodeType.Shop:
                _iconImage.sprite = _shopSprite;
                break;
            case NodeType.Reward: 
                _iconImage.sprite = _rewardSprite;
                break;
            case NodeType.Event: 
                _iconImage.sprite = _eventSprite;
                break;
            case NodeType.Start: 
                _iconImage.sprite = _startSprite;
                break;
            case NodeType.Boss: 
                _iconImage.sprite = _bossSprite;
                break;
            default: _iconImage.sprite = null;
                break;
        }

        return _iconImage.sprite;
    }

    public void SetHighlight(bool highlight)
    {
        if(_highlightBorder != null) 
            _highlightBorder.SetActive(highlight);
    }
}
