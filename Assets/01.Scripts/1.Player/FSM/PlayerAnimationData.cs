using UnityEngine;

[CreateAssetMenu(menuName = "Animation/PlayerAnimationData")]
public class PlayerAnimationData : ScriptableObject
{
    [Header("공통 애니메이션")]
    public AnimationClipData idle;    // 대기 모션
    public AnimationClipData hit;     // 피격 모션
    public AnimationClipData attack;  // 공격 모션
    public AnimationClipData dead;    // 사망 모션

    /// <summary>
    /// 해시 초기화
    /// </summary>
    public void Initialize()
    {
        idle?.Initialize();
        hit?.Initialize();
        attack?.Initialize();
        dead?.Initialize();
    }
}