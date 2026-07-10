using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyScoreSettings _enemyScoreSettings;
    [SerializeField] private GameSceneDirector _gameSceneDirector;

    [Header("UI Setting")]
    [SerializeField] private Text _scoreText;

    [Header("Game Phase Time Settings")]
    [SerializeField] private float _earlyPhaseTime = 7f;
    [SerializeField] private float _midPhaseTime = 14f;

    [Header("Add Score Cooldown")]
    [SerializeField] private float _addScoreCooldown = 2f;

    private float _divideValue = 5;

    private bool _canAddScore = true;

    public float Score { get; private set;}


    void Update()
    {

        if(Mathf.Floor(_gameSceneDirector._gameTimer) % _divideValue != 0 || !_canAddScore || Mathf.Floor(_gameSceneDirector._gameTimer) == 0) return;
        _canAddScore = false;

        StartCoroutine(UpdateSelectScore());
        
    }

    private IEnumerator UpdateSelectScore()
    {
        yield return new WaitForSeconds(_addScoreCooldown);

        if (_gameSceneDirector._gameTimer < _earlyPhaseTime)
        {
            Score += _enemyScoreSettings.EarlyScore;
        }
        else if (_gameSceneDirector._gameTimer < _midPhaseTime)
        {
            Score += _enemyScoreSettings.MidScore;
        }
        else
        {
            Score += _enemyScoreSettings.LastScore;
        }

        UpdateScore();
        _canAddScore = true;
    }

    /// <summary>
    /// テキストスコア更新
    /// </summary>
    private void UpdateScore()
    {
        _scoreText.text = Mathf.Floor(Score).ToString();
    }

    /// <summary>
    /// 敵選択処理
    /// </summary>
    /// <param name="enemy"></param>
    public void EnemySelection(CharacterStats enemy) 
    {
        switch (enemy.Id)
        {
            case 100://妖精ID
            case 200://大量妖精ID
                UpdateScore(_enemyScoreSettings.GhostScore);
                break;

            case 101://スライム
            case 201://大量スライム
                UpdateScore(_enemyScoreSettings.SlimeScore);
                break;

            case 102://ゴースト
                UpdateScore(_enemyScoreSettings.GhostScore);
                break;

            case 103://ゾンビ
                UpdateScore(_enemyScoreSettings.ZombieScore);
                break;

            case 104://スケルトン
                UpdateScore(_enemyScoreSettings.SkeletonScore);
                break;

            case 105://フライング・ブック
                UpdateScore(_enemyScoreSettings.FlyingBookScore);
                break;

            case 500://ボス
                UpdateScore(_enemyScoreSettings.BossScore);
                break;
        }
    }

    /// <summary>
    /// スコア加算処理
    /// </summary>
    /// <param name="score"></param>
    private void UpdateScore(float score)
    {
        Score += score;
        Debug.Log(score);
    }
}
