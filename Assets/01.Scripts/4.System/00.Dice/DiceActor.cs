using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
public class DiceActor : MonoBehaviour
{
    [Header("모델, 뷰 UI 연결용")]
    [SerializeField] private DiceView view;

    [Header("주사위 버튼")]
    [SerializeField] private Button[] diceButtons;
    
    [Header("전투 데미지 처리용")]
    [SerializeField] private PlayerDamageCalculator damageCalculator; 
    [SerializeField] private ScoreEffectController scoreEffectController; 
    
    [Header("주사위 스프라이트 출력용")]
    [SerializeField] private DiceSpriteController[] diceSpriteControllers;
    
    [Header("DoTween 애니메이션 전용 컨트롤러")]
    [SerializeField] private DiceRollAnimation[] diceRollAnimators;

    private DiceHandModel handModel = new(); // 주사위 모델 인스턴스

    private void Awake()
    {
        TurnManager.Instance.ResiterDiceActor(this);
        TurnManager.Instance.SetTurnPhase(TurnPhase.Ready); 
    }
    public void StartTurn() // 플레이어 턴 시작 
    {
        
        int rerollBouns = TurnManager.Instance.GetExtraRerollBonusValue(); //추가 리롤 값 확인
        
        handModel.Init(); // 주사위 리스트 초기화
        handModel.Evaluate(); // 족보 바로 계산

        view.ClearUI(); // UI 초기화
        view.UpdateDiceDisplay(handModel); // 주사위 화면 갱신
        view.UpdateRerollCount(handModel.MaxRerolls - handModel.CurrentRerolls); // 남은 리롤 수 표시
        
        view.UpdateHandInfo(handModel.Info); //바로 핸드 족보 판별
       
        if (handModel.Info != null)
        {
            view.UpdateSubmitButtonText(handModel.Info.description); // 족보 설명 부분
        }

        view.ShowHandBorders(handModel.Result.Indices); //족보 주사위 효과

        // 각 버튼에 리롤 리스너 등록
        foreach (var (btn, index) in diceButtons.Select((b, i) => (b, i)))
        {
            int capturedIndex = index;
            btn.onClick.AddListener(() => OnClickReroll(capturedIndex));
        }
        view.SetSubmitButtonInteractable(true);
        
    }

   public void OnClickRollAll()
   {
      // model.RollAll();    //모든 주사위 굴림
      handModel.Evaluate();   //족보 계산

       view.UpdateDiceDisplay(handModel); // ui 갱신
       view.UpdateHandInfo(handModel.Info);    // 족보 정보 갱신
       UpdateAllDiceSprites();
    //   view.SetRollButtonActive(false);    // 모든 주사위 굴림 비활성화
    //   view.SetSubmitButtonActive(true);   // 제출버튼 활성화
    
    view.UpdateSubmitButtonText(handModel.Info.description);
   }

    public void OnClickReroll(int index)
    {
        if (diceRollAnimators[index].IsRolling)
            return;
        
        if (!handModel.HasSubmitted && handModel.CurrentRerolls < handModel.MaxRerolls)
        {
            handModel.Reroll(index); // 선택한 주사위만 리롤
            handModel.Evaluate();   // 리롤한 걸 다시 족보 계산
            
            DiceModel dice = handModel.DiceList[index];
            
            diceButtons[index].interactable = false;// 리롤 버튼 비활성화
            
            // DoTween 기반 애니메이션 실행
            diceRollAnimators[index].PlayRollAnimation(dice.Color, dice.Value, () =>
            {
                diceButtons[index].interactable = true; // 리롤 버튼 활성화
            });

            // UI 나머지는 즉시 갱신 가능
            view.UpdateHandInfo(handModel.Info);
            view.UpdateRerollCount(handModel.MaxRerolls - handModel.CurrentRerolls);
            
            view.ShowHandBorders(handModel.Result.Indices);//족보 주사위 효과
        }
        else if (handModel.CurrentRerolls >= handModel.MaxRerolls)
        {
            view.ShowPopup("더 이상 리롤할 수 없습니다.");
        }
        else if (handModel.HasSubmitted)
        {
            view.ShowPopup("이미 제출되어 리롤이 불가능 합니다.");
        }
    }

    public void OnClickSubmit()
    {
        if (handModel.HasSubmitted)
        {
            view.ShowPopup("이미 주사위를 제출했습니다.",true);
            return;
        }

        
        if (handModel.Info == null || handModel.Result == null)
        {
            Debug.LogError("[OnClickSubmit] Info 또는 Result가 null입니다.");
            return;
        }

        if (damageCalculator == null)
        {
            Debug.LogError("[OnClickSubmit] damageCalculator가 null입니다. 인스펙터 연결 확인 요망.");
            return;
        }
        
        handModel.Submit(); // 제출됨 
        view.UpdateHandInfo(handModel.Info,true);
        view.SetSubmitButtonInteractable(false);
        view.ShowPopup(" 주사위가 제출되었습니다.",true);

        Debug.Log($"DiceList Count: {handModel.DiceList?.Count ?? -1}");
        Debug.Log($"HandInfo: {handModel.Info}");
        Debug.Log($"HandResult: {handModel.Result}");
        
        // 색상 효과 계산
        var colorEffects = DiceColorEffecter.Analyze(handModel.DiceList);

        // 데미지 계산기 초기화
        damageCalculator.Init(handModel.Info, handModel.Result, colorEffects, 0, 1f);

        
        // 최종 데미지 데이터 출력
        PlayerDamageData result = damageCalculator.GetPlayerDamageData();
        int totalDisplayScore = (result.baseScore + result.bonusScore) * result.multiplier;
        scoreEffectController?.PreviewHand(result.handName, totalDisplayScore, result.multiplier);// UI 미리보기 (점수 애니메이션)
        
        //TurnManager로 전달
        TurnManager.Instance.PlayerGetAttackDamage(result.finalDamage); 
        TurnManager.Instance.GetCounterReduction(result.counterDamageReduction);
        TurnManager.Instance.GetExtraRerollBouns(result.nextTurnExtraReroll);
    }
    
    private void UpdateAllDiceSprites() // 주사위의 실제값을 설정해줌
    {
        for (int i = 0; i < handModel.DiceList.Count; i++)
        {
            DiceModel dice = handModel.DiceList[i];
            diceSpriteControllers[i].SetSprite(dice.Color, dice.Value);
        }
    }
    
}

