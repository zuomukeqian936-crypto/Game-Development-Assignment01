using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneDirector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResultScoreController _resultScoreController;
    [SerializeField] private RankingController _rankingController;

    [Header("Show Score Time")]
    [SerializeField] private float _invisibleScoreTime = 1f;
    [SerializeField] private float _showMyScoreTime = 2f;
    [SerializeField] private float _showRankingTime = 4f;
    

    public bool IsShowingScore = false;

    private float _timer;

    void Update()
    {
        if (!IsShowingScore) return;

        _timer += Time.deltaTime;

        if (_timer < _invisibleScoreTime) return;
        else if (_timer < _showMyScoreTime)
        {
            _resultScoreController.ShowMyScore();
        }
        else if( _timer < _showRankingTime)
        {
            _rankingController.ShowRanking();
        }
    }

    //タイトルへ
    public void LoadSceneTitle()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("TitleScene");
    }

    //メイン画面へ
    public void LoadMainScenen()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("MainScene");
    }
}
