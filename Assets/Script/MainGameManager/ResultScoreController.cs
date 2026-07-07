using UnityEngine;
using UnityEngine.UI;

public class ResultScoreController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SaveData _saveData;

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
        _scoreText.text = Mathf.Floor(_saveData.MyScore).ToString();
        _scoreText.gameObject.SetActive(true);
    }

    
}
