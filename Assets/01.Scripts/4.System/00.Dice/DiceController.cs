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
    public Image[] diceColorImages;


    [Header("버튼 UI")] 
    public GameObject rollStartButton;
    public GameObject battleStartButton;

    [Header("UI 텍스트")] 
    public TMP_Text handInfoText;
    public TMP_Text rerollButtonText;

    [Header("팝업 오브젝트")]
    public GameObject popupObject;
    public TMP_Text popupText;

    [Header("점수 연출 전용 컨트롤러")] 
    public ScoreEffectController scoreEffectController;

    [Header("내부 데이터")] 
    private List<DiceData> diceList = new(); // 주사위 값 저장
    private int maxRerolls = 3;     //최대 리롤
    private int currentRerolls = 0;     //현재까지 리롤
    private int finalScore = 0;     //최종점수 계산
    private bool hasRolled = false; //최소굴림 여부
    private bool hasSubmitted = false; //주사위 제출 여부

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
                color = DiceColor.None
            });
        }

        currentRerolls = 0;
        hasRolled = false;
        hasSubmitted = false;

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
            diceList[i].color = (DiceColor)Random.Range(1,5);
        }

        hasRolled = true;
        rollStartButton.SetActive(false);
        battleStartButton.SetActive(true);

        UpdateHandPreview();
        UpdateUI();
    }

    private void Reroll(int index)
    {
        {
            if (!hasRolled)
            {
                ShowPopup("아직 주사위를 굴리지 않았습니다.");
                return;
            }

            if (currentRerolls >= maxRerolls)
            {
                ShowPopup("더 이상 리롤할 수 없습니다.");
                return;
            }

            diceList[index].value = Random.Range(1, 7);
            currentRerolls++;

            rerollButtonText.text = $"{maxRerolls - currentRerolls}";
            UpdateHandPreview();
            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        for (int i = 0; i < Mathf.Min(diceList.Count, diceTexts.Length); i++) //인덱스 초과 방지
        {
            diceTexts[i].text = diceList[i].value.ToString();
            switch (diceList[i].color)
            {
                case DiceColor.Red:    diceColorImages[i].color = Color.red; break;
                case DiceColor.Blue:   diceColorImages[i].color = Color.blue; break;
                case DiceColor.Yellow: diceColorImages[i].color = Color.yellow; break;
                case DiceColor.Black:  diceColorImages[i].color = Color.black; break;
                default:               diceColorImages[i].color = Color.white; break; // None
            }
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
        if (hasSubmitted)
        {
            ShowPopup("이미 주사위를 제출했습니다.");
            return;
        }

        hasSubmitted = true;

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
    
    private void ShowPopup(string message, float duration = 1.5f)
    {
        popupObject.SetActive(true);
        popupText.text = message;
        
        CancelInvoke(nameof(HidePopup));
        Invoke(nameof(HidePopup), duration);
    }

    private void HidePopup()
    {
        popupObject.SetActive(false);
    }

    public void ResetDiceState()
    {
        for (int i = 0; i < diceList.Count; i++)
        {
            diceList[i].value = 1;
            diceList[i].color = DiceColor.None;
        }
        
        currentRerolls = 0;
        hasRolled = false;
        hasSubmitted = false;
        
        currentResult = new HandResult();
        
        rollStartButton.SetActive(true);
        battleStartButton.SetActive(false);
        rerollButtonText.text = $"{maxRerolls}";
        handInfoText.text = "주사위를 굴리세요";
        
        scoreEffectController.ClearPreview();
        UpdateUI();
    }
    public int GetFinalScore() //최종 점수
    {
        return currentResult.FinalScore;
    }

    public HandType GetHandType() // 제출 시 핸드족보
    {
        return currentResult.type;
    }
}