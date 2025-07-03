using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;

    public void SetHP(int currentHP, int maxHP)
    {
        float fillAmount = maxHP > 0 ? (float)currentHP / maxHP : 0f;
        hpFillImage.fillAmount = fillAmount;
    }
}
