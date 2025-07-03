using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/GridStageConfig")]
public class GridStageConfig : StageConfig
{
    [Header("Grid전용: 방 개수 범위")]
    public int MinRoomCount;
    public int MaxRoomCount;

    public override IMapGenerator CreateGenerator(
        int columns,
        int rows,
        int currnetRunCount,
        StageProgress stageProgress
    )
    {
        int roomCount = Random.Range(MinRoomCount, MaxRoomCount + 1);
        var assigner = new NodeTypeAssigner(
            BattleWeight, ShopWeight, RestWeight, EventWeight, EmptyWeight
        );
        return new GridMapGenerator(
            columns, rows, roomCount, assigner);
    }
}
