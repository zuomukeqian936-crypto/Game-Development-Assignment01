using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseWeaponSpawner : MonoBehaviour
{
    //武器のプレハブ
    [SerializeField] private  GameObject _prefabWeapon;

    public WeaponSpawnerStats _weaponStats;

    public float _totalDamage; //与えた総ダメージ
    public float _totalTimer; //稼働タイマー

    //生成タイマー
    protected float _spawnTimer;
    //生成下武器のリスト
    protected List<BaseWeapon> _weapons;

    protected EnemySpawnerController _enemySpawner;

    //初期化
    public void Init(EnemySpawnerController enemySpawner, WeaponSpawnerStats weaponStats)
    {
        _weapons = new List<BaseWeapon>();
        this._enemySpawner = enemySpawner;
        this._weaponStats = weaponStats;    
    }

    //稼働タイマー
    private void FixedUpdate()
    {
        _totalTimer += Time.fixedDeltaTime;
    }

    //武器生成
    protected BaseWeapon CreateWeapon(Vector3 position,Vector3 forward, Transform parent = null)
    {
        if (_prefabWeapon == null)
        {
            Debug.LogError($"{name}: _prefabWeapon が設定されていません");
            return null;
        }
        //生成
        GameObject obj = Instantiate(_prefabWeapon, position, _prefabWeapon.transform.rotation, parent);

        //共通データセット
        BaseWeapon weapon = obj.GetComponent<BaseWeapon>();

        //データ初期化
        weapon.Init(this, forward);

        _weapons.Add(weapon);

        return weapon;
    }

    //武器生成
    protected BaseWeapon CreateWeapon(Vector3 position, Transform parent = null)
    {
        return CreateWeapon(position, Vector2.zero, parent);
    }

    //武器のアップデートを停止する
    public void SetEnabled(bool enabled = true)
    {
        this.enabled = enabled;

        _weapons.RemoveAll(item => !item);

        //生成した武器を停止
        foreach(var item in _weapons)
        {
            item.enabled = enabled;
            //物理処理停止
            item.GetComponent<Rigidbody2D>().simulated = enabled;
        }
    }

    //タイマー消化チェック
    protected bool isSpawnTimerNotElapsed()
    {
        _spawnTimer -= Time.deltaTime;
        if (0 < _spawnTimer) return true;

        return false;
    }

    //レベルアップ時のデータを返す
    public WeaponSpawnerStats GetLevelUpStats(bool isNextLevel = false)
    {
        //次のレベル
        int nextLv = _weaponStats.Lv + 1;

        //次のレベルがあるかどうか調べ、あれば上書き
        WeaponSpawnerStats ret = WeaponSpawnerSettings.Instance.Get(_weaponStats.Id, nextLv);

        if(_weaponStats.Lv < ret.Lv)
        {

        }
        else
        {
            //説明をアイテムのものに書き換える
            ItemData itemData = ItemSettings.Instance.Get(_weaponStats._leevlUpItemId);
            ret.Description = itemData.Description;
        }

        //レベルを1あげて返すかどうか
        if (isNextLevel)
        {
            ret.Lv = nextLv;
        }

        return ret;
    }

    //レベルアップ
    public void LevelUp()
    {
        //現在のレベル
        int lv = _weaponStats.Lv;

        //次のレベルデータ
        WeaponSpawnerStats nextData = GetLevelUpStats();

        //現在のレベルと違えば上書き
        if (_weaponStats.Lv < nextData.Lv)
        {
            _weaponStats = nextData;
        }
        else
        {
            //説明をアイテムのものに書き換える
            ItemData itemData = ItemSettings.Instance.Get(_weaponStats._leevlUpItemId);
            _weaponStats.AddItemData(itemData);
        }

        _weaponStats.Lv = lv + 1;
    }
}
