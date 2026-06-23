using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "Scriptable Objects/CharacterSettings")]
public class CharacterSettings : ScriptableObject
{

    public List<CharacterStats> _datas;

    static CharacterSettings instance;
    public static CharacterSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<CharacterSettings>(nameof(CharacterSettings));
            }
            return instance;
        }
    }


    //リストのIDからデータを検索する
    public CharacterStats Get(int id)
    {
        return (CharacterStats)_datas.Find(item => item.Id == id).GetCopy();
    }

    //敵生成処理
    public EnemyController CreateEnemy(int id, GameSceneDirector sceneDirector, Vector3 position)
    {
        CharacterStats stats = Instance.Get(id);

        GameObject obj = Instantiate(stats.prefab, position, Quaternion.identity);

        EnemyController ctrl = obj.GetComponent<EnemyController>();
        ctrl.Init(sceneDirector, stats);

        return ctrl;
    }

    //player生成処理
    public PlayerController CreatePlayer(int id, GameSceneDirector sceneDirector,
        EnemySpawnerController enemySpawner, Text levelText, Slider sliderHp, Slider sliderXp)
    {
        CharacterStats stats = instance.Get(id);

        GameObject obj = Instantiate(stats.prefab, Vector3.zero, Quaternion.identity);

        //データセット
        PlayerController ctrl = obj.GetComponent<PlayerController>();
        ctrl.Init(sceneDirector,enemySpawner,stats,levelText,sliderHp,sliderXp);

        return ctrl;
    }

}
public enum MoveType
{
    TargetPlayer,
    TargetDirection
}

[Serializable]
public class CharacterStats : BaseStats
{
    public GameObject prefab;
    //初期装備武器ID
    public List<int> DefaultWeaponIds;
    //装備可能武器ID
    public List<int> UsableWeaponIds;
    //装備可変数
    public int UsableweaponMax;
    public MoveType MoveType;
    
}
