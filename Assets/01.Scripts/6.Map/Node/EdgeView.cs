using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 두 노드를 잇는 선을 그리는 뷰
/// </summary>
public class EdgeView : MonoBehaviour
{
    [SerializeField]private Image _lineImage;
    [SerializeField] private float _thickness = 8f;
    public void Initialize(Vector2 from, Vector2 to)
    {
        RectTransform rt = _lineImage.rectTransform;

        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        Vector2 delta = to - from;
        float length = delta.magnitude;
        float angleDeg = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        
        rt.sizeDelta = new Vector2(length, _thickness);
        transform.localPosition = (from + to) * 0.5f;
        rt.localRotation = Quaternion.Euler(0, 0, angleDeg);
    }

    public void SetHighlight(bool highlight)
    {
        if (_lineImage != null)
        {
            _lineImage.color = highlight ? Color.cyan : Color.yellow;
        }
    }
}
