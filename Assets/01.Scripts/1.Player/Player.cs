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

    [Header("기초 정보")]
    [SerializeField] private TextMeshPro PlayerHP;
    private int currentHP;

    public bool IsAlive => currentHP > 0;

    void Awake()
    {
        // progress = GetComponent<PlayerProgress>();
        // progress.Init(data);
        // 강화 수치 적용된 progress.MaxHP 로 변경 예정
        currentHP = data.MaxHP;
        UpdateHP();
    }

    private void OnEnable()
    {
        TurnManager.Instance.RegisterPlayer(this);
    }

    /// <summary>
    /// 플레이어 데미지 받는 메서드
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, float counterDamageReduction)
    {
        float result = damage * (1f - counterDamageReduction);
        currentHP -= Mathf.RoundToInt(result);

        UpdateHP();
        Debug.Log($"{data.CharacterName}이(가) {damage} 데미지! 남은 체력: {currentHP}");

        if (currentHP <= 0)
        {
            // 사망 처리 결과창
        }
    }

    void UpdateHP()
    {
        PlayerHP.text = $"{currentHP}";
    }
}
