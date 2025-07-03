using System.Collections;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using System;

public class DiceRollAnimation : MonoBehaviour
{
    [SerializeField] private DiceSpriteController spriteController;
    private Coroutine currentRoutine;

    private bool isRolling = false;
    public bool IsRolling => isRolling; // 주사위가 돌아가고 있는지 판별용

    public void PlayRollAnimation(DiceColor color, int finalValue, Action onComplete)
    {
        if (isRolling) return; // 중복 실행 방지
        isRolling = true;
        currentRoutine = StartCoroutine(RollAnimationCoroutine(color, finalValue, onComplete));
    }

    private IEnumerator RollAnimationCoroutine(DiceColor color, int finalValue, Action onComplete)
    {

        try
        {
            Tween shake = transform.DORotate(new Vector3(0, 0, 15f), 0.03f) // 주사위 돌아가는 각도, 시간
                                   .SetLoops(10, LoopType.Restart); // 회전수 

            // 그동안 스프라이트를 빠르게 변경
            for (int i = 0; i < 10; i++)
            {
                int tempValue = Random.Range(1, 7);
                spriteController.SetSprite(color, tempValue);
                yield return new WaitForSeconds(0.03f); // 애니메이션 프레임에 맞게 조절
            }

            // 흔들기 애니메이션이 끝날 때까지 기다림
            yield return shake.WaitForCompletion();

            // 마지막 눈값 고정
            spriteController.SetSprite(color, finalValue);
            transform.rotation = Quaternion.identity;
        }
        finally
        {
            isRolling = false;
            currentRoutine = null;
            onComplete?.Invoke();
        }
    }
}
