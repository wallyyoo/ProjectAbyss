using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeView : MonoBehaviour
{
     [SerializeField] private TextMeshProUGUI _label = default;
     private StageNodeData _data;
     private RectTransform _rt;

     private void Awake()
     {
         _rt = GetComponent<RectTransform>();
     }
     public void Initialize(StageNodeData data)
     {
        _data = data;
        _label.text = data.Type.ToString();
        _rt.anchoredPosition = data.Position;
     }
}
