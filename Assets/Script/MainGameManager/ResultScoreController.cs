using UnityEngine;
using UnityEngine.UI;

public class ResultScoreController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RankingSaveData _rankingSaveData;

    [Header("Score Text")]
    [SerializeField] private Text _scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _scoreText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 自分のスコアを表示する処理
    /// </summary>
    public void ShowMyScore()
    {
        _rankingSaveData.LoadRankingData();
        _scoreText.text = Mathf.Floor(_rankingSaveData.CurrentData.MyScore).ToString();
        _scoreText.gameObject.SetActive(true);
    }    
}
