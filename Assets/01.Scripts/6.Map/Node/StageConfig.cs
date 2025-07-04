using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageConfig : ScriptableObject
{
    [Header("공통 설정")]
    public NodeType FarthestNode;
    public float BattleWeight;
    public float ShopWeight;
    public float RestWeight;
    public float EventWeight;
    public float EmptyWeight;

    public abstract IMapGenerator CreateGenerator(
        int columns,
        int rows,
        int currentRunCount,
        StageProgress stageProgress
    );

}
