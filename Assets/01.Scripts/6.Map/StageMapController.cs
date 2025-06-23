using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMapController : MonoBehaviour
{
    [SerializeField] private StageMapView _stageMapView = default;
        private IGraphGenerator _graphGenerator;
    
        private void Awake()
        {
            if(_stageMapView == null)
                _stageMapView = GetComponent<StageMapView>();
            
            // 분기 확률 60%, 메인 경로 길이 6, 최대 분기 2
            _graphGenerator = new StageGraphGenerator(0.6f);
        }

        private void Start()
        {
            OnStageCleared();
        }
        /// <summary>
        /// 플레이어가 스테이지를 클리어할 때마다 호출
        /// </summary>
        public void OnStageCleared()
        {
            StageGraphData newGraph = _graphGenerator.Generate(mainPathLength: 6, maxBranches: 2);
            _stageMapView.DrawGraph(newGraph);
        }
}
