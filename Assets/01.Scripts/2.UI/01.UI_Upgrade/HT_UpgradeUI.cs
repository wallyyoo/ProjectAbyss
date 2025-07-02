using UnityEngine;
using UnityEngine.UI;
using System;

public class HT_UpgradeUI : MonoBehaviour
{
    private void Awake()
    {
        SetupUpgradeButtons();
    }

    private void SetupUpgradeButtons()
    {
        foreach (Transform group in transform)
        {
            HandType type = ParseHandTypeFromName(group.name);
            if (type == HandType.None) continue;

            Button up = group.Find("HT_up")?.GetComponent<Button>();
            Button down = group.Find("HT_down")?.GetComponent<Button>();

            if (up) up.onClick.AddListener(() =>
            {
                UpgradeManager.TryUpgrade(type,
                onSuccess: () => UIManager.Instance.UpgradeHandUI.Refresh(),
                onFail: () => Debug.Log("업그레이드 실패"));
            });
            if (down) down.onClick.AddListener(() =>
            {
                UpgradeManager.TryDowngrade(type,
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
