using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceView : MonoBehaviour // 주사위ui 전체를 관리
{
    [Header("주사위 UI")] 
    public TMP_Text[] diceTexts;
    public Image[] diceColorImages;

    [Header("버튼 UI")] 
    public GameObject rollStartButton; //현재 비활성화
    public GameObject submitButton;

    [Header("UI 텍스트")] 
    public TMP_Text handInfoText;
    public TMP_Text submitButtonText;
    public TMP_Text rerollButtonText;

    [Header("팝업 오브젝트")] // 주사위를 굴릴수 있는지 경고 팝업
    public GameObject popupObject;
    public TMP_Text popupText;

    [Header("점수 연출 전용 컨트롤러")]
    public ScoreEffectController scoreEffectController;
    
    public DiceSpriteController[] diceSpriteController;
    
    private bool popupManualCloseEnabled = false; // 팝업창 

    public void UpdateDiceDisplay(DiceHandModel model) // 주사위 값, 색상 갱신
    {
        for (int i = 0; i < model.DiceList.Count; i++)
        {
            diceTexts[i].text = model.DiceList[i].Value.ToString(); // 주사위 숫자 출력
           // diceColorImages[i].color = GetColor(model.DiceList[i].Color); // 색상 출력(비사용)
            
            diceSpriteController[i].SetSprite(model.DiceList[i].Color, model.DiceList[i].Value);
            
        }
    }

   public void UpdateHandInfo(HandInfo info, bool isFinal = false) // 족보 및 점수 미리보기 업데이트
   {
       if (info != null)
       {
           handInfoText.text = isFinal ? $"{info.name} (제출됨)" : info.name;
           scoreEffectController.PreviewHand(info.name, info.baseScore, info.multiplier);
           
           UpdateSubmitButtonText(info.description);
       }
       else
       {
           handInfoText.text = "족보 없음";
           scoreEffectController.ClearPreview();
           
           UpdateSubmitButtonText("족보 없음");
       }
   }
    public void UpdateRerollCount(int remaining) // 리롤 횟수 ui 갱신
    {
        rerollButtonText.text = remaining.ToString();
    }
    public void ShowPopup(string message, bool allowManualClose = true)
    {
        popupText.text = message;
        popupObject.SetActive(true);
        popupManualCloseEnabled = allowManualClose;

        CancelInvoke();
        Invoke("HidePopup", 1.5f); // 자동 사라짐
    }

    private void Update()
    {
        if (popupManualCloseEnabled && popupObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0)) // 마우스 클릭
            {
                HidePopup();
            }
        }
    }

    public void HidePopup()
    {
        popupObject.SetActive(false);
    }
    
    

  //  public void SetRollButtonActive(bool active)
  //  {
  //      rollStartButton.SetActive(active);
  //  } 처음부터 주사위가 돌아가있어야 하기 때문에 현재는 비활성화

  //  public void SetSubmitButtonActive(bool active)
  //  {
  //      submitButton.SetActive(active);
  //  } 제출 버튼은 이제 상시로 활성화
    public void UpdateSubmitButtonText(string description) // 족보의 설명을 보여주는 텍스트 최신화
    {
        submitButtonText.text = description;
    }
    
    public void SetSubmitButtonInteractable(bool isOn)
    {
        Button btn = submitButton.GetComponent<Button>();
        if (btn != null)
        {
            btn.interactable = isOn;
   
        }
    }

    public void ClearUI() // ui초기화
    {
        handInfoText.text = "주사위를 굴리세요";
        rerollButtonText.text = "3";
        popupObject.SetActive(false);
        scoreEffectController.ClearPreview();
        
        //SetRollButtonActive(true);        처음부터 주사위가 돌아가있어야 하기 때문에 현재는 비활성화
        //SetSubmitButtonActive(false);
    }

//  private Color GetColor(DiceColor color) // DiceColor enum에 따른 UnityEngine.Color 반환 이 부분은 추후 에셋 이미지로 변경
//  {
//      return color switch
//      {
//          DiceColor.Red => Color.red,
//          DiceColor.Blue => Color.blue,
//          DiceColor.Yello => Color.yellow,
//          DiceColor.Black => Color.black,
//          _ => Color.gray
//      };
//  }
}

