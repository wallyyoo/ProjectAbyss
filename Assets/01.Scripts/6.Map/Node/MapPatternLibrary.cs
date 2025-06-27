using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapPatternLibrary
{
    /// <summary>정사각형 링 패턴</summary>
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
    public static List<Vector2Int> CreateDiagonal(int length)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                positions.Add(new Vector2Int(j,-i));
            }
        }
        return positions;
    }

    ///<summary>십자 패턴</summary>
    public static List<Vector2Int> CreateCross(int armLength)
    {
        List<Vector2Int>positions = new List<Vector2Int>();
        for (int x = -armLength; x <= armLength; x++)
        {
            positions.Add(new Vector2Int(x,0));
        }

        for (int y = -armLength; y <= armLength; y++)
        {
            if (y != 0)
            {
                positions.Add(new Vector2Int(0,y));
            }
        }
        return positions;
    }
}
