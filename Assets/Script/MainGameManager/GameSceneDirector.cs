using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private PanelLevelUpController _panelLevelUpController;
    [SerializeField] private PanelTreasureChestController _treasureChestController;

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

    [Header("Treasures Settigns")]//宝箱関連
    [SerializeField] private GameObject _treasureChestPrefab;
    [SerializeField] private List<int> _treasureChestItemIds;
    [SerializeField] private float _treasureChestTimerMin;
    [SerializeField] private float _treasureChestTimerMax;
    private float _treasureChestTimer;

    [Header("Left Icon Image Settings")]//左上に表示するアイコン
    [SerializeField] private Transform _canbas;
    [SerializeField] private GameObject _ImagePlayerIconPrefab;
    private Dictionary<BaseWeaponSpawner, GameObject> _playerWeaponIcons;
    private Dictionary<ItemData, GameObject> _playerItemIcons;
    private const int _playerIconStartX = 20;
    private const int _playerIconStartY = -40;

    [Header("Enemy Kill Count UI Settings")]
    [SerializeField] Text _defeatedEnemyCountText;
    public int _defeatedEnemyCount;

    public Vector2 _TileMapStart;
    public Vector2 _TileMapEnd;
    public Vector2 _WorldStart;
    public Vector2 _WorldEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //変数初期化
        _playerWeaponIcons = new Dictionary<BaseWeaponSpawner, GameObject>();
        _playerItemIcons = new Dictionary<ItemData, GameObject>();
        SetInitialPlayer();
        SetInitialMovementRange();
        _enemySpawnerController.Init(this, _tileMapCollider);
        _panelLevelUpController.Init(this);
        _treasureChestController.Init(this);

        //初期値
        _treasureChestTimer = Random.Range(_treasureChestTimerMin, _treasureChestTimerMax);
        _defeatedEnemyCount = -1;

        dispPlayerIcon();

        //倒した敵更新
        AddDefeatedEnemy();

        //TimeScaleリセット
        SetEnabled();

        if (_playerController != null) return;
        _playerController = FindAnyObjectByType<PlayerController>();

        
    }

    void Update()
    {
        //ゲームタイマー更新
        UpdateGameTimer();
        //宝箱生成
        UpdateTreasureChestSpawner();
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

    //ゲーム再開/停止処理
    private void SetEnabled(bool enabled = true)
    {
        this.enabled = enabled; 
        Time.timeScale = (enabled) ? 1 : 0;
        _playerController.SetEnabled(enabled);
    }

    //ゲーム再開
    public void PlayGame(BonusData bonusData = null)
    {
        //アイテム追加
        _playerController.AddBonusData(bonusData);

        //ステータス反映
        dispPlayerIcon();

        //ゲーム再開
        SetEnabled();
    }

    //レベルアップ時
    public void DispPanelLevelUp()
    {
        //追加したアイテム
        List<WeaponSpawnerStats> items = new List<WeaponSpawnerStats>();

        //生成数
        int randomCount = _panelLevelUpController.GetButtonCount();
        //武器の数が足りない場合は減らす
        int listCount = _playerController.GetUsableWeaponIds().Count;

        if(listCount < randomCount)
        {
            randomCount = listCount;
        }

        for(int i = 0; i < randomCount; i++)
        {
            //ボーナスをランダムで生成
            WeaponSpawnerStats randomItem = _playerController.GetRandomSpawnerStats();

            //データなし
            if (null == randomItem) continue;

            //かぶりチェック
            WeaponSpawnerStats findItem = items.Find(item => item.Id == randomItem.Id);

            //かぶり無し
            if(null == findItem)
            {
                items.Add(randomItem);
            }
            //もう一周
            else
            {
                i--;
            }
        }

        //レベルアップパネル表示
        _panelLevelUpController.DispPanel(items);

        //ゲーム停止
        SetEnabled(false);
    }

    //宝箱パネルを表示
    public void DispPanelTreasureChest()
    {
        //ランダムアイテム
        ItemData item = GetRandomItemData();

        //データなし
        if(null == item) return;

        //パネル表示
        _treasureChestController.DispPanel(item);
        //ゲーム中断
        SetEnabled(false);
    }

    //アイテムをランダムで返す
    private ItemData GetRandomItemData()
    {
        if (1 > _treasureChestItemIds.Count) return null;

        //抽選
        int rnd = Random.Range(0, _treasureChestItemIds.Count);
        return ItemSettings.Instance.Get(_treasureChestItemIds[rnd]);
    }

    //宝箱生成
    private void UpdateTreasureChestSpawner()
    {
        //タイマー
        _treasureChestTimer -= Time.deltaTime;
        //タイマー未消化
        if (0 < _treasureChestTimer) return;

        //生成場所
        float x = Random.Range(_WorldStart.x, _WorldEnd.x);
        float y = Random.Range(_WorldStart.y, _WorldEnd.y);

        //当たり判定のあるタイル上かどうか
        if (Utils.IsColliderTile(_tileMapCollider, new Vector2(x, y))) return;

        //生成
        GameObject obj = Instantiate(_treasureChestPrefab, new Vector3(x, y, 0), Quaternion.identity);
        obj.GetComponent<TresureChestController>().Init(this);

        //次のタイマーセット
        _treasureChestTimer = Random.Range(_treasureChestTimerMin, _treasureChestTimerMax);
    }

    //プレイヤーアイコンセット
    private void SetPlayerIcon(GameObject obj, Vector2 pos, Sprite icon, int count)
    {
        //画像
        Transform image = obj.transform.Find("Icon Image");
        image.GetComponent<Image>().sprite = icon;

        //テキスト
        Transform text = obj.transform.Find("Count Text");
        text.GetComponent<TextMeshProUGUI>().text = "" + count;

        //場所
        obj.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    //アイコンの表示を更新
    private void dispPlayerIcon()
    {
        //武器アイコンの表示位置
        float x = _playerIconStartX;
        float y = _playerIconStartY;
        float w = _ImagePlayerIconPrefab.GetComponent<RectTransform>().sizeDelta.x + 1;

        foreach(var item in _playerController._weaponSpawner)
        {
            //作成済みのデータがあれば取得する
            _playerWeaponIcons.TryGetValue(item, out GameObject obj);

            //なければ作成する
            if (!obj)
            {
                obj = Instantiate(_ImagePlayerIconPrefab, _canbas);
                _playerWeaponIcons.Add(item, obj);
            }

            //アイコンセット
            SetPlayerIcon(obj, new Vector2(x, y), item._weaponStats._icon, item._weaponStats.Lv);

            //次の位置
            x += w;
        }

        //アイテムアイコン表示位置
        x = _playerIconStartX;
        y = _playerIconStartY - w;

        foreach(var item in _playerController._itemDatas)
        {
            //作成済みのデータがあれば取得する
            _playerItemIcons.TryGetValue(item.Key, out GameObject obj);

            //なければ作成する
            if (!obj)
            {
                obj = Instantiate(_ImagePlayerIconPrefab, _canbas);
                _playerItemIcons.Add(item.Key, obj);
            }

            //アイコンセット
            SetPlayerIcon(obj, new Vector2(x, y), item.Key.Icon, item.Value);

            //次の位置
            x += w;
        }
    }

    //倒した敵をカウント
    public void AddDefeatedEnemy()
    {
        _defeatedEnemyCount++;
        _defeatedEnemyCountText.text = "" + _defeatedEnemyCount;
    }
}
