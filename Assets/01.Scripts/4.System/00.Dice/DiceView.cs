using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceView : MonoBehaviour
{
    [Header("주사위 UI")] public Button[] diceButtons;
    public TMP_Text[] diceTexts;
    public Image[] diceColorImages;

    [Header("버튼 UI")] public GameObject rollStartButton;
    public GameObject battleStartButton;

    [Header("UI 텍스트")] public TMP_Text handInfoText;
    public TMP_Text rerollButtonText;

    [Header("팝업 오브젝트")] public GameObject popupObject;
    public TMP_Text popupText;

    [Header("점수 연출 전용 컨트롤러")] public ScoreEffectController scoreEffectController;

    public void UpdateDiceDisplay(DiceHandModel model)
    {
        for (int i = 0; i < model.DiceList.Count; i++)
        {
            diceTexts[i].text = model.DiceList[i].Value.ToString();
            diceColorImages[i].color = GetColor(model.DiceList[i].Color);
        }
    }

    public void UpdateHandInfo(HandInfo info, bool isFinal = false)
    {
        if (info != null)
        {
            handInfoText.text = isFinal ? $"{info.name} (제출됨)" : info.name;
            scoreEffectController.PreviewHand(info.name, info.baseScore, info.multiplier);
        }
        else
        {
            handInfoText.text = "족보 없음";
            scoreEffectController.ClearPreview();
        }
    }

    public void UpdateRerollCount(int remaining)
    {
        rerollButtonText.text = remaining.ToString();
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupObject.SetActive(true);
        CancelInvoke();
        Invoke("HidePopup", 1.5f);
    }

    public void HidePopup()
    {
        popupObject.SetActive(false);
    }

    public void SetRollButtonActive(bool active)
    {
        rollStartButton.SetActive(active);
    }

    public void SetBattleButtonActive(bool active)
    {
        battleStartButton.SetActive(active);
    }

    public void ClearUI()
    {
        handInfoText.text = "주사위를 굴리세요";
        rerollButtonText.text = "3";
        popupObject.SetActive(false);
        scoreEffectController.ClearPreview();
    }

    private Color GetColor(DiceColor color)
    {
        return color switch
        {
            DiceColor.Red => Color.red,
            DiceColor.Blue => Color.blue,
            DiceColor.Yellow => Color.yellow,
            DiceColor.Black => Color.black,
            _ => Color.white
        };
    }
}

