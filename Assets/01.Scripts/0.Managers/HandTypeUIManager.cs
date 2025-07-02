using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HandUIManager : MonoBehaviour
{
    [SerializeField] private List<HandTypeUI> handUIList;

    private Color defaultBgColor = new Color32(173, 185, 202, 255); // 원래 배경색
    private Color maxLevelBgColor = new Color32(249, 218, 119, 255); // 노란색

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (var ui in handUIList)
        {
            if (!HandDatabase.table.TryGetValue(ui.type, out var baseInfo) ||
                !UpgradeDatabase.table.TryGetValue(ui.type, out var upgrade))
            {
                Debug.LogWarning($"HandDatabase 또는 UpgradeDatabase에 {ui.type} 없음");
                continue;
            }

            ApplyBaseInfo(ui, baseInfo, upgrade);
            ApplyLevelVisual(ui, upgrade.level, ui.type);
        }
    }

    private void ApplyBaseInfo(HandTypeUI ui, HandInfo baseInfo, HandTypeUpgradeData upgrade)
    {
        int upgradeScore = baseInfo.baseScore + upgrade.add_score;
        int upgradeMultiplier = baseInfo.multiplier + upgrade.add_multiplier;

        ui.nameText.text = baseInfo.name;
        ui.scoreText.text = upgradeScore.ToString();
        ui.multiplierText.text = upgradeMultiplier.ToString();

        if (ui.manaCountText != null)
            ui.manaCountText.text = upgrade.manaCount.ToString();
    }

    private void ApplyLevelVisual(HandTypeUI ui, int currentLevel, HandType type)
    {
        int maxLevel = DiceTableDatabase.GetMaxLevel(type);

        bool isMax = currentLevel >= maxLevel;

        // 레벨 텍스트
        ui.levelText.text = isMax ? "Max" : $"Lv.{currentLevel}";

        // 배경 색상 처리
        var bgImage = ui.levelText.transform.parent.GetComponent<Image>();
        if (bgImage != null)
            bgImage.color = isMax ? maxLevelBgColor : defaultBgColor;

        // CanvasGroup 알파 값 변경
        if (ui.manaCountText != null)
        {
            var group = ui.manaCountText.transform.parent.GetComponent<CanvasGroup>();
            if (group != null)
                group.alpha = isMax ? 0.4f : 1f;
        }

        // 수정 소모량 Max 전환
        ui.manaCountText.text = isMax ? "Max" : ui.manaCountText.text;
    }
}