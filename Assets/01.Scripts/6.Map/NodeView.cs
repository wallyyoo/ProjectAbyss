using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

/// <summary>
/// 화면에 표시되는 노드 뷰. 클릭 이벤트를 MapController에 전달.
/// </summary>
public class NodeView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _roomImage; //방 이미지
    [SerializeField] private Image _indicatorPrefab; // + 표시 프리펩

    [SerializeField] private Color _defaultColor = Color.gray;
    [SerializeField] private Color _currentColor = Color.green;
    [SerializeField] private Color _availableColor = Color.yellow;
    [SerializeField] private Color _visitedColor = Color.white;
    
    
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
            indicator.rectTransform.anchoredPosition = dir * (_roomImage.rectTransform.sizeDelta.x * 0.5f);
            _indicators.Add(indicator);
        }
    }

    public void SetCurrent(bool isCurrent)
    {
        _roomImage.color = isCurrent ? _currentColor : _defaultColor;
    }

    public void SetAvailable(bool isAvailable)
    {
        _roomImage.color = isAvailable ? _availableColor : _defaultColor;
    }

    public void SetVisited(bool isVisited)
    {
        _roomImage.color = isVisited ? _visitedColor : _defaultColor;
    }

    public void SetDefault()
    {
        _roomImage.color = _defaultColor;
    }
}
