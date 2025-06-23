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
    private int _nodeId;
    private Action<int> _onClick;

    /// <summary>
    /// 뷰 초기화
    /// </summary>
    public void Initialize(int nodeId, Vector2 position, Action<int> onClick)
    {
        _nodeId = nodeId;
        transform.localPosition = position;
        _onClick = onClick;
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        _onClick?.Invoke(_nodeId);
    }
}
