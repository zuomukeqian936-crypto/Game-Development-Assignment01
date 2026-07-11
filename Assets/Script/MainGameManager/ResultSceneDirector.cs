using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSceneDirector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResultScoreController _resultScoreController;
    [SerializeField] private RankingController _rankingController;
    [SerializeField] private RankingSaveData _rankingSaveData;

    [Header("Show Score Time")]
    [SerializeField] private float _invisibleScoreTime = 1f;
    [SerializeField] private float _showMyScoreTime = 2f;
    [SerializeField] private float _showRankingTime = 4f;

    [Header("UI Setting")]
    [SerializeField] private Image _backGroundImage;

    [Header("Fade Out Time"), Tooltip("背景色の変わる時間")]
    [SerializeField] private float _FadeOutTime = 2f;
    

    public bool _isShowingScore = false;

    private float _timer;

    void Start()
    {
        _backGroundImage.gameObject.SetActive(false);
        _backGroundImage.color = new Color(0f, 0f, 0f, 0f);
    }

    void Update()
    {
        if (_isShowingScore) return;

        _timer += Time.deltaTime;

        if (_timer < _invisibleScoreTime) return;
        else if (_timer < _showMyScoreTime)
        {
            _resultScoreController.ShowMyScore();
        }
        else if( _timer < _showRankingTime)
        {
            _isShowingScore = true;
            StartCoroutine(_rankingController.ShowRanking());
        }
    }

    //タイトルへ
    public void LoadSceneTitle()
    {
        _backGroundImage.gameObject.SetActive(true);

        DOTween.KillAll();
        _backGroundImage.DOFade(1f, _FadeOutTime)
            .OnComplete(() =>
             SceneManager.LoadScene("TitleScene"));
    }

    //メイン画面へ
    public void LoadMainScenen()
    {
        _backGroundImage.gameObject.SetActive(true);

        DOTween.KillAll();
        _backGroundImage.DOFade(1f, _FadeOutTime)
           .OnComplete(() =>
            SceneManager.LoadScene("MainScene"));
    }

    /// <summary>
    /// ランキングリセットボタン
    /// </summary>
    public void OnRankingReset()
    {
        _rankingSaveData.SaveRankingReset();
    }
}
