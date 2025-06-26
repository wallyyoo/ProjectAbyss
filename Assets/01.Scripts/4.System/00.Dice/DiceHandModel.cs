using System.Collections.Generic;
using System.Linq;

public class DiceHandModel  // 5개의 주사위 굴림, 족보 판정, 색상조합 확인
{
    public List<DiceModel> DiceList { get; private set; } = new(); //주사위 리스트
    public HandType Type { get; private set; }  //족보 종류
    public HandInfo Info { get; private set; }  //족보 정보
    public int FinalScore => Info != null ? Info.baseScore * Info.multiplier : 0; // 최종점수 계산

    public int MaxRerolls { get; private set; } = 3;
    public int CurrentRerolls { get; private set; } = 0;
    public bool HasSubmitted { get; private set; } = false;

    public void Init() // 시작시 주사위 5개 초기화
    {
        DiceList.Clear();
        for (int i = 0; i < 5; i++)
        {
            var model = new DiceModel();
            model.Init();
            DiceList.Add(model);
        }

        CurrentRerolls = 0;
        HasSubmitted = false;
        Type = HandType.None;
        Info = null;
    }

    public void RollAll()
    {
        foreach (var die in DiceList)
            die.Roll(false); //색상은 유지하고 값만 변경
    }

    public void Reroll(int index) // 특정 인덱스의 주사위만 리롤
    {
        if (index < 0 || index >= DiceList.Count || CurrentRerolls >= MaxRerolls || HasSubmitted)
            return;

        DiceList[index].Roll();
        CurrentRerolls++;
    }

    public void Evaluate()  // 현재 주사위값으로 족보 판정
    {
        var values = DiceList.Select(d => d.Value).ToList();
        Type = HandEvaluator.Evaluate(values);  //족보 판정
        Info = HandDatabase.table[Type];        //족보 점수, 정보 
    }

    public void Submit()    // 주사위 제출
    {
        if (!HasSubmitted)
        {
            Evaluate();     // 제출 시점 족보를 다시 계산
            HasSubmitted = true;
        }
    }
} 
