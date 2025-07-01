using UnityEngine;
using System.Collections.Generic;

public class HandUIManager : MonoBehaviour
{
    [SerializeField] private List<HandTypeUI> handUIList;

    private void Start()
    {
        foreach (var ui in handUIList)
        {
            if (HandDatabase.table.TryGetValue(ui.type, out var info))
            {
                ui.nameText.text = info.name; // 족보 이름
                ui.scoreText.text = info.baseScore.ToString(); // 베이스 스코어
                ui.multiplierText.text = $"x{info.multiplier}"; // 배율
            }
            else
            {
                Debug.Log($"핸드타입 {ui.type} 정보가 HandDatabase에 없음");
            }
        }
    }
}
