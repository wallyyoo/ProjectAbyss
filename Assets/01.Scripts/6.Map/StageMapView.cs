using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMapView : MonoBehaviour
{
    [SerializeField] private RectTransform _container = default;
    [SerializeField] private GameObject _nodePrefab = default;
    [SerializeField] private GameObject _linePrefab = default;

    public void DrawGraph(StageGraphData graph)
    {
        foreach (StageNodeData node in graph.Nodes)
        {
            GameObject nodeObject = Instantiate(_nodePrefab, _container);
            nodeObject.GetComponent<NodeView>().Initialize(node);

            // 연결선 그리기
            foreach (StageNodeData neighbor in node.Connections)
            {
                if (neighbor.Id > node.Id) // 중복 방지
                {
                    DrawLine(node.Position, neighbor.Position);
                }
            }
        }
    }

    private void DrawLine(Vector2 start, Vector2 end)
    {
        GameObject lineObject = Instantiate(_linePrefab, _container);
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
