using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    private TurnManager turnManager;
    private UIManager uiManager;

    [Header("스탯")]
    private int currentHP;
    private int damage;

    public bool IsAlive => currentHP > 0;

    void Awake()
    {
        turnManager = TurnManager.Instance;
        uiManager = UIManager.Instance;
        currentHP = data.MaxHP;
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
            // 사망 처리 결과창
        }
    }

    /// <summary>
    /// 데미지 수치 받아오는 메서드
    /// </summary>
    /// <returns></returns>
    public void GetAttackDamage(int value)
    {
        damage = value; // 이 부분에 주사위 데미지 계산 부분 
    }
}
