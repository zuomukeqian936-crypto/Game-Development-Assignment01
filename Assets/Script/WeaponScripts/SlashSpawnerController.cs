using UnityEngine;

public class SlashSpawnerController : BaseWeaponSpawner
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

        Vector3 pos = transform.position;
        pos.x += 2f * dir;

        //生成
        SlashController ctrl = (SlashController)CreateWeapon(pos, transform);

        //SoundManager.Instance.PlaySE(1);

        //角度を変える処理
        ctrl.transform.eulerAngles = ctrl.transform.eulerAngles * dir;

        //次回の生成タイマー
        _spawnTimer = _onceSpawnTime;
        _onceSpawnCount--;

        //生成が終わったらリセット
        if(1 > _onceSpawnCount)
        {
            _spawnTimer = _weaponStats.GetRandomSpawnTimer();
            _onceSpawnCount = (int)_weaponStats._spawnCount;
        }
    }
}
