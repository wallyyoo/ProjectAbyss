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
    
    
    private int _nodeId;
    private Action<int> _onClick;
    private List<Image> _indicators = new List<Image>();
    

    /// <summary>
    /// 뷰 초기화
    /// </summary>
    public void Initialize(int nodeId, Vector2 pos, Action<int> onClick)
    {
        _nodeId = nodeId;
        _onClick = onClick;
        transform.localPosition = pos;

        _button.onClick.AddListener(() => _onClick?.Invoke(_nodeId));
    }

    public void SetConnectionIndicator(MapModel mapModel, Dictionary<int, Vector2> positions)
    {
        foreach (Image img in _indicators) Destroy(img.gameObject);
        _indicators.Clear();
        
        NodeModel self = mapModel.Nodes.Find(n=> n.Id == _nodeId);

        foreach (int neighborId in self.ConnectedNodeIds)
        {
            Vector2 myPos = positions[_nodeId];
            Vector2 otherPos = positions[neighborId];
            Vector2 dir = (otherPos - myPos).normalized;
            
            Image indicator = Instantiate(_indicatorPrefab, transform);
            indicator.rectTransform.anchoredPosition = dir * (_roomImage.rectTransform.sizeDelta.x * 0.5f);
            _indicators.Add(indicator);
        }
    }
}
