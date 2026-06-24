using UnityEngine;

public class BoomerangSpawnerController : BaseWeaponSpawner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //武器生成
        for(int i = 0; i < _weaponStats._spawnCount; i++)
        {
            CreateWeapon(transform.position);
        }

        _spawnTimer = _weaponStats.GetRandomSpawnTimer();
    }
}
