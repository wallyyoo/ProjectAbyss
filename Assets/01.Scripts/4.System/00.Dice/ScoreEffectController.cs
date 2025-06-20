
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreEffectController : MonoBehaviour
{
    [Header("UI 결과 출력")] public TMP_Text handTypeName;
    public TMP_Text handTypeTotalScore;
    public TMP_Text handTypeMulitplier;
    public GameObject totalScoreBoard;
    public TMP_Text handTypeScorePreview;   // 실시간 프리뷰용
    public TMP_Text handTypeScoreAnimated;  // 연출용
    public void PreviewHand(string handName, int score)
    {
        handTypeName.text = handName;
        handTypeScorePreview.text = score.ToString(); // 여기만 사용
        handTypeScoreAnimated.text = "";              // 애니메이션용은 숨김
        handTypeMulitplier.text = "";
        handTypeTotalScore.text = "";
        totalScoreBoard.SetActive(false);
    }

    public void ClearPreview()
    {
        handTypeName.text = "";
        handTypeScorePreview.text = ""; 
        handTypeScoreAnimated.text = "";  
        handTypeMulitplier.text = "";
        handTypeTotalScore.text = "";
        totalScoreBoard.SetActive(false);
    }

    public void PlayScoreEffect(string handName, int baseScore, int multiplier, int finalScore)
    {
        Sequence seq = DOTween.Sequence();

        // 연출용 준비
        handTypeScorePreview.gameObject.SetActive(false); // 미리 보이던 거 끄고
        handTypeScoreAnimated.gameObject.SetActive(true); // 애니메이션용 켜기
        handTypeScoreAnimated.text = "0"; // 0부터 시작

        handTypeName.text = handName;
        handTypeMulitplier.text = "";
        handTypeTotalScore.text = "";
        totalScoreBoard.SetActive(false);

        seq.AppendInterval(0.3f);

        // 점수 올라가는 연출
        int displayBase = 0;
        seq.Append(DOTween.To(() => displayBase, x =>
        {
            displayBase = x;
            handTypeScoreAnimated.text = displayBase.ToString();
        }, baseScore, 0.6f).SetEase(Ease.OutQuad));

        seq.AppendCallback(() =>
        {
            handTypeMulitplier.text = $"x{multiplier}";
            totalScoreBoard.SetActive(true); // 1초 기다리기 전에 판을 먼저 켜줌
        });

        seq.AppendInterval(1f);

        // 총합 점수 증가 애니메이션
        int displayedFinalScore = 0;
        seq.Append(DOTween.To(() => displayedFinalScore, x =>
        {
            displayedFinalScore = x;
            handTypeTotalScore.text = displayedFinalScore.ToString();
        }, finalScore, 0.6f).SetEase(Ease.OutQuad));
    }
}