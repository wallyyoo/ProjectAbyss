using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public enum TurnPhase { Ready, PlayerAttack, EnemyAttack }

public class TurnManager : Singleton<TurnManager>
{
    private Enemy enemy;
    public Player player;
    private int playerAttackDamage;

    public TurnPhase CurrentPhase { get; private set; } = TurnPhase.Ready;

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
        CurrentPhase = nextPhase;
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
        player.TakeDamage(damage);
    }

    /// <summary>
    /// 플레이어 턴 끝난 후 적 턴 (반격 페이즈)
    /// </summary>
    public void PlayerTurnEnd()
    {
        CurrentPhase = TurnPhase.EnemyAttack;

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