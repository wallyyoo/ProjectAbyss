using UnityEngine;
using UnityEngine.UI;
using System;

public class StatUpgradeUI : MonoBehaviour
{
    private void Awake()
    {
        SetupStatUpgradeButtons();
    }

    private void SetupStatUpgradeButtons()
    {
        foreach (Transform group in transform)
        {
            PlayerStatType type = ParseStatTypeFromName(group.name);
            if (type == PlayerStatType.None) continue;

            Button up = group.Find("ST_up")?.GetComponent<Button>();
            Button down = group.Find("ST_down")?.GetComponent<Button>();

            if (up) up.onClick.AddListener(() =>
            {
                StatUpgradeManager.TryUpgrade(type,
                    onSuccess: () => UIManager.Instance.StatUIManager.Refresh(),
                    onFail: () => Debug.Log("스탯 업그레이드 실패"));
            });

            if (down) down.onClick.AddListener(() =>
            {
                StatUpgradeManager.TryDowngrade(type,
                    onSuccess: () => UIManager.Instance.StatUIManager.Refresh(),
                    onFail: () => Debug.Log("스탯 다운그레이드 실패"));
            });
        }
    }

    private PlayerStatType ParseStatTypeFromName(string name)
    {
        foreach (PlayerStatType type in Enum.GetValues(typeof(PlayerStatType)))
        {
            if (name.Equals(type.ToString(), StringComparison.OrdinalIgnoreCase))
                return type;
        }
        return PlayerStatType.None;
    }
}

