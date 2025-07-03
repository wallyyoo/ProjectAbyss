using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 한 장(Chapter) 안에서 어떤 순서로 스테이지를 진행할 지 정의
/// 예: [Exploration, Corridor, Exploration, Boss]
/// </summary>
[CreateAssetMenu(menuName = "Map/ChapterSetting")]
public class ChapterSetting : ScriptableObject
{
    [Tooltip("장 번호")]
    public int ChapterNumber;

    [Tooltip("이 장에서 순서대로 등장할 스테이지 타입 목록")]
    public StageConfig[] Stages;

}
