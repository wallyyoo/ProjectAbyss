using UnityEngine;

[CreateAssetMenu(menuName = "Character/Player")]
public class PlayerData : BaseCharacterData
{
    [Header("재화 초기 값")]
    [SerializeField] private int startingGold;

    public int StartingGold => startingGold;
}
