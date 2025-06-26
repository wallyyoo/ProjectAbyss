using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.Mathematics;
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
    public void TakeDamage(int damage, float counterDamageReduction)
    {
        float result = damage * (1f - counterDamageReduction);
        currentHP -= Mathf.RoundToInt(result);

        UPdateHP();
        Debug.Log($"{data.CharacterName}이(가) {damage} 데미지! 남은 체력: {currentHP}");

        if (currentHP <= 0)
        {
            // 사망 처리 결과창
        }
    }

    void UPdateHP()
    {
        PlayerHP.text = $"{currentHP}";
    }
}
