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
    
    [Header("점수 연출 전용 컨트롤러")]
    public ScoreEffectController scoreEffectController;
    
    private void Start()
    {
        InitializeDice();
        RegisterAllButtonEvents();
       
        rollButton.onClick.AddListener(RollDice);
        playButton.onClick.AddListener(Play);
        
        UpdateUI();
    }

    private void RegisterAllButtonEvents() // 버튼 클릭 시 주사위 이동
    {
        for (int i = 0; i < rolledDiceButtons.Length; i++) 
        {
            int index = i;
            rolledDiceButtons[i].onClick.AddListener(() => MoveToSelected(index));// 하단 >> 상단 이동
        }

        for (int i = 0; i < selectedDiceButtons.Length; i++) 
        {
            int index = i;
            selectedDiceButtons[i].onClick.AddListener(() => RemoveFromSelected(index));// 상단 >> 하단 이동
        }
    }

    private void UpdateHandPreview()
    {
        if (selectedIndices.Count >= 1)
        {
            var selectedValue = selectedIndices.Select(i => rolledDice[i].value).ToList();
            HandType previewHand = HandEvaluator.Evaluate(selectedValue);
            HandInfo previewInfo = HandDatabase.table[previewHand];

            scoreEffectController.PreviewHand(previewInfo.name, previewInfo.baseScore);
        }
        else
        {
            scoreEffectController.ClearPreview();
        }
    }
    public void InitializeDice() //주사위 초기화
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
        List<int> values = selectedDice.Select(d => d.value).ToList();

        HandType hand = HandEvaluator.Evaluate(values);
        HandInfo info = HandDatabase.table[hand];

        int baseScore = info.baseScore;
        int multiplier = info.multiplier;
        /* 
         추후 데미지 계산식에 효과를 넣으려면
         */
        int finalScore = info.baseScore * info.multiplier;

        Debug.Log($"족보: {info.name}, 점수: {baseScore} * {multiplier} = {finalScore}");

        scoreEffectController.PlayScoreEffect(info.name, baseScore, multiplier, finalScore );

    }

    public void UpdateUI()
    {
        for (int i = 0; i < rolledDice.Count; i++)
        {
            rolledDiceTexts[i].text = rolledDice[i].value.ToString();
            rolledDiceTexts[i].color = selectedIndices.Contains(i) ? Color.gray : Color.white;
            
            //rolledDiceImages[i].color = rolledDice[i].color == DiceColor.Black ? Color.blue : Color.white;
        }

        for (int i = 0; i < selectedDiceTexts.Length; i++)
        {
            if (i < selectedIndices.Count)
            {
                int sourceIndex = selectedIndices[i];
                selectedDiceTexts[i].text = rolledDice[sourceIndex].value.ToString();
                selectedDiceTexts[i].color = Color.white;
               // selectedDiceImages[i].color = rolledDice[sourceIndex].color == DiceColor.Black ? Color.black : Color.white;
            }
            else
            {
                selectedDiceTexts[i].text = "";
                selectedDiceImages[i].color = new Color(0, 0, 0, 0); // 투명
            }
        }

        UpdateHandPreview();

    }
}