using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DiceSpriteController : MonoBehaviour
{
    [Header("이미지에 적용할 타겟")] public Image diceImage; 

    [Header("스프라이트 시트")] [SerializeField] 
    private string spriteSheetPath = "DiceSheet";
    private Dictionary<string, Sprite> spriteMap;
    
    [Header("주사위 애니메이터")]
    [SerializeField] private Animator animator;

    public Sprite Sprite { get; private set; }
    private void Awake()
    {
        LoadspriteMap();
    }
    private void LoadspriteMap()
    {
        spriteMap = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>(spriteSheetPath);

        foreach (var sprite in sprites)
        {
            spriteMap[sprite.name] = sprite;
        }
    }
    public void SetSprite(DiceColor color, int value)
    {
        string key = $"Dice{color}_{value}";
        if (spriteMap.TryGetValue(key, out var sprite))
        {
            diceImage.sprite = sprite;
            diceImage.color = Color.white;
            Sprite = sprite;
        }

        else
        {
            Debug.LogWarning($"해당 키값의 스프라이트 없음"+key);
            Sprite = null;
        }
    }
    public void PlayRollAnimation(DiceColorType color)
    {
        string animName = color switch
        {
            DiceColorType.Red => "DiceRoll_Red",
            DiceColorType.Blue => "DiceRoll_Blue",
            DiceColorType.Yellow => "DiceRoll_Yello",
            DiceColorType.Black => "DiceRoll_Black",
            _ => "DiceRoll_Black"
        };

        animator.Play(animName, 0, 0f); // 애니메이션 재시작
    }
}