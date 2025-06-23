using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    private TurnManager turnManager;


    private int currentHP;
    private int damage;

    void Awake()
    {
        currentHP = data.MaxHP;
    }

    void Start()
    {
        turnManager = TurnManager.Instance;
    }

    private void OnEnable()
    {
        turnManager.RegisterPlayer(this);
    }

    /// <summary>
    /// 플레이어 데미지 받는 메서드
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"{data.CharacterName}이(가) {damage} 데미지! 남은 체력: {currentHP}");

        if (currentHP <= 0)
        {
            // 사망 처리 필요
        }
    }

    /// <summary>
    /// 데미지 수치 받아오는 메서드
    /// </summary>
    /// <returns></returns>
    public int GetAttackDamage()
    {
        // 주사위 로직에서 데미지 받아오기
        // damage 에 할당
        return 0;
    }
}
