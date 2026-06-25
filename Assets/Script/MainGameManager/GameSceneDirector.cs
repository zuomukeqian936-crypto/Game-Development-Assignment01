using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameSceneDirector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _grid;
    [SerializeField] private Tilemap _tileMapCollider;
    [SerializeField] private Transform _parentDamageText;
    [SerializeField] private GameObject _prefabDamageText;
    [SerializeField] private EnemySpawnerController _enemySpawnerController;
    [SerializeField] public PlayerController _playerController;

    [Header("Timer Settings")]
    [SerializeField] private Text _textTimer;
    public float _gameTimer;
    public float _oldSeonds = -1;

    [Header("Player Spawn Settings")]
    [SerializeField] private Slider _sliderXP;
    [SerializeField] private Slider _sliderHP;
    [SerializeField] private Text _levelText;

    [Header("XP Prefab")]
    [SerializeField] private List<GameObject> _prefabXP;

    public Vector2 _TileMapStart;
    public Vector2 _TileMapEnd;
    public Vector2 _WorldStart;
    public Vector2 _WorldEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetInitialPlayer();
        SetInitialMovementRange();
        _enemySpawnerController.Init(this, _tileMapCollider);

        if (_playerController != null) return;
        _playerController = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        UpdateGameTimer();
    }

    /// <summary>
    /// カメラの移動範囲を設定
    /// </summary>
    private void SetInitialMovementRange()
    {
        foreach (Transform item in _grid.GetComponentInChildren<Transform>())
        {
            //開始位置
            if (_TileMapStart.x > item.position.x)
            {
                _TileMapStart.x = item.position.x;
            }
            if (_TileMapStart.y > item.position.y)
            {
                _TileMapStart.y = item.position.y;
            }

            //終了位置
            if (_TileMapEnd.x < item.position.x)
            {
                _TileMapEnd.x = item.position.x;
            }
            if (_TileMapEnd.y < item.position.y)
            {
                _TileMapEnd.y = item.position.y;
            }
        }

        //画面半分の描画範囲　（デフォルトで５タイル）
        float cameraSize = Camera.main.orthographicSize;

        //画面縦横比(16:9)
        float aspect = (float)Screen.width / (float)Screen.height;

        //プレイヤーの移動できる範囲
        _WorldStart = new Vector2(_TileMapStart.x - cameraSize * aspect, _TileMapStart.y - cameraSize);
        _WorldEnd = new Vector2(_TileMapEnd.x + cameraSize * aspect, _TileMapEnd.y + cameraSize);
    }  

    /// <summary>
    /// ダメージ表示処理
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    public void DispDamage(GameObject target, float damage)
    {
        GameObject obj = Instantiate(_prefabDamageText, _parentDamageText);
        obj.GetComponent<DamageText>().Init(target, damage);
    }

    /// <summary>
    /// ゲームタイマー更新処理
    /// </summary>
    private void UpdateGameTimer()
    {
        _gameTimer += Time.deltaTime;

        int seconds = (int)_gameTimer % 60;
        if (seconds == _oldSeonds) return;

        _textTimer.text = Utils.GetTextTimer(_gameTimer);
        _oldSeonds = seconds;
    }


    private void SetInitialPlayer()
    {
        //プレイヤー作成
        int playerId = 0;
        _playerController = CharacterSettings.Instance.CreatePlayer(playerId, this, _enemySpawnerController, _levelText, _sliderHP, _sliderXP);
    }

    //経験値取得
    public void CreateXP(EnemyController enemyController)
    {
        float xp = Random.Range(enemyController._characterStats.XP, enemyController._characterStats.MaxXP);
        if (0 > xp) return;

        //5未満
        GameObject prefab = _prefabXP[0];
        //10以上
        if(10 <= xp)
        {
            prefab = _prefabXP[2];
        }
        else if(5  <= xp)
        {
            prefab = _prefabXP[1];
        }

        //初期化
        GameObject obj = Instantiate(prefab, enemyController.transform.position, Quaternion.identity);
        XPController ctrl = obj.GetComponent<XPController>();
        ctrl.Init(this, xp);
    }
}
