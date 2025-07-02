using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageConfiguration
{
    public int StageNumber { get; }
    public int SubStageNumber { get; }

    public StageConfiguration(int stageNumber, int subStageNumber)
    {
        StageNumber = stageNumber;
        SubStageNumber = subStageNumber;
    }

    public StageType StageType
    {
        get
        {
            if (StageNumber == 1 && SubStageNumber >= 1 && SubStageNumber <= 3)
                return StageType.Exploration;
            if (StageNumber == 1 && SubStageNumber > 3)
                return StageType.Boss;

            return StageType.Exploration;
        }
        
    }

    private (int Min, int Max) GetRoomCountRange()
    {
        if (StageNumber == 1)
        {
            switch (SubStageNumber)
            {
                case 1:
                    return (18, 22);
                case 2:
                    return (20, 24);
                case 3:
                    return (22, 26);
            }
        }

        return (0, 0);
    }

    public int CalculateRoomCount()
    {
        (int minCount, int maxCount) = GetRoomCountRange();
        return Random.Range(minCount, maxCount+1);
    }
}
