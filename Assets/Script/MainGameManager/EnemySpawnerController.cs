using System;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum SpawnType
{
    Normal,
    Group,
}

[Serializable]
public class EnemySpawnData
{
    public string _title;

    //出現経過時間
    public int _elapsedMinutes;
    public int _elapsedSeconds;

    //出現タイプ
    public SpawnType _spawnType;

    //生成時間
    public float _spawnDuration;

    //生成数
    public int _spawnCountMax;

    //生成する敵ID
    public List<int> _enemyIds;
}

public class EnemySpawnerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameSceneDirector _gameSceneDirector;
    [SerializeField] private GameObject _player;

    [Header("Enemy Spawn Data")]
    [SerializeField] private List<EnemySpawnData> _enemySpawnDatas;

    [Header("Enemy Spawn Group Radius")]
    [SerializeField] private float  _groupSpawnRadius = 0.5f;

    //生成した敵
    private List<EnemyController> _enemies;

    private Tilemap _tilemapCollider;

    private EnemySpawnData _enemySpawnData;

    //経過時間
    private float _oldSecondes;
    private float _spawnTimer;

    //現在のデータ位置
    private int _spawnDataIndex;

    //敵の出現位置
    private const float _spawnRadius = 13f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_player != null) return;

        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemySpawnData();
        SpawnEnemy();
    }

    public void Init(GameSceneDirector gameSceneDirector, Tilemap tilemapCollider)
    {
        this._gameSceneDirector = gameSceneDirector;
        this._tilemapCollider = tilemapCollider;

        //生成した敵を保存
        _enemies = new List<EnemyController>();
        _spawnDataIndex = -1;
    }

    private void SpawnEnemy()
    {
        if (_enemySpawnData == null) return;

        //タイマー更新
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer > 0) return;

        if (SpawnType.Group == _enemySpawnData._spawnType)
        {
            SpawnGroup();
        }
        else if(SpawnType.Normal == _enemySpawnData._spawnType)
        {
            SpawnNormal();
        }

        _spawnTimer = _enemySpawnData._spawnDuration;
    }

    //通常生成
    private void SpawnNormal()
    {
        Vector3 center = _player.transform.position;

        //敵生成
        for(int i = 0; i < _enemySpawnData._spawnCountMax; i++)
        {
            //プレイヤー周辺の出現場所
            float angle = 360 / _enemySpawnData._spawnCountMax * i;

            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * _spawnRadius;

            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * _spawnRadius;

            //生成位置
            Vector2 pos = center + new Vector3(x, y, 0);

            //当たり判定のあるタイル状なら生成しない処理
            if (Utils.IsColliderTile(_tilemapCollider, pos)) continue;

            CreateRandomEnemy(pos);
        }
    }

    private void CreateRandomEnemy(Vector3 pos)
    {
        //データからランダムなIDを取得
        int rnd = UnityEngine.Random.Range(0, _enemySpawnData._enemyIds.Count);
        int id = _enemySpawnData._enemyIds[rnd];

        //敵生成
        EnemyController enemy = CharactorSettings.Instance.CreateEnemy(id, _gameSceneDirector, pos);
        _enemies.Add(enemy);
    }

    //グループで生成
    private void SpawnGroup()
    {
        Vector3 center = _player.transform.position;

        //プレイヤー周辺の出現場所
        float angle = UnityEngine.Random.Range(0, 360);

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * _spawnRadius;

        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * _spawnRadius;

        //生成位置
        center += new Vector3(x, y, 0);
        float radius = _groupSpawnRadius;

        //敵生成
        for (int i = 0; i < _enemySpawnData._spawnCountMax; i++)
        {
            //プレイヤー周辺の出現場所
            angle = 360 / _enemySpawnData._spawnCountMax * i;

            x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;

            y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            //生成位置
            Vector2 pos = center + new Vector3(x, y, 0);

            //当たり判定のあるタイル状なら生成しない処理
            if (Utils.IsColliderTile(_tilemapCollider, pos)) continue;

            CreateRandomEnemy(pos);
        }
    }

    /// <summary>
    /// 経過時間で敵の生成データを入れる処理
    /// </summary>
    private void UpdateEnemySpawnData()
    {
        if(_oldSecondes == _gameSceneDirector._oldSeonds) return;

        int idx = _spawnDataIndex + 1;

        //データの最後
        if (_enemySpawnDatas.Count - 1 < idx) return;

        //設定された経過時間を超えていたらデータを入れ替える
        EnemySpawnData data = _enemySpawnDatas[idx];
        int elapsedSeconds = data._elapsedMinutes * 60 + data._elapsedSeconds;

        if(elapsedSeconds < _gameSceneDirector._gameTimer)
        {
            _enemySpawnData = _enemySpawnDatas[idx];

            //次回用の設定
            _spawnDataIndex = idx;
            _spawnTimer = 0;
            _oldSecondes = _gameSceneDirector._oldSeonds;
        }
    }

    //すべての敵を返す
    public List<EnemyController> GetEnemies()
    {
        _enemies.RemoveAll(item => item == null);
        return _enemies;
    }
}
