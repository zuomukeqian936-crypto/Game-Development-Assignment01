using UnityEngine;

public class AxeSpawnerController : BaseWeaponSpawner
{
    [Header("Spawn Settings")]
    [SerializeField] private int _onceSpawnCount;
    [SerializeField] private float _onceSpawnTime = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _onceSpawnCount = (int)StatsTypes.SpawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttack();
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void HandleAttack()
    {
        //タイマー管理
        if (isSpawnTimerNotElapsed()) return;

        int dir = (_onceSpawnCount % 2 == 0) ? 1 : -1;

        //生成
        AxeController ctrl = (AxeController)CreateWeapon(transform.position);

        //斜めに力を加える処理
        ctrl.GetComponent<Rigidbody2D>().AddForce(new Vector2(100 * dir, 350));

        //次回の生成タイマー
        _spawnTimer = _onceSpawnTime;
        _onceSpawnCount--;

        //生成が終わったらリセット
        if (1 > _onceSpawnCount)
        {
            _spawnTimer = _weaponStats.GetRandomSpawnTimer();
            _onceSpawnCount = (int)_weaponStats._spawnCount;
        }
    }
}
