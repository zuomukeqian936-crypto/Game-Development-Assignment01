using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyController : MonoBehaviour
{
    private enum State
    {
        Alive,
        Dead,
    }

    [Header("References")]
    [SerializeField] private CharacterStats _characterStats;
    [SerializeField] private GameSceneDirector _sceneDirector;
    [SerializeField] private GameObject _player;
    

    [Header("Enemy Attack Cool Down Time")]
    private float _maxCoolDownTime = 0.5f;
    private float _coolDownTimer;

    [Header("Pace Variation Setting")]
    [SerializeField] private float _minValue = 0.8f;
    [SerializeField] private float _maxValue = 1.2f;

    [Header("Character Size Settings")]
    [SerializeField] private float _size = 0.8f;

    [Header("Character Rotation Settings")]
    [SerializeField] private float _rotation = 10f;

    private Vector2 _forward;

    private State _state;

    private Rigidbody2D _rb2D;

    
    void Update()
    {
        UpdateTimer();
        PlayerDirection();
    }



    //初期化
    public void Init(GameSceneDirector gameSceneDirector, CharacterStats characterStats)
    {
        this._sceneDirector = gameSceneDirector;
        this._characterStats = characterStats;

        _rb2D = GetComponent<Rigidbody2D>();

        float random = Random.Range(_minValue, _maxValue);
        float Speed = 1 / _characterStats.MoveSpeed * random;

        //サイズ
        float addx = _size;
        float x = addx;
        transform.DOScale(x, Speed)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo);

        //回転
        float addz = _rotation;
        float z = Random.Range(-addz, addz);

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = z;

        transform.eulerAngles = rotation;
        transform.DORotate(new Vector3(0, 0, -z), Speed)
            .SetLoops(-1, LoopType.Yoyo);

        //進む方向
        _player = GameObject.FindWithTag("Player");

        if (_player != null)
        {
            Vector2 dir = _player.transform.position - transform.position;
            _forward = dir;

            _state = State.Alive;
        }
    }

    /// <summary>
    /// playerの方向を計算する処理
    /// </summary>
    private void PlayerDirection()
    {
        if (State.Alive != _state) return;

        if (_player != null)
        {
            Vector2 dir = _player.transform.position - transform.position;
            _forward = dir.normalized;

            MoveEnemy();
        }
    }

    //敵移動処理
    private void MoveEnemy()
    {
        _rb2D.position += _forward * _characterStats.MoveSpeed * Time.deltaTime;
    }

    //タイマー更新処理
    private void UpdateTimer()
    {
        if(0 < _coolDownTimer)
        {
            _coolDownTimer -= Time.deltaTime;
        }

        if (_characterStats.AliveTime > 0)
        {
            _characterStats.AliveTime -= Time.deltaTime;
            if (_characterStats.AliveTime <= 0)
                SetDead(false);
        }

    }

    //敵が死んだときに呼び出される処理
    private void SetDead(bool createXP = true)
    {
        if (State.Alive != _state) return;

        //物理挙動停止
        _rb2D.simulated = false;

        //アニメーション停止
        transform.DOKill();
        
        //縦につぶれるアニメーション
        transform.DOScaleY(0,0.5f).OnComplete(()  => Destroy(gameObject));

        if (createXP)
        {

        }

        _state = State.Dead;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AttackPlayer(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        AttackPlayer(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    /// <summary>
    /// プレイヤーへ攻撃する処理
    /// </summary>
    /// <param name="collsiion"></param>
    private void AttackPlayer(Collision2D collsiion)
    {
        if (!collsiion.gameObject.TryGetComponent<PlayerController>(out var player)) return;

        if (0 < _coolDownTimer) return;

        if (State.Alive != _state) return;

        player.Damage(_characterStats.Attack);
        _coolDownTimer = _maxCoolDownTime;
    }

    /// <summary>
    /// 敵ダメージ処理
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public float Damage(float attack)
    {
        if (State.Alive != _state) return 0;

        float damage = Mathf.Max(0, attack - _characterStats.Defense);
        _characterStats.HP -= damage;

        _sceneDirector.DispDamage(gameObject, damage);

        if(0 > _characterStats.HP)
        {
            SetDead();
        }

        return damage;
    }
}
