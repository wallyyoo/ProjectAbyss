using UnityEngine;

[System.Serializable]
public class AnimationClipData
{
    public string animatorStateName; // Animator 상태 이름 ex) Idle, Attack, Hit, Dead
    public float duration;   // 애니메이션 길이

    [HideInInspector]
    public int stateHash;    // Animator.Play 호출용 해시값

    public void Initialize()
    {
        stateHash = Animator.StringToHash(animatorStateName);
    }
}