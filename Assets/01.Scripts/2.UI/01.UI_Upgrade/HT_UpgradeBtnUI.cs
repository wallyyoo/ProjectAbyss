using UnityEngine;
using UnityEngine.UI;
using System;

public class HT_UpgraBtnUI : MonoBehaviour
{
    private void Awake()
    {
        SetupHandTypeUpgradeButtons();
    }

    private void SetupHandTypeUpgradeButtons()
    {
        foreach (Transform group in transform)
        {
            HandType type = ParseHandTypeFromName(group.name);
            if (type == HandType.None) continue;

            Button up = group.Find("HT_up")?.GetComponent<Button>();
            Button down = group.Find("HT_down")?.GetComponent<Button>();

            if (up) up.onClick.AddListener(() =>
            {
                UpgradeHandTypeManager.TryUpgrade(type,
                onSuccess: () => UIManager.Instance.UpgradeHandUI.Refresh(),
                onFail: () => Debug.Log("업그레이드 실패"));
            });
            if (down) down.onClick.AddListener(() =>
            {
                UpgradeHandTypeManager.TryDowngrade(type,
                onSuccess: () => UIManager.Instance.UpgradeHandUI.Refresh(),
                onFail: () => Debug.Log("다운 그레이드 실패"));
            });
        }
    }

    private HandType ParseHandTypeFromName(string name)
    {
        foreach (HandType type in Enum.GetValues(typeof(HandType)))
        {
            if (name.Equals(type.ToString(), StringComparison.OrdinalIgnoreCase))
                return type;
        }
        return HandType.None;
    }
}
