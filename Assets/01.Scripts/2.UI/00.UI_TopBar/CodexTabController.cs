using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CodexTabController : MonoBehaviour
{
    [UIBind("CodexTabs/HandTypeTab")] private Button handTypeTab;
    [UIBind("CodexTabs/DestinyDiceTab")] private Button destinyDiceTab;
    [UIBind("CodexTabs/InventoryTab")] private Button inventoryTab;

    [UIBind("HandTypePanel")] private GameObject handTypePanel;
    [UIBind("DestinyDicePanel")] private GameObject destinyDicePanel;
    [UIBind("InventoryPanel")] private GameObject inventoryPanel;

    private List<Button> tabButtons;
    private List<GameObject> tabPanels;

    private Color SelectedColor = new Color(118f / 255f, 113f / 255f, 113f / 255f);
    private Color normalColor = new Color(64f / 255f, 64f / 255f, 64f / 255f);

    private int currentIndex = -1;

    void Awake()
    {
        UIAutoBinder.BindUI(this, transform);

        tabButtons = new List<Button>() { handTypeTab, destinyDiceTab, inventoryTab };
        tabPanels = new List<GameObject>() { handTypePanel, destinyDicePanel, inventoryPanel };

        for (int i = 0; i < tabButtons.Count; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => SwitchTab(index));
        }
    }

    void OnEnable()
    {
        SwitchTab(0);
    }

    public void SwitchTab(int index)
    {
        if (index == currentIndex) return;

        currentIndex = index;

        for (int i = 0; i < tabPanels.Count; i++)
        {
            tabPanels[i].SetActive(i == index);

            var Image = tabButtons[i].GetComponent<Image>();
            Image.color = (i == index) ? SelectedColor : normalColor;
        }
    }
}
