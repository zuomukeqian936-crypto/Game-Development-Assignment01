using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;

public class BombController : BaseWeapon
{
    enum State
    {
        Bomb,
        Explosion,
        DamageFloor,
        Destroy,
    }

    private State _state;

    private Animator _animator;
    private Dictionary<State, float> _animationTimer;
    private float _damageFloorCoolDownTime = 0.5f;
    private Dictionary<EnemyController, float> _damageFloorTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化
        _animationTimer = new Dictionary<State, float>();
        _damageFloorTimer = new Dictionary<EnemyController, float>();
        _animator = GetComponent<Animator>();

        //爆弾時
        _animationTimer.Add(State.Bomb, Random.Range(0.5f, 1.5f));
        //爆発時
        _animationTimer.Add(State.Explosion, 0.66f);
        //ダメージフロア
        _animationTimer.Add(State.DamageFloor, 30f);

        //初期状態
        _state = State.Bomb;
    }

    // Update is called once per frame
    void Update()
    {
        if (_animationTimer.ContainsKey(_state))
        {
            _animationTimer[_state] -= Time.deltaTime;
            if(0 > _animationTimer[_state])
            {
                ChangeState(++_state);
            }
        }
    }

    private void ChangeState(State next)
    {
        //爆発
        if(State.Explosion == next)
        {
            _animator.SetTrigger("isExplosion");
            _rb2D.gravityScale = 0;
            _rb2D.linearVelocity = Vector2.zero;
        }

        //ダメージフロア
        else if(State.DamageFloor == next)
        {
            _animator.SetTrigger("isDamageFloor");
            GetComponent<SpriteRenderer>().sortingOrder = 2;
        }

        //終了
        else if(State.Destroy == next)
        {
            Destroy(gameObject);
        }

        //現在の状態
        _state = next;
    }

    //衝突時
    private void OnTriggereEnter2D(Collider2D collision)
    {
        //敵以外
        if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        //爆弾
        if(State.Bomb == _state)
        {
            DefaultAttackEnemy(collision);
            ChangeState(State.Explosion);
        }

        //爆発中
        else if(State.Explosion == _state)
        {
            DefaultAttackEnemy(collision);
        }
    }

    //衝突している間
    private void OnTriggerStay2D(Collider2D collision)
    {
        //ダメージフロアじゃない
        if (State.DamageFloor != _state) return;

        //敵以外
        if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        //ターゲットのタイマーをセット
        _damageFloorTimer.TryAdd(enemy, _damageFloorCoolDownTime);

        //タイマーを消化
        _damageFloorTimer[enemy] -= Time.deltaTime;

        //一定時間でダメージ
        if(0 > _damageFloorTimer[enemy])
        {
            AttackEnemy(collision, _weaponSpawnerStats.Attack / 3f);
            _damageFloorTimer[enemy] = _damageFloorCoolDownTime; 
        }
    }
}
