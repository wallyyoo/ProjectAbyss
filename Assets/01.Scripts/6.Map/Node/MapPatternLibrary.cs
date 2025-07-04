using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapPatternLibrary
{
    /// <summary>
    /// 원형 링 패턴
    /// </summary>
    /// <param name="horizontalRadius">세로 길이</param>
    /// <param name="verticalRadius">가로길이</param>
    /// <param name="thickness">두께</param>
    /// <returns></returns>
    public static List<Vector2Int> CreateCircularRing(int horizontalRadius, int verticalRadius,int thickness)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        int outerA = horizontalRadius;
        int outerB = verticalRadius;
        int innerA = Mathf.Max(1, horizontalRadius - thickness);
        int innerB = Mathf.Max(1, verticalRadius - thickness);
      

        for (int x = -outerA ; x <= outerA; x++)
        {
            for (int y = -outerB; y <= outerB; y++)
            {
                //정규화된 거리: (x/반경A)^2 + (y/반경B)^2
                float normalizedOuter =
                    (x / (float)outerA) * (x / (float)outerA)
                    +(y / (float)outerB) * (y / (float)outerB);
                float normalizedInner =
                    (x / (float)innerA) * (x / (float)innerA)
                    + (y / (float)innerB) * (y / (float)innerB);
                
                if (normalizedOuter <= 1.0f
                    && normalizedInner >= 1.0f)
                {
                    positions.Add(new Vector2Int(x, y));
                }
            }
        }

        positions.RemoveAll(delegate(Vector2Int p)
        {
            bool isHorizontalExtreme = (p.y == 0 && Mathf.Abs(p.x) == outerA);
            bool isVerticalExtreme = (p.x == 0 && Mathf.Abs(p.y) == outerB);
            return (isHorizontalExtreme || isVerticalExtreme);
        });
        
        return positions;
    }

    /// <summary>피라미드 패턴</summary>
    public static List<Vector2Int> CreatePyramid(int levels)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        for (int row = 0; row < levels; row++)
        {
            int halfWidth = row;
            for (int x = -halfWidth; x <= halfWidth; x++)
            {
                positions.Add(new Vector2Int(x,-row));
            }
        }
        return positions;
    }

    /// <summary>피라미드 패턴</summary>
    public static List<Vector2Int> CreateDiagonal(int width, int height, int offsetXperRow = 1)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        int halfWidth = width / 2;
        int halfHeight = height / 2;
        
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col <= width; col++)
            {
                int x = col + row * offsetXperRow-halfWidth - halfHeight*offsetXperRow;
                int y = -row + halfHeight;
                positions.Add(new Vector2Int(x,y));
            }
        }
        return positions;
    }

    ///<summary>십자 패턴</summary>
    public static List<Vector2Int> CreateCross(int armLength, int thickness)
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
        int halfThickness = thickness/2;
        
        for (int x = -armLength; x <= armLength; x++)
        {
            for (int y = -halfThickness; y <= halfThickness; y++)
            {
                positions.Add(new Vector2Int(x,y));
            }
        }

        for (int x = -halfThickness; x <= halfThickness; x++)
        {
            for (int y = -armLength; y<=armLength; y++)
            {
                positions.Add(new Vector2Int(x,y));
            }
        }
        return new List<Vector2Int>(positions);
    }

    public static List<Vector2Int> CreateBossRing(int innerRadius, int outerDistance)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        positions.Add(new Vector2Int(0, outerDistance));
        positions.Add(new Vector2Int(outerDistance, 0));
        positions.Add(new Vector2Int(0, -outerDistance));
        positions.Add(new Vector2Int(-outerDistance, 0));

        for (int x = -innerRadius; x <= innerRadius; x++)
        {
            for (int y = -innerRadius; y <= innerRadius; y++)
            {
                // if (x == 0 && y == 0)
                // {
                //     continue;
                // }
                 positions.Add(new Vector2Int(x, y));
            }
        }

        return positions.Distinct().ToList();
    }

    public static List<Vector2Int> CreateRectangularRing(int outerRadius, int innerHalf)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        if (outerRadius < 1)
        {
            Debug.LogWarning("파라미터 유효하지않음");
            return positions;
        }

        for (int x = -outerRadius; x <= outerRadius; x++)
        {
            for (int y = -outerRadius; y <= outerRadius; y++)
            {
                if (Mathf.Abs(x)+Mathf.Abs(y)>outerRadius)
                {
                    continue;
                }

                if (Mathf.Abs(x) < innerHalf && Mathf.Abs(y) < innerHalf)
                {
                    continue;
                }
                positions.Add(new Vector2Int(x,y));
                    
            }
        }

        return positions;
    }
}
