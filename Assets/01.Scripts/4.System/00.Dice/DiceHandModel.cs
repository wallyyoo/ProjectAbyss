using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceHandModel
{
    public List<DiceModel> DiceList { get; private set; } = new();
    public HandType Type { get; private set; }
    public HandInfo Info { get; private set; }
    public HandResult Result { get; private set; }
    public int FinalScore => Info != null ? Info.baseScore * Info.multiplier : 0;
    public int MaxRerolls { get; private set; } = 3;
    public int CurrentRerolls { get; private set; } = 0;
    public bool HasSubmitted { get; private set; } = false;

    public void Init()
    {
        DiceList.Clear();               // 기존 주사위 리스트 삭제
        for (int i = 0; i < 5; i++)
        {
            var model = new DiceModel();// 새로운 주사위 모델 생성
            model.Init();                 // 초기화
            model.Roll(false);  // 디폴트 주사위 굴린값
            DiceList.Add(model);
        }
        CurrentRerolls = 0;

        int rerollBouns = TurnManager.Instance.GetExtraRerollBonusValue();
        MaxRerolls = 3 + rerollBouns;// 리롤 보너스 적용

        Debug.Log($"리롤보너스 +{rerollBouns} 적용됨 = {MaxRerolls}");

        HasSubmitted = false;
        Type = HandType.None;
        Info = null;
        Result = null;
    }

    public void Evaluate()
    {
        var values = DiceList.Select(d => d.Value).ToList();
        Result = HandEvaluator.Evaluate(values);
        Type = Result.Type;

        // =============== 변경 한 부분 (시작) ===============
        var baseInfo = HandDatabase.table[Type];
        var upgrade = PlayerProgressManager.Instance?.Progress?.GetHandData(Type);
        int addScore = upgrade?.add_score ?? 0;
        int addMultiplier = upgrade?.add_multiplier ?? 0;

        Info = new HandInfo(
            baseInfo.name,
            baseInfo.baseScore + addScore,
            baseInfo.multiplier + addMultiplier,
            baseInfo.description
            );
        // Info = HandDatabase.table[Type];
        // =============== 변경 한 부분 (끝) ===============
    }

    public void Reroll(int index)
    {
        if (index < 0 || index >= DiceList.Count || CurrentRerolls >= MaxRerolls || HasSubmitted)
            return;

        DiceList[index].Roll();
        CurrentRerolls++;
    }

    public void Submit()
    {
        if (!HasSubmitted)
        {
            if (DiceList == null || DiceList.Count == 0)
            {
                Debug.LogError("[DiceHandModel] 주사위가 존재하지 않아 Submit을 할 수 없습니다.");
                return;
            }


            Evaluate();
            HasSubmitted = true;
        }
    }

}
