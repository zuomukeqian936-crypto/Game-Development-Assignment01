using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResultSceneDirector _resultSceneDirector;
    [SerializeField] private SaveData saveData;

    [Header("Ranking Text Settings")]
    [SerializeField] private Text[] placeTexts;   // 1〜5位の順位テキスト
    [SerializeField] private Text[] scoreTexts;   // 1～5位の順位テキスト


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    /// <summary>
    /// /// 初期化
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
    public void ShowRanking()
    {
        var scores = saveData.RankingScoreList;

        // ランキングを降順に並び替え
        scores.Sort();
        scores.Reverse();

        for (int i = 0; i < scores.Count && i < placeTexts.Length; i++)
        {
            placeTexts[i].gameObject.SetActive(true);
            scoreTexts[i].gameObject.SetActive(true);

            scoreTexts[i].text = scores[i].ToString();
        }

        _resultSceneDirector.IsShowingScore = true;
    }
}
