using UnityEngine;

public class KinifeSpawnerController : BaseWeaponSpawner
{
    [Header("Spawn Settings")]
    [SerializeField] private int _onceSpawnCount;
    [SerializeField] private float _onceSpawnTime = 0.3f;

    PlayerController _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _onceSpawnCount = (int)_weaponStats._spawnCount;
        _player = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //武器生成
        KinifeController ctrl = (KinifeController)CreateWeapon(transform.position, _player._forward);

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
