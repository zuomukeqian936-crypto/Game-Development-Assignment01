using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingSaveData : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScoreManager _scoreManager;

    [Header("Ranking Max Count"), Tooltip("ランキングリストの最大範囲")]
    [SerializeField] private int _rankingMaxCount = 5;

    public static RankingSaveData Instance { get; private set; }

    public SaveData CurrentData { get; private set; } //セーブデータの保存

    void Awake()
    {
        Instance = this;


    }

    void Start()
    {
        // セーブデータがあるならロード
        if (SaveManager.Load() != null)
        {
            LoadRankingData();
        }
    }

    /// <summary>
    /// セーブ
    /// </summary>
    public void SaveRankingData()
    {
        SaveData data = SaveManager.Load();

        // データがなければ新規作成
        if (data == null)
        {
            data = new SaveData();
        }

        data.RankingScoreList.Add(_scoreManager.Score);
        data.MyScore = _scoreManager.Score;

        SaveManager.Save(data);
        Debug.Log("Ranking Data Saved");

        foreach(var score in data.RankingScoreList)
        {
            Debug.Log(score);
        }

        SceneManager.LoadScene("ResultScene");
    }


    /// <summary>
    /// セーブデータをロード
    /// </summary>
    public void LoadRankingData()
    {
        CurrentData = SaveManager.Load();
        if (CurrentData == null)
        {
            Debug.Log("No Save Data");
            return;
        }

        Debug.Log("Ranking Data Loaded");
    }

    /// <summary>
    /// ランキングリストが５以上のときは最下位を削除する処理
    /// </summary>
    public void DeleteRankingEntry()
    {
        SaveData data = SaveManager.Load();

        if (data.RankingScoreList.Count <= _rankingMaxCount) return;

        // 最も低いスコアを取得（List<float> なので Min() が使える）
        float lowest = data.RankingScoreList.Min();

        // 削除
        data.RankingScoreList.Remove(lowest);

        Debug.Log($"{lowest}が削除されました");
        Debug.Log(data.RankingScoreList.Count);

        SaveManager.Save(data);
    }


}
