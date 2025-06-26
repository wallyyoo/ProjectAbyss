using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    private PlayerProgress progress;

    private TurnManager turnManager;
    private UIManager uiManager;

    [Header("기초 정보")]
    [SerializeField] private TextMeshPro PlayerHP;
    private int currentHP;
    private int damage;

    public bool IsAlive => currentHP > 0;

    void Awake()
    {
        // progress = GetComponent<PlayerProgress>();
        // progress.Init(data);
        turnManager = TurnManager.Instance;
        uiManager = UIManager.Instance;
        // 강화 수치 적용된 progress.MaxHP 로 변경 예정
        currentHP = data.MaxHP;
        UPdateHP();
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
        UPdateHP();
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

    void UPdateHP()
    {
        PlayerHP.text = $"{currentHP}";
    }
}
