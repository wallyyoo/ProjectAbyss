using UnityEngine;

/// <summary>
/// 플레이어와 적이 공통으로 가지는 값
/// </summary>
public abstract class BaseCharacterData : ScriptableObject
{
    [Header("기본 정보")]
    [SerializeField] protected string characterName;

    [Header("기본 능력치")]
    [SerializeField] protected int maxHP;

    // 읽기용 프로퍼티
    public string CharacterName => characterName;
    public int MaxHP => maxHP;
}
