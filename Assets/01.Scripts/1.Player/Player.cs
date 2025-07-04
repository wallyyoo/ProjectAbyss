using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    [Header("UI")]
    [SerializeField] private PlayerUI playerUI;

    [Header("Animations")]
    [SerializeField] private PlayerAnimationData animationData;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerFSM playerFSM;

    private int maxHP;
    private int currentHP;

    public bool IsAlive => currentHP > 0;

    public void Init()
    {
        ApplyUpgradeMaxHP();
        currentHP = maxHP;
        UpdateHP();
    }

    /// <summary>
    /// 업그레이드시 호출, maxHP 갱신용
    /// </summary>
    public void ApplyUpgradeMaxHP()
    {
        int baseHP = data.MaxHP;
        var statData = PlayerProgressManager.Instance?.Progress?.
        GetStatData(PlayerStatType.MaxHP);

        int addHP = statData?.add_Stats ?? 0;
        maxHP = baseHP + addHP;

        currentHP = maxHP;
        UpdateHP();
    }

    private void OnEnable()
    {
        TurnManager.Instance.RegisterPlayer(this);
        PlayerProgressManager.Instance.OnStatUpgraded += StatUpgraded;
        UpdateHP();
    }

    private void OnDisable()
    {
        if (PlayerProgressManager.Instance != null)
        PlayerProgressManager.Instance.OnStatUpgraded -= StatUpgraded;
    }

    /// <summary>
    /// 플레이어 데미지 받는 메서드
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, float counterDamageReduction)
    {
        float result = damage * (1f - counterDamageReduction);
        currentHP -= Mathf.RoundToInt(result);

        Debug.Log($"현재 최대 체력 : {maxHP}");
        Debug.Log($"{data.CharacterName}이(가) {damage} 데미지! 남은 체력: {currentHP}");

        UpdateHP();

        if (currentHP <= 0)
        {
            playerFSM.EnterState(PlayerState.Dead); // 사망 애니메이션 상태
            // 사망 결과창
        }
        else
        {
            playerFSM.EnterState(PlayerState.Hit); // 피격 애니메이션 재생
        }
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        UpdateHP();
    }

    private void UpdateHP()
    {
        if (playerUI != null && gameObject.activeInHierarchy)
        {
            playerUI.SetHP(currentHP, maxHP);
        }
    }

    /// <summary>
    /// 이벤트 호출용
    /// </summary>
    /// <param name="type"></param>
    private void StatUpgraded(PlayerStatType type)
    {
        if (type == PlayerStatType.MaxHP)
        {
            ApplyUpgradeMaxHP();
        }
    }

    // ====== 애니메이션 관련 ======
    private void Start()
    {
        animationData.Initialize();
        playerFSM.Init(animator, animationData);
    }
}
