using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class DiceActor : MonoBehaviour
{
    [Header("모델, 뷰 UI 연결용")]
    [SerializeField] private DiceView view;

    [Header("주사위 버튼")]
    [SerializeField] private Button[] diceButtons;
    
    [Header("전투 데미지 처리용")]
    [SerializeField] private PlayerDamageCalculator damageCalculator; 
    [SerializeField] private ScoreEffectController scoreEffectController; 
    [SerializeField] private Player testPlayer;
    
    [Header("주사위 스프라이트 출력용")]
    [SerializeField] private DiceSpriteController[] diceSpriteControllers;

    

    private DiceHandModel model = new(); // 주사위 모델 인스턴스
    
    private void Start()
    {
        model.Init(); // 주사위 리스트 초기화
        model.Evaluate(); // 족보 바로 계산

        view.ClearUI(); // UI 초기화
        view.UpdateDiceDisplay(model); // 주사위 화면 갱신
        view.UpdateRerollCount(model.MaxRerolls - model.CurrentRerolls); // 남은 리롤 수 표시
        view.UpdateHandInfo(model.Info); //바로 핸드 족보 판별
       
        if (model.Info != null)
        {
            view.UpdateSubmitButtonText(model.Info.description); // 족보 설명 부분
        }

     //  view.SetRollButtonActive(true); // 최초 스타트 롤 버튼 활성화
     //  view.SetSubmitButtonActive(false);  // 제출 버튼 비활성화

        // 각 버튼에 리롤 리스너 등록
        foreach (var (btn, index) in diceButtons.Select((b, i) => (b, i)))
        {
            int capturedIndex = index;
            btn.onClick.AddListener(() => OnClickReroll(capturedIndex));
        }
        
    }

   public void OnClickRollAll()
   {
      // model.RollAll();    //모든 주사위 굴림
       model.Evaluate();   //족보 계산

       view.UpdateDiceDisplay(model); // ui 갱신
       view.UpdateHandInfo(model.Info);    // 족보 정보 갱신
       UpdateAllDiceSprites();
    //   view.SetRollButtonActive(false);    // 모든 주사위 굴림 비활성화
    //   view.SetSubmitButtonActive(true);   // 제출버튼 활성화
    
    view.UpdateSubmitButtonText(model.Info.description);
   }

    public void OnClickReroll(int index)
    {
        if (!model.HasSubmitted && model.CurrentRerolls < model.MaxRerolls)
        {
            model.Reroll(index); // 선택한 주사위만 리롤
            model.Evaluate();   // 리롤한 걸 다시 족보 계산
            
            diceSpriteControllers[index].SetSprite(model.DiceList[index].Color, model.DiceList[index].Value); //이미지

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
            view.ShowPopup("이미 제출되어 리롤이 불가능 합니다.");
        }
    }

    public void OnClickSubmit()
    {
        if (model.HasSubmitted)
        {
            view.ShowPopup("이미 주사위를 제출했습니다.",true);
            return;
        }

        model.Submit(); // 제출됨 
        view.UpdateHandInfo(model.Info,true);
        view.SetSubmitButtonInteractable(false);
        view.ShowPopup(" 주사위가 제출되었습니다.",true);

        // 디버그깅용 주사위 색상 출력 
        foreach (var die in model.DiceList)
        {
            Debug.Log($"[디버그] 주사위 색상: {die.Color}, 값: {die.Value}");
        }
        HandInfo info = model.Info;
        int baseScore = info.baseScore;
        int multiplier = info.multiplier;

        // 색상 효과 계산
        var colorEffects = DiceColorEffecter.Analyze(model.DiceList);

        // 데미지 계산기 초기화
        damageCalculator.Init(model.Info, model.Result, colorEffects, 0, 1f);

        // 최종 데미지 데이터 출력
        PlayerDamageData result = damageCalculator.GetPlayerDamageData();
        int totalDisplayScore = (result.baseScore + result.bonusScore) * result.multiplier;
        scoreEffectController?.PreviewHand(result.handName, totalDisplayScore, result.multiplier);// UI 미리보기 (점수 애니메이션)
        
        Debug.Log($"[제출 완료] {result}");

        TurnManager.Instance.GetCounterReduction(result.counterDamageReduction);

  

        //  플레이어에게 전달 (테스트)
        // testPlayer?.GetDamageFromCalculator(result); 
        TurnManager.Instance.PlayerGetAttackDamage(result.finalDamage);
    }
    
    private void UpdateAllDiceSprites()
    {
        for (int i = 0; i < model.DiceList.Count; i++)
        {
            DiceModel die = model.DiceList[i];
            diceSpriteControllers[i].SetSprite(die.Color, die.Value);
        }
    }
    
}

