using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private TextMeshPro hp;

    private int currentHP;
    private int attackTurn;
    private int currentTurn;
    public int dropGold;

    private TurnManager turnManager;

    public bool IsAlive => currentHP > 0;


    private void Awake()
    {
        attackTurn = data.AttackTurn;
        turnManager = TurnManager.Instance;
    }

    private void Start()
    {
        currentHP = data.MaxHP;
        currentTurn = attackTurn;
        dropGold = data.DropGold;
        UpdateHP(currentHP);
    }

    private void OnEnable()
    {
        turnManager.RegisterEnemy(this);
    }

    /// <summary>
    /// 적 행동
    /// </summary>
    public void ProcessTurn()
    {
        currentTurn--; // SO

        if (currentTurn <= 0)
        {
            AttackPlayer();
            currentTurn = attackTurn; ; // 초기화
        }
        else
        {
            Debug.Log($"{data.CharacterName}의 남은 공격 턴: {currentTurn}");
            // enemyFSM.EnterState(EnemyState.Stun);
        }

        turnManager.SetTurnPhase(TurnPhase.Ready);
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
            Die();
        }

        UpdateHP(currentHP);
    }

    public void Die()
    {
        // PlayerProgressManager.Instance.
        Destroy(gameObject);
    }

    private void AttackPlayer()
    {
        // enemyFSM.EnterState(PlayerState.Attack);
        turnManager.PlayerTakeDamage(data.AttackDamage);
    }

    void UpdateHP(int currentHP)
    {
        hp.text = $"{currentHP}";
    }
}