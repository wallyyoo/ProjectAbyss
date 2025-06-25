using System.Collections.Generic;
using System.Linq;

public class DiceHandModel  // 주사위 굴림, 족보 판정, 색상조합 확인
{
    public List<DiceModel> DiceList { get; private set; } = new();
    public HandType Type { get; private set; }
    public HandInfo Info { get; private set; }
    public int FinalScore => Info != null ? Info.baseScore * Info.multiplier : 0;

    public int MaxRerolls { get; private set; } = 3;
    public int CurrentRerolls { get; private set; } = 0;
    public bool HasSubmitted { get; private set; } = false;

    public void Init()
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
            die.Roll(false);
    }

    public void Reroll(int index)
    {
        if (index < 0 || index >= DiceList.Count || CurrentRerolls >= MaxRerolls || HasSubmitted)
            return;

        DiceList[index].Roll();
        CurrentRerolls++;
    }

    public void Evaluate()
    {
        var values = DiceList.Select(d => d.Value).ToList();
        Type = HandEvaluator.Evaluate(values);
        Info = HandDatabase.table[Type];
    }

    public void Submit()
    {
        if (!HasSubmitted)
        {
            Evaluate();
            HasSubmitted = true;
        }
    }
} 
