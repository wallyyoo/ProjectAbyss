
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreEffectController : MonoBehaviour // ui 연출에 효과를 주는 컨트롤러
{
    [Header("UI 결과 출력")] 
    public TMP_Text handTypeName;
    public TMP_Text handTypeMulitplier;
    public TMP_Text handTypeScorePreview;   // 실시간 프리뷰용


    public void PreviewHand(string handName, int score, int multiplier) // 족보 이름, 점수, 배율을 받아 애니메이션으로 출력
    {
        handTypeName.text = handName;

        handTypeScorePreview.gameObject.SetActive(true);

        // 점수 애니메이션
        DOTween.Kill(handTypeScorePreview);
        int displayScore = 0;
        handTypeScorePreview.text = "0";
        DOTween.To(() => displayScore, x =>
        {
            displayScore = x;
            handTypeScorePreview.text = displayScore.ToString();
        }, score, 0.4f).SetEase(Ease.OutQuad).SetId(handTypeScorePreview);

        // 배율 애니메이션
        DOTween.Kill(handTypeMulitplier);
        float displayMul = 0;
        handTypeMulitplier.text = "0";
        DOTween.To(() => displayMul, x =>
        {
            displayMul = x;
            handTypeMulitplier.text = $"{Mathf.RoundToInt(displayMul)}";
        }, multiplier, 0.4f).SetEase(Ease.OutQuad).SetId(handTypeMulitplier);
    }
    
    public void ClearPreview() // 모든 텍스트 초기화 및 트윈 제거
    {
        DOTween.Kill(handTypeScorePreview);
        DOTween.Kill(handTypeMulitplier);

        handTypeName.text = "";
        handTypeScorePreview.text = "";
        handTypeMulitplier.text = "";
    }

}
