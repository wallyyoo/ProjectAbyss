using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵 생성기 인터페이스: 다양한 알고리즘 교체 가능
/// </summary>
public interface IMapGenerator
{
    MapModel Generate(int depth, int minWidth, int maxWidth);
}
