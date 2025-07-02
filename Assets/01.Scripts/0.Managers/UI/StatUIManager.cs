using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUIManager : MonoBehaviour
{
    [SerializeField] private List<StatTypeUI> statUIList;

    private Color defaultBgColor = new Color32(173, 185, 202, 255);
    private Color maxLevelBgColor = new Color32(249, 218, 119, 255);

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (var ui in statUIList)
        {
            int currentLevel = PlayerProgressManager.Instance.GetStatUpgradeLevel(ui.type);
            var currentData = StatUpgradeDatabase.table.GetValueOrDefault(ui.type);
            var nextData = StatTableDatabase.GetUpgradeData(ui.type, currentLevel + 1);

            if (currentData == null)
            {
                Debug.LogWarning($"StatUpgradeDatabase에 {ui.type} 데이터 없음");
                continue;
            }

            int nextLevel = currentLevel + 1;
            var nextUpgrade = StatTableDatabase.GetUpgradeData(ui.type, nextLevel);

            ApplyBaseInfo(ui, currentData, nextData, nextUpgrade);
            ApplyLevelVisual(ui, currentLevel, ui.type);
        }
    }

    private void ApplyBaseInfo(StatTypeUI ui, BaseStatTableData current, BaseStatTableData next, BaseStatTableData nextUpgrade)
    {
        var stats = current.stats.ToString();
        ui.statText.text = $"+ {stats}";

        if (ui.manaCountText != null)
        {
            if (nextUpgrade != null)
                ui.manaCountText.text = nextUpgrade.manaCount.ToString();
            else
                ui.manaCountText.text = "Max";
        }
    }

    private void ApplyLevelVisual(StatTypeUI ui, int currentLevel, PlayerStatType type)
    {
        int maxLevel = StatTableDatabase.GetMaxLevel(type);
        bool isMax = currentLevel >= maxLevel;

        ui.levelText.text = isMax ? "Max" : $"Lv.{currentLevel}";

        var bgImage = ui.levelText.transform.parent.GetComponent<Image>();
        if (bgImage != null)
            bgImage.color = isMax ? maxLevelBgColor : defaultBgColor;

        if (ui.manaCountText != null)
        {
            var group = ui.manaCountText.transform.parent.GetComponent<CanvasGroup>();
            if (group != null)
                group.alpha = isMax ? 0.4f : 1f;
        }
    }
}


