using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 두 노드를 잇는 선을 그리는 뷰
/// </summary>
public class EdgeView : MonoBehaviour
{
    [SerializeField]private LineRenderer _lineRenderer;
    
    public void Initialize(Vector2 from, Vector2 to)
    {
        _lineRenderer.useWorldSpace = false;

        Vector3 startPos = new Vector3(from.x, from.y, 0f);
        Vector3 endPos = new Vector3(to.x, to.y, 0f);
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.SetPosition(1, endPos);
    }
}
