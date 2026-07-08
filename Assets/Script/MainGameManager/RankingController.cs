using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResultSceneDirector _resultSceneDirector;
    [SerializeField] private RankingSaveData _rankingSaveData;

    [Header("Ranking Text Settings")]
    [SerializeField] private Text[] placeTexts;   // 1〜5位の順位テキスト
    [SerializeField] private Text[] scoreTexts;   // 1～5位の順位テキスト

    [Header("Show Ranking Wait Time"), Tooltip("次のランキングの表示時間")]
    [SerializeField] private float _rankingWaitTime = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    /// <summary>
    /// /// UI初期化
    /// </summary>
    private void Init()
    {
        foreach (var text in placeTexts)
            text.gameObject.SetActive(false);

        foreach (var score in scoreTexts)
            score.gameObject.SetActive(false);
    }

    /// <summary>
    /// SaveData のランキングを UI に反映
    /// </summary>
    public IEnumerator ShowRanking()
    {
        var scores = _rankingSaveData.CurrentData.RankingScoreList;

        // ランキングを降順に並び替え
        scores.Sort();
        scores.Reverse();

        for (int i = 0; i < scores.Count && i < placeTexts.Length; i++)
        {
            placeTexts[i].gameObject.SetActive(true);
            scoreTexts[i].gameObject.SetActive(true);

            scoreTexts[i].text = Mathf.Floor(scores[i]).ToString();

            yield return new WaitForSeconds(_rankingWaitTime);
        }

        _rankingSaveData.DeleteRankingEntry();
    }
}
