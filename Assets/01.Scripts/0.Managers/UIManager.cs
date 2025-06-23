using System;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI goldText;

    public void UpdateGold(int currentGold)
    {
        goldText.text = $"{currentGold:N0}";
    }
}