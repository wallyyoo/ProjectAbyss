using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;

    private int currentHP;
    private int attackTurn;
    private int currentTurn;

    private TurnManager turnManager;

    private void Awake()
    {
        attackTurn = data.AttackTurn;
    }

    private void Start()
    {
        currentHP = data.MaxHP;
        currentTurn = attackTurn;

        turnManager = TurnManager.Instance;
    }

    private void OnEnable()
    {
        turnManager.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        turnManager.UnRegisterEnemy(this);
    }

    public void ProcessTurn()
    {
        currentTurn--;

        if (currentTurn <= 0)
        {
            AttackPlayer();
            currentTurn = attackTurn; ; // 초기화
        }
        else
        {
            Debug.Log($"{data.CharacterName}의 남은 공격 턴: {currentTurn}");
        }
    }

    /// <summary>
    /// 적 데미지 받는 메서드
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

    private void AttackPlayer()
    {
        turnManager.PlayerTakeDamage(data.AttackDamage);
    }
}