using UnityEngine;

public class DiceRollAnimator : MonoBehaviour
{
    
    [SerializeField] private Animator animator;

    public void PlayRoll(DiceColor color)
    {
        string animName = $"DiceRoll_{color}";
        animator.Play(animName,0,0f);
    }
  
}
