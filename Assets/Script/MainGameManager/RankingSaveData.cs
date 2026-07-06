using System.Linq;
using UnityEngine;

public class RankingSaveData : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScoreManager _scoreManager;

    [Header("Ranking Max Count"), Tooltip("ランキングリストの最大範囲")]
    [SerializeField] private int _rankingMaxCount = 5;

    public static RankingSaveData Instance { get; private set; }
    
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
        SaveData data = new SaveData();

        data.HighScoreList.Add(_scoreManager.Score);

        SaveManager.Save(data);
        Debug.Log("Ranking Data Saved");
    }


    /// <summary>
    /// セーブデータをロード
    /// </summary>
    public void LoadRankingData()
    {
        SaveData data = SaveManager.Load();
        if (data == null)
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

        if (data.HighScoreList.Count <= _rankingMaxCount) return;

        // 最も低いスコアを取得（List<float> なので Min() が使える）
        float lowest = data.HighScoreList.Min();

        // 削除
        data.HighScoreList.Remove(lowest);

        SaveManager.Save(data);
    }


}
