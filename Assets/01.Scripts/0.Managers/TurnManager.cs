using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public enum TurnPhase { Ready, PlayerAttack, EnemyAttack }

public class TurnManager : Singleton<TurnManager>
{
    private Enemy enemy;
    private Player player;
    private int playerAttackDamage;

    private float currentCounterReduction = 0f; // 데미지 감소 값 저장

    private float currentStunChance = 0f;
    private int extraRerollBonus = 0;
    private DiceActor diceActor;
   

    public TurnPhase CurrentPhase { get; private set; } = TurnPhase.Ready;

    /// <summary>
    /// 주사위 로직 가져오기
    /// </summary>
    /// <param name="p"></param>
    public void ResiterDiceActor(DiceActor actor)
    {
        diceActor = actor;
    }
    
    /// <summary>
    /// 활성화 된 플레이어 확인
    /// </summary>
    /// <param name="p"></param>
    public void RegisterPlayer(Player p)
    {
        player = p;
    }

    /// <summary>
    /// 활성화 된 적 확인
    /// </summary>
    /// <param name="e"></param>
    public void RegisterEnemy(Enemy e)
    {
        enemy = e;
    }

    /// <summary>
    /// 페이즈 설정
    /// </summary>
    /// <param name="nextPhase"></param>
    public void SetTurnPhase(TurnPhase nextPhase)
    {
        Debug.Log($"▶ SetTurnPhase 호출됨: {nextPhase}");
        CurrentPhase = nextPhase;

        if (nextPhase == TurnPhase.Ready) //새 턴이 시작하면 주사위 초기화 
        {
            currentCounterReduction = 0f;
            currentStunChance = 0f;
            extraRerollBonus = 0;

            if (diceActor != null)
            {
                diceActor.StartTurn();
            }
        }
    }

    /// <summary>
    /// 플레이어 공격 값 받아오기
    /// </summary>
    /// <param name="attackDamage"></param>
    public void PlayerGetAttackDamage(int attackDamage)
    {
        playerAttackDamage = attackDamage;

        PlayerAttackPhase();
    }

    /// <summary>
    /// 데미지 감소 받아오는 메서드
    /// </summary>
    /// <param name="reduction"></param>
    public void GetCounterReduction(float reduction)
    {
        currentCounterReduction = reduction;
    }

    /// <summary>
    /// 추가 리롤 횟수 저장
    /// </summary>
    /// <param name="rerollBonus"></param>
    public void GetExtraRerollBouns(int rerollBonus)
    {
        extraRerollBonus = rerollBonus;
    }
    public int GetExtraRerollBonusValue()
    {
        return extraRerollBonus;
    }
    
    /// <summary>
    /// 적 기절 확률 저장
    /// </summary>
    /// /// <param name="stunChance"></param>
    public void GetStunChance(float stunChance)
    {
        currentStunChance = stunChance;
    }
    public float GetStunChanceValue()
    {
        return currentStunChance;
    }
    
    /// <summary>
    /// 플레이어 공격 페이즈 시작
    /// </summary>
    [Button("플레이어 공격 테스트")]
    public void PlayerAttackPhase()
    {
        StartCoroutine(PlayerAttackSequence());
    }

    /// <summary>
    /// 플레이어 데미지 받음
    /// </summary>
    /// <param name="damage"></param>
    public void PlayerTakeDamage(int damage)
    {
        player.TakeDamage(damage, currentCounterReduction);
    }

    /// <summary>
    /// 플레이어 턴 끝난 후 적 턴 (반격 페이즈)
    /// </summary>
    public void PlayerTurnEnd()
    {
        CurrentPhase = TurnPhase.EnemyAttack;

        float chance = GetStunChanceValue();
        bool isstuned = Random.value < chance;

        if (isstuned)
        {
            Debug.Log($"적 기절 성공");
            SetTurnPhase(TurnPhase.Ready);
            return;
        }
        
        enemy.ProcessTurn();
    }

    /// <summary>
    /// 플레이어 공격 페이즈 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerAttackSequence()
    {
        CurrentPhase = TurnPhase.PlayerAttack;

        // 애니메이션 실행 부분 플레이어에 추가할 가능성도 있음

        yield return new WaitForSeconds(1); // 공격 애니메이션 시간만큼 기다리게 설정

        if (enemy == null)
        {
            SetTurnPhase(TurnPhase.Ready);
            yield break;
        }

        enemy.TakeDamage(playerAttackDamage); // 사망 처리 포함

        // 적 피격 애니메이션

        // yield return new WaitForSeconds(1); // 피격 애니메이션 시간만큼 기다리게 설정

        if (!enemy.IsAlive)
        {
            // 승리 결과창 출력 메서드 + 페이즈 레디로 다시 설정
            yield break;
        }

        PlayerTurnEnd();
    }
}