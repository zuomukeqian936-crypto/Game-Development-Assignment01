using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSpawnerSettings", menuName = "Scriptable Objects/WeaponSettings")]
public class WeaponSpawnerSettings : ScriptableObject
{

    public List<WeaponSpawnerStats> _datas;

    static WeaponSpawnerSettings instance;
    public static WeaponSpawnerSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<WeaponSpawnerSettings>(nameof(WeaponSpawnerSettings));
            }
            return instance;
        }
    }


    //リストのIDからデータを検索する
    public WeaponSpawnerStats Get(int id, int lv)
    {
        WeaponSpawnerStats ret = null;

        foreach (var item in _datas)
        {
            if (id != item.Id) continue;

            if (lv == item.Lv)
            {
                return (WeaponSpawnerStats)item.GetCopy();
            }

            //仮のデータが超えるレベルがあったら入れる処理
            if (null == ret)
            {
                ret = item;
            }

            //探しているレベルより下で暫定データより大きい場合の処理
            else if (item.Lv < lv && ret.Lv < item.Lv)
            {
                ret = item;
            }
        }

        return (WeaponSpawnerStats)ret.GetCopy();
    }

    //作成
    public BaseWeaponSpawner CreateWeaponSpawner(int id, EnemySpawnerController enemySpawner, Transform parent = null)
    {
        WeaponSpawnerStats stats = Instance.Get(id, 1);
        //オブジェクト作成
        GameObject obj = Instantiate(stats._prefabSpawner, parent);
        //データセット
        BaseWeaponSpawner spawner = obj.GetComponent<BaseWeaponSpawner>();
        spawner.Init(enemySpawner, stats);

        return spawner;
    }

}
//武器生成処理
    

[System.Serializable]
public class WeaponSpawnerStats : BaseStats
{
    public GameObject _prefabSpawner;
    public Sprite _icon;
    public int _leevlUpItemId;　　　//レベルアップの際に追加されるアイテムID

    //生成する数
    public float _spawnCount;

    public float _spawnTimerMin;
    public float _spawnTimerMax;

    //生成時間取得
    public float GetRandomSpawnTimer()
    {
        return Random.Range(_spawnTimerMin, _spawnTimerMax);
    }
}
