using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DiceController : MonoBehaviour
{
    [Header("주사위 UI")] 
    public Button[] diceButtons;
    public TMP_Text[] diceTexts;

    [Header("버튼 UI")] 
    public GameObject rollStartButton;
    public GameObject battleStartButton;

    [Header("UI 텍스트")] 
    public TMP_Text handInfoText;
    public TMP_Text rerollButtonText;
    public TMP_Text diceNoticeText;

    [Header("점수 연출 전용 컨트롤러")] 
    public ScoreEffectController scoreEffectController;

    [Header("내부 데이터")] 
    private List<DiceData> diceList = new(); // 주사위 값 저장
    private int maxRerolls = 3;     //최대 리롤
    private int currentRerolls = 0;     //현재까지 리롤
    private int finalScore = 0;     //최종점수 계산
    private bool hasRolled = false; //최소굴림 여부

    private HandResult currentResult = new();
    

    private void Start()
    {
        Init();
        
        rollStartButton.GetComponent<Button>().onClick.AddListener(FirstRollAllDice);
        battleStartButton.GetComponent<Button>().onClick.AddListener(FinalizeHand);

        for (int i = 0; i < diceButtons.Length; i++)
        {
            int index = i;
            diceButtons[i].onClick.AddListener(() => Reroll(index));    //클릭시 리롤
        }
    }

    private void Init() //주사위데이터 초기화 
    {
        diceList.Clear();
        for (int i = 0; i < 5; i++)
        {
            diceList.Add(new DiceData
            {
                value = 1,
                color = DiceColor.Black   // 향후 다른 색 표기
            });
        }

        currentRerolls = 0;
        hasRolled = false;

        rollStartButton.SetActive(true);
        battleStartButton.SetActive(false);
        rerollButtonText.text = $"{maxRerolls}";
        handInfoText.text = "주사위를 굴리세요";
        
        currentResult= new HandResult();
        scoreEffectController.ClearPreview();
        UpdateUI();
    }

    private void FirstRollAllDice() //처음 주사위 굴림
    {
        for (int i = 0; i < diceList.Count; i++)
        {
            diceList[i].value = Random.Range(1, 7);
        }

        hasRolled = true;
        rollStartButton.SetActive(false);
        battleStartButton.SetActive(true);

        UpdateHandPreview();
        UpdateUI();
    }

    private void Reroll(int index)
    {
        if (!hasRolled || currentRerolls >= maxRerolls) return;

        diceList[index].value = Random.Range(1, 7);
        currentRerolls++;

        rerollButtonText.text = $"{maxRerolls - currentRerolls}";
        UpdateHandPreview();
        UpdateUI();

        if (currentRerolls < 0)
        {
            
        }
        
    }
    private void UpdateUI()
    {
        for (int i = 0; i < Mathf.Min(diceList.Count, diceTexts.Length); i++) //인덱스 초과 방지
        {
            diceTexts[i].text = diceList[i].value.ToString();
            // 향후 색상 표현 가능
        }
    }

    private void UpdateHandPreview() // 핸드의 족보UI
    {
        var values = diceList.Select(d => d.value).ToList();
        var handType = HandEvaluator.Evaluate(values);
        var info = HandDatabase.table[handType];

        finalScore = info.baseScore * info.multiplier;
        handInfoText.text = $"{info.name}";
        
        scoreEffectController.PreviewHand(info.name, info.baseScore, info.multiplier);
    }
    
    private void FinalizeHand()
    {
        // 현재 주사위 값에서 족보 평가
        var values = diceList.Select(d => d.value).ToList();
        var handType = HandEvaluator.Evaluate(values);
        var info = HandDatabase.table[handType];

        currentResult.type = handType;
        currentResult.baseHandTypeScore = info.baseScore;
        currentResult.multiplier = info.multiplier;

        // 리롤 애니메이션
        scoreEffectController.FinalizePreviewLock();

        rollStartButton.SetActive(false);
        battleStartButton.SetActive(true);
    }

    public int GetFinalScore()
    {
        return currentResult.FinalScore;
    }

    public HandType GetHandType()
    {
        return currentResult.type;
    }

}