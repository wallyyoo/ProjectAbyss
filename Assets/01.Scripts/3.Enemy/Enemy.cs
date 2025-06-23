using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private TextMeshProUGUI turn;
    [SerializeField] private TextMeshProUGUI hp;

    private int currentHP;
    private int attackTurn;
    private int currentTurn;
    public int dropGold;

    private TurnManager turnManager;

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
        UpdateTurn(currentTurn);
        UpdateHP(currentHP);
    }

    private void OnEnable()
    {
        turnManager.RegisterEnemy(this);
    }

    public void ProcessTurn()
    {
        currentTurn--;

        if (currentTurn <= 0)
        {
            AttackPlayer();
            currentTurn = attackTurn; ; // 초기화
            UpdateTurn(currentTurn);
        }
        else
        {
            Debug.Log($"{data.CharacterName}의 남은 공격 턴: {currentTurn}");
            UpdateTurn(currentTurn);
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
            Die();
        }

        UpdateHP(currentHP);
    }

    public void Die()
    {
        turnManager.player.AddGold(dropGold);
        Destroy(gameObject);
    }

    private void AttackPlayer()
    {
        turnManager.PlayerTakeDamage(data.AttackDamage);
    }

    private void UpdateTurn(int currentTurn)
    {
        turn.text = $"{currentTurn}";
    }

    void UpdateHP(int currentHP)
    {
        hp.text = $"{currentHP}";
    }
}