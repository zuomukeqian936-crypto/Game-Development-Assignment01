using UnityEngine;

public class ArrowController : BaseWeapon
{

    public EnemyController _target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //方向
        Vector2 forward = _target.transform.position - transform.position;

        //角度に変換
        float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
        //角度を代入
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットがいないときの処理
        if (!_target)
        {
            Destroy(gameObject);
            return;
        }

        //移動
        Vector2 forward = _target.transform .position - transform.position;
        _rb2D.position += forward.normalized * _weaponSpawnerStats.MoveSpeed * Time.deltaTime;
    }

    //トリガーが突破したとき
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敵以外
        if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        //通常ダメージ
        float attack = _weaponSpawnerStats.Attack;

        //敵と衝突
        if(_target == enemy)
        {
            _target = null;
        }

        //ターゲット以外
        else
        {
            attack /= 3;
        }

        AttackEnemy(collision, attack);
    }
}
