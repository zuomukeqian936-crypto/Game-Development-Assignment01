using UnityEngine;

public class PinwheelSpawnerController : BaseWeaponSpawner
{

    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //武器生成
        for(int i = 0; i <_weaponStats._spawnCount; i++)
        {
            CreateWeapon(transform.position);
        }

        _spawnTimer = _weaponStats.GetRandomSpawnTimer();
    }
}
