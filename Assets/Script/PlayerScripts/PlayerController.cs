using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInitialStats _playerStats;
    [SerializeField] private GameSceneDirector _gameSceneDirector;
    [SerializeField] private CharacterStats _characterStats;
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
    private List<int> levelRequirements;

    public Vector2 _forward;

    private Text _levelText;



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
        levelRequirements = new List<int>();

        this._gameSceneDirector = sceneDirector;
        this._enemySpawnerController = enemySpawner;
        this._characterStats = characterStats;
        this._levelText = levelText;
        this._sliderHP = sliderHP;
        this._sliderXP = sliderXP;

        //プレイヤーの向き
        _forward = Vector2.zero;

        //経験値の閾値リスト
        levelRequirements.Add(0);
        for(int i = 1; i < 1000; i++)
        {
            int prevxp = levelRequirements[i - 1];
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
            levelRequirements.Add(prevxp + addXP);
           
        }

        _characterStats.MaxXP = levelRequirements[1];

        //UIの初期化
        SetLevelText();
        SetSliderHP();
        SetSliderXP();

        //MoveSliderHP();
    }

    /// <summary>
    /// プレイヤーの移動処理（新インプットシステム使用）
    /// </summary>
    private void MovePlayer()
    {
        Vector2 input = _playerAction.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, input.y, 0f);

        _rb2D.linearVelocity = new Vector2(move.x * _characterStats.MoveSpeed, move.y * _characterStats.MoveSpeed);

        //MoveAnimator(move);
    }

    //アニメーション再生処理
    private void MoveAnimator(Vector3 move)
    {
        //上移動アニメーション設定
        if (move.y >= 1)
        {
            trigger = "isUp";
        }

        //下移動アニメーション設定
        if (move.y <= 1)
        {
            trigger = "isDown";
        }

        //右移動アニメーション設定
        if (move.x >= 0.5f)
        {
            trigger = "isRight";
        }

        //左移動アニメーション設定
        if (move.x <= 0.5f)
        {
            trigger = "isLeft";
        }

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

        float damage = Mathf.Max(0, attack - _playerStats.Defense);
        _playerStats.HP -= damage;

        //ダメージ表示
        _gameSceneDirector.DispDamage(gameObject, damage);

        if (0 > _playerStats.HP)
        {

        }

        if (0 > _playerStats.HP) _playerStats.HP = 0;
        SetSliderHP();
    }

    /// <summary>
    /// HPスライダーの値を更新
    /// </summary>
    private void SetSliderHP()
    {
        _sliderHP.maxValue = _playerStats.MaxHP;
        _sliderHP.value = _playerStats.HP;
    }

    /// <summary>
    /// XPスライダーの値を更新
    /// </summary>
    private void SetSliderXP()
    {
        _sliderXP.maxValue = _playerStats.MaxXP;
        _sliderXP.value = _playerStats.XP;
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

    private void OnDisable()
    {
        _playerAction.Disable();
    }

}
