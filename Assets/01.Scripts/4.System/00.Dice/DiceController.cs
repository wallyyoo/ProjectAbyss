using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceController : MonoBehaviour
{
    [Header("1번 배열: 하단 주사위 UI")]
    public Button[] rolledDiceButtons;
    public TMP_Text[] rolledDiceTexts;
    public Image[] rolledDiceImages;

    [Header("2번 배열: 상단 고정된 주사위 UI")]
    public Button[] selectedDiceButtons;
    public TMP_Text[] selectedDiceTexts;
    public Image[] selectedDiceImages;

    [Header("주사위 데이터")]
    public List<DiceData> rolledDice = new List<DiceData>();    // 6개 고정
    public List<int> selectedIndices = new List<int>();         // 1번 배열의 선택된 인덱스
    
    [Header("컨트롤 버튼")]
    public Button rollButton;
    public Button playButton;

    private void Start()
    {
        InitializeDice();
        RegisterAllButtonEvents();
       
        rollButton.onClick.AddListener(RollDice);
        playButton.onClick.AddListener(Play);
        
        UpdateUI();
    }

    private void RegisterAllButtonEvents()
    {
        for (int i = 0; i < rolledDiceButtons.Length; i++)
        {
            int index = i;
            rolledDiceButtons[i].onClick.AddListener(() => MoveToSelected(index));
        }

        for (int i = 0; i < selectedDiceButtons.Length; i++)
        {
            int index = i;
            selectedDiceButtons[i].onClick.AddListener(() => RemoveFromSelected(index));
        }
    }

    public void InitializeDice()
    {
        rolledDice.Clear();
        for (int i = 0; i < 6; i++)
        {
            rolledDice.Add(new DiceData
            {
                value = Random.Range(1, 7),
                color = DiceColor.Black
            });
        }
        selectedIndices.Clear();
        UpdateUI();
    }

    public void MoveToSelected(int index)
    {
        if (!selectedIndices.Contains(index) && selectedIndices.Count < 5)
        {
            selectedIndices.Add(index);
            UpdateUI();
        }
    }

    public void RemoveFromSelected(int selectedSlotIndex)
    {
        if (selectedSlotIndex < selectedIndices.Count)
        {
            selectedIndices.RemoveAt(selectedSlotIndex);
            UpdateUI();
        }
    }

    public void RollDice()
    {
        for (int i = 0; i < rolledDice.Count; i++)
        {
            if (!selectedIndices.Contains(i))
            {
                rolledDice[i].value = Random.Range(1, 7);
            }
        }

        UpdateUI();
    }

    public void Play()
    {
        List<DiceData> selectedDice = selectedIndices.Select(i => rolledDice[i]).ToList();

        Debug.Log("플레이 버튼 클릭 - 선택된 주사위:");
        foreach (var dice in selectedDice)
        {
            Debug.Log($"Value: {dice.value}");
        }

        // 추후 족보 평가 함수 호출 위치
    }

    public void UpdateUI()
    {
        for (int i = 0; i < rolledDice.Count; i++)
        {
            rolledDiceTexts[i].text = rolledDice[i].value.ToString();
            rolledDiceTexts[i].color = selectedIndices.Contains(i) ? Color.gray : Color.white;
            rolledDiceImages[i].color = rolledDice[i].color == DiceColor.Black ? Color.black : Color.white;
        }

        for (int i = 0; i < selectedDiceTexts.Length; i++)
        {
            if (i < selectedIndices.Count)
            {
                int sourceIndex = selectedIndices[i];
                selectedDiceTexts[i].text = rolledDice[sourceIndex].value.ToString();
                selectedDiceTexts[i].color = Color.white;
                selectedDiceImages[i].color = rolledDice[sourceIndex].color == DiceColor.Black ? Color.black : Color.white;
            }
            else
            {
                selectedDiceTexts[i].text = "";
                selectedDiceImages[i].color = new Color(0, 0, 0, 0); // 투명
            }
        }
    }
}