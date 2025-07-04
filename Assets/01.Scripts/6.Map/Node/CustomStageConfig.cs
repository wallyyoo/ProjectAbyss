using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/CustomStageConfig")]
public class CustomStageConfig : StageConfig
{
    [Header("Custom 전용: 패턴 파라미터")]
    public PatternType PatternType;
    [SerializeField] private int RingHorizontalRadius;
    [SerializeField] private int RingVerticalRadius;
    [SerializeField] private int RingThickness;
    [SerializeField] private int PyramidLevels;
    [SerializeField] private int DiagonalWidth;
    [SerializeField] private int DiagonalHeight;
    [SerializeField] private int DiagonalOffset;
    [SerializeField] private int CrossArmLength;
    [SerializeField] private int CrossThickness;
    [SerializeField] private int BossInnerRadius;
    [SerializeField] private int BossOuterDistance;
    [SerializeField] private int RingRadius;
    [SerializeField] private int RingInnerHalf;

    
    

    public override IMapGenerator CreateGenerator(
        int columns,
        int rows,
        int currentRunCount,
        StageProgress stageProgress)
    {
        List<Vector2Int> pattern = PatternType switch
        {
            PatternType.CircularRing => MapPatternLibrary.CreateCircularRing(RingHorizontalRadius, RingVerticalRadius,
                RingThickness),
            PatternType.Pyramid => MapPatternLibrary.CreatePyramid(PyramidLevels),
            PatternType.Diagonal => MapPatternLibrary.CreateDiagonal(DiagonalWidth, DiagonalHeight, DiagonalOffset),
            PatternType.Cross => MapPatternLibrary.CreateCross(CrossArmLength, CrossThickness),
            PatternType.BossRing => MapPatternLibrary.CreateBossRing(BossInnerRadius, BossOuterDistance),
            PatternType.ColliderRing => MapPatternLibrary.CreateRectangularRing(RingRadius,RingInnerHalf)
            
        };
        var assigner = new NodeTypeAssigner(BattleWeight,ShopWeight,RestWeight,EventWeight,EmptyWeight);

        return new CustomMapGenerator(PatternType,
            pattern, assigner);
    }
    
}
