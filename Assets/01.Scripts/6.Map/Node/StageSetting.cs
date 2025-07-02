using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSetting", menuName = "Game/StageSetting")]
public class StageSetting : ScriptableObject
{
    [Tooltip("스테이지 유형 별 노드 가중치")]
    
    public StageType stageType;
    
    [Header("RoomCount")]
    public int baseRoomCount;
    public int ExtraRoomPerStage;
    
    [Header("Farthest Node")]
    public NodeType FarthestNode;
    
    [Header("Node Type Weights")]
    public float battleWeight;
    public float shopWeight;
    public float restWeight;
    public float eventWeight;
    public float emptyWeight;
}
