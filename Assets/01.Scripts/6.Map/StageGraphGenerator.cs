using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IGraphGenerator
{
    StageGraphData Generate(int mainPathLength, int maxBranches);
}
public class StageGraphGenerator : IGraphGenerator
{
    private readonly float _branchChance;
    
        public StageGraphGenerator(float branchChance)
        {
            _branchChance = branchChance;
        }
    
        public StageGraphData Generate(int mainPathLength, int maxBranches)
        {
            StageGraphData graph = new StageGraphData();
    
            // 1) 메인 경로 생성
            List<StageNodeData> mainPath = CreateMainPath(mainPathLength, graph);
    
            // 2) 분기 생성
            CreateBranches(mainPath, maxBranches, graph);
    
            // 3) 노드 위치 배치 (단순 격자 배치 예시)
            LayoutNodes(graph, mainPathLength);
    
            return graph;
        }
    
        private List<StageNodeData> CreateMainPath(int length, StageGraphData graph)
        {
            List<StageNodeData> path = new List<StageNodeData>();
    
            // 시작 노드
            StageNodeData startNode = new StageNodeData(0, StageType.Start);
            graph.AddNode(startNode);
            path.Add(startNode);
    
            // 중간 노드
            for (int index = 1; index < length - 1; ++index)
            {
                StageNodeData middleNode = new StageNodeData(index, SelectRandomStageType());
                graph.AddNode(middleNode);
    
                path[index - 1].AddConnection(middleNode);
                path.Add(middleNode);
            }
    
            // 보스 노드
            StageNodeData bossNode = new StageNodeData(length - 1, StageType.Boss);
            graph.AddNode(bossNode);
            path[path.Count - 1].AddConnection(bossNode);
            path.Add(bossNode);
    
            return path;
        }
    
        private void CreateBranches(List<StageNodeData> mainPath, int maxBranches, StageGraphData graph)
        {
            int nextId = graph.Nodes.Count;
    
            foreach (StageNodeData node in mainPath)
            {
                int branchCount = UnityEngine.Random.Range(0, maxBranches + 1);
    
                for (int i = 0; i < branchCount; ++i)
                {
                    if (UnityEngine.Random.value > _branchChance)
                    {
                        StageNodeData branchNode = new StageNodeData(nextId++, SelectRandomStageType());
                        graph.AddNode(branchNode);
    
                        node.AddConnection(branchNode);
                    }
                }
            }
        }
    
        private void LayoutNodes(StageGraphData graph, int mainPathLength)
        {
            // 1) 시작 노드 찾기
            StageNodeData start = graph.Nodes.Find(n => n.Type == StageType.Start);
            if (start == null) return;

            // 2) BFS로 depth 계산
            var depthMap = new Dictionary<StageNodeData, int>();
            var queue    = new Queue<StageNodeData>();
            depthMap[start] = 0;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                int currentDepth = depthMap[node];

                foreach (var neigh in node.Connections)
                {
                    if (depthMap.ContainsKey(neigh))
                        continue;

                    depthMap[neigh] = currentDepth + 1;
                    queue.Enqueue(neigh);
                }
            }
            // 3) depth 그룹별로 묶어서 위치 지정
            const float X_SPACING = 100f;  // 노드 간 가로 간격 (픽셀 단위)
            const float Y_SPACING = 75f;  // 노드 간 세로 간격

            var groups = depthMap
                         .GroupBy(kv => kv.Value)
                         .OrderBy(g => g.Key);

            foreach (var group in groups)
            {
                int count = group.Count();
                int i = 0;
                foreach (var kv in group)
                {
                    StageNodeData node = kv.Key;
                    // 화면 가운데를 0으로 두고 좌우로 퍼뜨리기
                    float x = (i - (count - 1) / 2f) * X_SPACING;
                    float y = -kv.Value * Y_SPACING;
                    node.Position = new Vector2(x, y);
                    i++;
                }
            }
            
            
            
            
            
            
            
        }
    
        private StageType SelectRandomStageType()
        {
            // 시작, 보스 제외한 랜덤 타입 선택 (확률 조정 가능)
            float randomValue = UnityEngine.Random.value;
    
            if (randomValue < 0.5f)
            {
                return StageType.Battle;
            }
    
            if (randomValue < 0.75f)
            {
                return StageType.Exploration;
            }
    
            return StageType.Treasure;
        }
}
