using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.Overlays;

public class UIManager : Singleton<UIManager>
{
    // ==== 재화 (골드, 수정) ====
    [UIBind("TopBar/UI_Currency/Gold/GoldText")] private TextMeshProUGUI goldText;
    [UIBind("TopBar/UI_Currency/EnchantCore/EnchantCoreText")] private TextMeshProUGUI enchantCoreText;
    [UIBind("TopBar/UI_Currency")] private RectTransform currencyPanel;

    // ==== 제한 ====
    [UIBind("TopBar/Constraint/Back/Image/ConstText")] private TextMeshProUGUI constText;
    [UIBind("TopBar/Constraint/ConstButton")] private Button constButton;
    [UIBind("TopBar/Constraint/Back")] private GameObject constPanel;

    // ==== 코덱스 ====
    [UIBind("TopBar/Codex")] private Button codexButton;
    [UIBind("CodexPanel")] private GameObject codexPanel;
    [UIBind("CodexPanel/CodexCloseButton")] private Button codexCloseButton;

    // ==== 미궁 층 ====
    [UIBind("TopBar/Floor/FloorText")] private TextMeshProUGUI floorText;

    // ==== 메뉴 ====
    [UIBind("TopBar/Menu")] private Button menuButton;
    [UIBind("MenuPanel")] GameObject menuPanel;
    [UIBind("MenuPanel/MenuClose")] private Button menuCloseButton;


    protected override void Awake()
    {
        base.Awake();
        UIAutoBinder.BindUI(this, transform);

        constButton.onClick.AddListener(ToggleConstPanel);
        codexButton.onClick.AddListener(OpenCodex);
        menuButton.onClick.AddListener(OpenMenu);

        menuCloseButton.onClick.AddListener(CloseMenu);
        codexCloseButton.onClick.AddListener(CloseCodex);
    }

    /// <summary>
    /// 몬스터 강화 or 플레이어 제약사항 열고 닫기
    /// </summary>
    public void ToggleConstPanel()
    {
        constPanel.SetActive(!constPanel.activeSelf);
    }

    /// <summary>
    /// 메뉴 열기
    /// </summary>
    public void OpenMenu()
    {
        menuPanel.transform.SetAsLastSibling();
        menuPanel.SetActive(true);
    }

    /// <summary>
    /// 메뉴닫기
    /// </summary>
    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }

    public void OpenCodex()
    {
        codexPanel.SetActive(true);
    }

    public void CloseCodex()
    {
        codexPanel.SetActive(false);
    }

    public void UpdateConstraint()
    {
        constText.text = $""; // 몬스터 강화 또는 제한 사항 확인하는 변수 추가
    }

    public void UpdateFloor()
    {
        floorText.text = $""; // 층 정보 저장하거나 설정하는 변수 추가
    }

    public void UpdateGold(int currentGold)
    {
        goldText.text = $"{currentGold:N0}";
        StartCoroutine(RebuildAfterDelay());
    }

    public void UpdateEnchantCore(int currentEchantCore)
    {
        enchantCoreText.text = $"{currentEchantCore:N0}";
    }

    /// <summary>
    /// 재화 부분 갱신용
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(currencyPanel);
    }

    /// <summary>
    /// 골드 리빌드 용
    /// </summary>
    /// <returns></returns>
    private IEnumerator RebuildAfterDelay()
    {
        yield return null;
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(currencyPanel);
    }
}