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

    [Header("재화")]
    private int currentGold;
    [SerializeField] private int testamount = 200; // 골드 증가 감소 테스트 수치 (추후 삭제)

    void Awake()
    {
        turnManager = TurnManager.Instance;
        uiManager = UIManager.Instance;
        currentHP = data.MaxHP;
        currentGold = data.StartingGold;
    }

    void Start()
    {
        uiManager.UpdateGold(currentGold);
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
        return 1;
    }

    /// <summary>
    /// 골드 획득 메서드
    /// </summary>
    /// <param name="amount"></param>
    public void AddGold(int amount)
    {
        if (amount < 0) return;

        currentGold += amount;
        Debug.Log($"골드 {amount} 획득! 현재 골드 : {currentGold}");

        uiManager.UpdateGold(currentGold);
    }

    /// <summary>
    /// 골드 소모 메서드
    /// </summary>
    /// <param name="amount"></param>
    public void SpendGold(int amount)
    {
        if (amount < 0) return;

        if (amount > currentGold)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        currentGold -= amount;
        // 업그레이드시 골드 소모
        Debug.Log($"골드 {amount} 소모, 현재 골드 : {currentGold}");

        uiManager.UpdateGold(currentGold);
    }

    // ==================== 테스트 코드 ====================
    [Button("골드 증가")]
    public void TestAddGold()
    {
        if (testamount < 0) return;

        currentGold += testamount;
        Debug.Log($"골드 {testamount} 획득! 현재 골드 : {currentGold}");

        uiManager.UpdateGold(currentGold);
    }

    [Button("골드 감소")]
    public void TestSpendGold()
    {
        if (testamount < 0) return;

        if (testamount > currentGold)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        currentGold -= testamount;
        // 업그레이드시 골드 소모
        Debug.Log($"골드 {testamount} 소모, 현재 골드 : {currentGold}");

        uiManager.UpdateGold(currentGold);
    }
}
