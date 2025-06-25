using UnityEngine;
using UnityEngine.UI;
using System.Linq;//
public class DiceActor : MonoBehaviour
{
    [Header("Model & View Reference")]
    [SerializeField] private DiceView view;

    [Header("Dice Buttons")]
    [SerializeField] private Button[] diceButtons;

    private DiceHandModel model = new();
    
    private void Start()
    {
        model = new DiceHandModel();
        model.Init();

        view.ClearUI();
        view.UpdateDiceDisplay(model);
        view.UpdateRerollCount(model.MaxRerolls - model.CurrentRerolls);

        view.SetRollButtonActive(true);
        view.SetBattleButtonActive(false);

        foreach (var (btn, index) in diceButtons.Select((b, i) => (b, i)))
        {
            int capturedIndex = index;
            btn.onClick.AddListener(() => OnClickReroll(capturedIndex));
        }
    }

    public void OnClickRollAll()
    {
        model.RollAll();
        model.Evaluate();

        view.UpdateDiceDisplay(model);
        view.UpdateHandInfo(model.Info);
        view.SetRollButtonActive(false);
        view.SetBattleButtonActive(true);
    }

    public void OnClickReroll(int index)
    {
        if (!model.HasSubmitted && model.CurrentRerolls < model.MaxRerolls)
        {
            model.Reroll(index);
            model.Evaluate();

            view.UpdateDiceDisplay(model);
            view.UpdateHandInfo(model.Info);
            view.UpdateRerollCount(model.MaxRerolls - model.CurrentRerolls);
        }
        else if (model.CurrentRerolls >= model.MaxRerolls)
        {
            view.ShowPopup("더 이상 리롤할 수 없습니다.");
        }
        else if (model.HasSubmitted)
        {
            view.ShowPopup("이미 제출했습니다.");
        }
    }

    public void OnClickSubmit()
    {
        if (model.HasSubmitted)
        {
            view.ShowPopup("이미 주사위를 제출했습니다.");
            return;
        }

        model.Submit();
        view.UpdateHandInfo(model.Info);
        view.SetBattleButtonActive(false);
    }

    public int GetFinalScore() => model.FinalScore;
    public HandType GetHandType() => model.Type;
}

