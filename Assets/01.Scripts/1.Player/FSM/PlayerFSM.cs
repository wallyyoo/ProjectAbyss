using System.Collections;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    private PlayerState currentState;

    private Animator animator; // 플레이어 애니메이터
    private PlayerAnimationData animationData;

    public void Init(Animator animator, PlayerAnimationData animData)
    {
        this.animator = animator;
        this.animationData = animData;
        EnterState(PlayerState.Idle);
    }

    public void EnterState(PlayerState newState)
    {
        currentState = newState;
        StopAllCoroutines();

        switch (newState)
        {
            case PlayerState.Idle:
                animator.Play(animationData.idle.stateHash);
                break;
            case PlayerState.Attack:
                StartCoroutine(AttackRoutine());
                break;
            case PlayerState.Hit:
                StartCoroutine(HitRoutine());
                break;
            case PlayerState.Dead:
                StartCoroutine(DeadRoutine());
                break;
        }
    }

    private IEnumerator AttackRoutine()
    {
        animator.Play(animationData.attack.stateHash);
        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(animationData.attack.duration);

        // 공격 끝났다는 신호를 보내자 (예: 턴매니저 호출)
        TurnManager.Instance.PlayerAttackFinished();

        // 다시 Idle 상태로
        if (currentState == PlayerState.Attack)
        EnterState(PlayerState.Idle);
    }

    private IEnumerator HitRoutine()
    {
        animator.Play(animationData.hit.stateHash);
        yield return new WaitForSeconds(animationData.hit.duration);
        EnterState(PlayerState.Idle);
    }

    private IEnumerator DeadRoutine()
    {
        animator.Play(animationData.dead.stateHash);
        yield return new WaitForSeconds(animationData.dead.duration);
        EnterState(PlayerState.Idle);
    }
}
