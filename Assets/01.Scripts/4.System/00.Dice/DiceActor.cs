using UnityEngine;
using UnityEngine.UI;
using System.Linq;//
public class DiceActor : MonoBehaviour
{
    [Header("모델, 뷰 UI 연결용")]
    [SerializeField] private DiceView view;

    [Header("주사위 버튼")]
    [SerializeField] private Button[] diceButtons;
    
    [Header("전투 데미지 처리용")]
    [SerializeField] private PlayerDamageCalculator damageCalculator; 
    [SerializeField] private ScoreEffectController scoreEffectController; 
    [SerializeField] private TestPlayer testPlayer;

    private DiceHandModel model = new(); // 주사위 모델 인스턴스
    
    private void Start()
    {
        model = new DiceHandModel();
        model.Init(); // 주사위 리스트 초기화

        view.ClearUI(); // UI 초기화
        view.UpdateDiceDisplay(model); // 주사위 화면 갱신
        view.UpdateRerollCount(model.MaxRerolls - model.CurrentRerolls); // 남은 리롤 수 표시

        view.SetRollButtonActive(true); // 최초 스타트 롤 버튼 활성화
        view.SetSubmitButtonActive(false);  // 제출 버튼 비활성화

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
        view.SetSubmitButtonActive(true);
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
        view.SetSubmitButtonActive(false);
        
        // 디버그: 주사위 색상 출력
        foreach (var die in model.DiceList)
        {
            Debug.Log($"[디버그] 주사위 색상: {die.Color}, 값: {die.Value}");
        }
        HandInfo info = model.Info;
        int baseScore = info.baseScore;
        int multiplier = info.multiplier;

        // 2. 색상 효과 계산
        var colorEffects = DiceColorEffecter.Analyze(model.DiceList);

        // 3. 데미지 계산기 초기화
        damageCalculator.Init(baseScore, multiplier, colorEffects, 0, 1f);

        // 4. 최종 데미지 데이터 출력
        PlayerDamageData result = damageCalculator.GetPlayerDamageData();
        Debug.Log($"[제출 완료] {result}");

        // 5. UI 미리보기 (점수 애니메이션)
        scoreEffectController?.PreviewHand(info.name, baseScore * multiplier, multiplier);

        // 6. 플레이어에게 전달 (테스트)
        testPlayer?.GetDamageFromCalculator(result); 
    }
    
}

