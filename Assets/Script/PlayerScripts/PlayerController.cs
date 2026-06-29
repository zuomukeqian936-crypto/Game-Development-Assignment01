using DG.Tweening;
using Dreamteck;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameSceneDirector _gameSceneDirector;
    [SerializeField] public CharacterStats _characterStats;
    [SerializeField] private EnemySpawnerController _enemySpawnerController;

    private Slider _sliderHP;
    private Slider _sliderXP;

    [Header("Player Attack Cool Down Time")]
    private float _maxCoolDownTime = 0.5f;
    private float _coolDownTimer;

    [Header("Max Level")]
    [SerializeField] private int _maxLevel = 1000;

    [Header("Add XP")]
    [SerializeField] private int _addXP = 16;

    private Rigidbody2D _rb2D;
    private PlayerControls _playerAction;
    private Animator _animator;

    //必要XP
    private List<int> _levelRequirements;

    public Vector2 _forward;

    private Text _levelText;

    //現在装備中の武器
    public List<BaseWeaponSpawner> _weaponSpawner;

    //追加したアイテムと個数
    public Dictionary<ItemData, int> _itemDatas;

    string trigger = "";

    void Awake()
    {
        _playerAction = new PlayerControls();

        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerAction.Enable();
    }
    
    private void Update()
    {
        UpdateTimer();
        MovePlayer();      
    }

    
    public void Init(GameSceneDirector sceneDirector, EnemySpawnerController enemySpawner,
        CharacterStats characterStats, Text levelText, Slider sliderHP, Slider sliderXP)
    { 
        _levelRequirements = new List<int>();
        _weaponSpawner = new List<BaseWeaponSpawner>();
        _itemDatas = new Dictionary<ItemData, int>();

        this._gameSceneDirector = sceneDirector;
        this._enemySpawnerController = enemySpawner;
        this._characterStats = characterStats;
        this._levelText = levelText;
        this._sliderHP = sliderHP;
        this._sliderXP = sliderXP;

        //プレイヤーの向き
        _forward = Vector2.zero;

        //経験値の閾値リスト
        _levelRequirements.Add(0);
        for(int i = 1; i < 1000; i++)
        {
            int prevxp = _levelRequirements[i - 1];
            //41以降はレベル枚に16XPずつ増加
            int addXP = _addXP;

            //レベル2までレベルアップするのに5XP
            if(i == 1)
            {
                addXP = 5;
            }
            else if(20 >= i)
            {
                addXP = 10;
            }
            else if (40 >= i)
            {
                addXP = 13;
            }

            //必要な経験値
            _levelRequirements.Add(prevxp + addXP);
           
        }

        _characterStats.MaxXP = _levelRequirements[1];

        //UIの初期化
        SetLevelText();
        SetSliderHP();
        SetSliderXP();

        //MoveSliderHP();

        //武器データセット
        foreach(int item in characterStats.DefaultWeaponIds)
        {
            AddWeaponSpawner(item);
        } 
    }

    /// <summary>
    /// プレイヤーの移動処理（新インプットシステム使用）
    /// </summary>
    private void MovePlayer()
    {
        Vector2 input = _playerAction.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, input.y, 0f);

        _rb2D.linearVelocity = new Vector2(move.x * _characterStats.MoveSpeed, move.y * _characterStats.MoveSpeed);

        _forward = new Vector2(move.x, move.y).normalized;

        MoveAnimator(move);
    }

    //アニメーション再生処理
    private void MoveAnimator(Vector2 move)
    {
        //上移動アニメーション設定
        if (move.y >= 1 && move.x == 0)
        {
            trigger = "isBackward";
        }

        //下移動アニメーション設定
        if (move.y <= -1 && move.x == 0)
        {
            trigger = "isForward";
        }

        //右移動アニメーション設定
        if (move.x >= 0.5f)
        {
            trigger = "isRight";
        }

        //左移動アニメーション設定
        if (move.x <= -0.5f)
        {
            trigger = "isLeft";
        }

        if (Vector2.zero == move) return;

        //アニメーション再生処理
        _animator.SetTrigger(trigger);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="attack"></param>
    public void Damage(float attack)
    {
        if (!enabled) return;

        float damage = Mathf.Max(0, attack - _characterStats.Defense);
        _characterStats.HP -= damage;

        //ダメージ表示
        _gameSceneDirector.DispDamage(gameObject, damage);

        if (0 > _characterStats.HP)
        {
            //操作できないようにする
            SetEnabled(false);

            //アニメーション
            transform.DOScale(new Vector2(5, 0), 2).SetUpdate(true)
                .OnComplete(() =>
                {
                    SceneManager.LoadScene("ResultScene");
                });
        }

        if (0 > _characterStats.HP) _characterStats.HP = 0;
        SetSliderHP();
    }

    /// <summary>
    /// HPスライダーの値を更新
    /// </summary>
    private void SetSliderHP()
    {
        _sliderHP.maxValue = _characterStats.MaxHP;
        _sliderHP.value = _characterStats.HP;
    }

    /// <summary>
    /// XPスライダーの値を更新
    /// </summary>
    private void SetSliderXP()
    {
        _sliderXP.maxValue = _characterStats.MaxXP;
        _sliderXP.value = _characterStats.XP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AttackEnemy(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        AttackEnemy(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    /// <summary>
    /// プレイヤーへ攻撃する処理
    /// </summary>
    /// <param name="collsiion"></param>
    private void AttackEnemy(Collision2D collsiion)
    {
        if (!collsiion.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        if (0 < _coolDownTimer) return;

        enemy.Damage(_characterStats.Attack);
        _coolDownTimer = _maxCoolDownTime;
    }

    //タイマー更新処理
    private void UpdateTimer()
    {
        if (0 < _coolDownTimer)
        {
            _coolDownTimer -= Time.deltaTime;
        }
    }

    //レベルテキスト更新
    private void SetLevelText()
    {
        _levelText.text = "Lv" + _characterStats.Lv;
    }

    private void AddWeaponSpawner(int id)
    {
        //装備済みならレベルアップ
        BaseWeaponSpawner spawner = _weaponSpawner.Find(item => item._weaponStats.Id == id);

        if (spawner)
        {
            spawner.LevelUp();
            return;
        }

        //新規追加
        spawner = WeaponSpawnerSettings.Instance.CreateWeaponSpawner(id, _enemySpawnerController, transform);

        if(null == spawner)
        {
            Debug.LogError("武器データがありません");
            return;
        }

        //装備済みリストへ追加
        _weaponSpawner.Add(spawner);
    }

    //経験値取得
    public void GetXP(float xp)
    {
        _characterStats.XP += xp;

        //レベル上限
        if (_levelRequirements.Count - 1 < _characterStats.Lv) return;

        //レベル上限
        if (_levelRequirements[_characterStats.Lv] <= _characterStats.XP)
        {
            _characterStats.Lv++;

            //次の経験値
            if(_characterStats.Lv < _levelRequirements.Count)
            {
                _characterStats.XP = 0;
                _characterStats.MaxXP = _levelRequirements[_characterStats.Lv];
            }
            //レベルアップパネルの表示
            _gameSceneDirector.DispPanelLevelUp();

            SetLevelText();
        }
        //表示更新
        SetSliderXP();
    }

    private void OnDisable()
    {
        _playerAction.Disable();
    }

    //装備可能な武器リスト
    public List<int> GetUsableWeaponIds()
    {
        List<int> ret = new List<int>(_characterStats.UsableWeaponIds);

        //装備可能数を超える場合は装備している武器のIDを返す
        if(_characterStats.UsableweaponMax -1 < _weaponSpawner.Count)
        {
            ret.Clear();
            foreach(var item in _weaponSpawner)
            {
                ret.Add(item._weaponStats.Id);
            }
        }

        return ret;
    }

    //装備可能な武器をランダムで返す
    public WeaponSpawnerStats GetRandomSpawnerStats()
    {
        //装備可能な武器ID
        List<int> usableIds = GetUsableWeaponIds();

        //装備可能な武器がない
        if (1 > usableIds.Count)
        {
            return null;
        }

        //抽選
        int rnd = Random.Range(0, usableIds.Count);
        int id = usableIds[rnd];

        //装備済みなら次のレベルのデータ
        BaseWeaponSpawner spawner = _weaponSpawner.Find(item => item._weaponStats.Id == id);

        if (spawner)
        {
            return spawner.GetLevelUpStats(true);
        }

        //新規ならレベル1のデータ
        return WeaponSpawnerSettings.Instance.Get(id, 1);

    }

    private void AddItemData(int id)
    {
        ItemData itemData = ItemSettings.Instance.Get(id);

        if(null == itemData)
        {
            Debug.LogError("アイテムデータが見つかりませんでした");
            return;
        }

        //データ追加
        _characterStats.AddItemData(itemData);

        //取得済みリストへ追加
        ItemData key = null;
        foreach(var item in _itemDatas)
        {
            if(item.Key.Id == itemData.Id)
            {
                key = item.Key;
                break;
            }
        }

        if(null == key)
        {
            _itemDatas.Add(itemData, 0);
            key = itemData;
        }

        _itemDatas[key]++;
    }

    //レベルアップやアイテム取得
    public void AddBonusData(BonusData bonusData)
    {
        if (null == bonusData) return;

        //武器データ
        if(null != bonusData._weaponSpawnerStats)
        {
            AddWeaponSpawner(bonusData._weaponSpawnerStats.Id);
        }

        //アイテムデータ
        if(null != bonusData._itemData)
        {
            AddItemData(bonusData._itemData.Id);
        }

        SetSliderHP();
    }

    //アップデート停止
    public void SetEnabled(bool enabled = true)
    {
        this.enabled = enabled;

        //武器
        foreach(var item in _weaponSpawner)
        {
            item.SetEnabled(enabled);
        }
    }

}
