using System.Collections.Generic;
using UnityEngine;
using static BaseStats;

[CreateAssetMenu(fileName = "ItemSettings", menuName = "Scriptable Objects/ItemSettings")]
public class ItemSettings : ScriptableObject
{

    public List<ItemData> _datas;

    static ItemSettings instance;
    public static ItemSettings Instance
    {
        get
        {
            if(!instance)
            {
                instance = Resources.Load<ItemSettings>(nameof(ItemSettings));
            }
            return instance;
        }
    }

    //リストのIDからデータを検索する
    public ItemData Get(int id)
    {
        return (ItemData)_datas.Find(item => item.Id == id).GetCopy();
    }
}

[System.Serializable]

public class ItemData
{
    public string Title;
    //固有ID
    public int Id;
    //アイテム名
    public string Name;
    //説明
    [TextArea] public string Description;
    //アイコン
    public Sprite Icon;
    //ボーナス
    public List<BonusStats> Bonuses;
    //コピーしてデータを残す
    public ItemData GetCopy()
    {
        return (ItemData)MemberwiseClone();
    }
}