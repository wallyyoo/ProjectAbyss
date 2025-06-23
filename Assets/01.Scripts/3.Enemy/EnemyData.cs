using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 적이 공통으로 가지는 값 ( 보스, 일반 )
/// </summary>
public abstract class EnemyData : BaseCharacterData
{
    [SerializeField] private int attackDamage;

    [Header("골드 드랍")]
    [SerializeField] private int dropGold;

    [Header("턴 관련")]
    [SerializeField] private int attackTurn;

    public int AttackDamage => attackDamage;
    public int AttackTurn => attackTurn;
    public int DropGold => dropGold;
}
