using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static SaveData Load()
    {
        if (!File.Exists(path))
        {
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void Delete()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            //Debug.Log("セーブデータ削除完了");
        }
    }

}

[System.Serializable]
public class SaveData
{
    public List<float> HighScoreList;
}

