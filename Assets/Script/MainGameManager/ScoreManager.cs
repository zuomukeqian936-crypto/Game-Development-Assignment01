using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private EnemyScoreSettings _enemyScoreSettings;
    [SerializeField] private GameSceneDirector _gameSceneDirector;

    [Header("UI Setting")]
    [SerializeField] private Text _scoreText;

    [Header("Game Phase Time Settings")]
    [SerializeField] private float _earlyPhaseTime = 7f;
    [SerializeField] private float _midPhaseTime = 14f;

    private float _divideValue = 5;

    public float Score { get; private set;}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    
    void Update()
    {

        if( _gameSceneDirector._gameTimer % _divideValue != 0) return;

        if(_gameSceneDirector._gameTimer < _earlyPhaseTime)
        {
            Score += _enemyScoreSettings.EarlyScore;
        }
        else if(_gameSceneDirector._gameTimer < _midPhaseTime)
        {
            Score += _enemyScoreSettings.MidScore;
        }
        else
        {
            Score += _enemyScoreSettings.LastScore;
        }
        
        UpdateScore();
    }

    /// <summary>
    /// スコア更新
    /// </summary>
    private void UpdateScore()
    {
        _scoreText.text = Mathf.Floor(Score).ToString();
    }

    

}
