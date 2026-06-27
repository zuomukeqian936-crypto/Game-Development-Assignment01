using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected BaseWeaponSpawner _baseWeaponSpawner;

    protected WeaponSpawnerStats _weaponSpawnerStats;

    protected Rigidbody2D _rb2D;
    protected Vector2 _forward; //方向
    
    //初期化
    public void Init(BaseWeaponSpawner spawner, Vector2 forward)
    {
        this._baseWeaponSpawner = spawner;
        this._weaponSpawnerStats = (WeaponSpawnerStats)spawner._weaponStats.GetCopy();
        this._forward = forward;
        this._rb2D = GetComponent<Rigidbody2D>();

        //生成時間があれば設定する
        if(-1 < _weaponSpawnerStats.AliveTime)
        {
            Destroy(gameObject, _weaponSpawnerStats.AliveTime);
        }
    }

    //敵へ攻撃処理
    protected void AttackEnemy(Collider2D collider2d, float attack)
    {
        if (!collider2d.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        //攻撃
        float damage = enemy.Damage(attack);
        //総ダメージ計算
        _baseWeaponSpawner._totalDamage += damage;

        //HP設定があれば自分もダメージ
        if (0 > _weaponSpawnerStats.HP) return;
        _weaponSpawnerStats.HP--;
        if (0 > _weaponSpawnerStats.HP) Destroy(gameObject);
    }

    //敵へ攻撃（デフォルト攻撃）
    protected void DefaultAttackEnemy(Collider2D collider2d)
    {
        AttackEnemy(collider2d, _weaponSpawnerStats.Attack);
    }
}
