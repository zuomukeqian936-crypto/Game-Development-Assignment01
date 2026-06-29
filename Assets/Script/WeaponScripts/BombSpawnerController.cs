using UnityEngine;
using UnityEngine.UIElements;

public class BombSpawnerController : BaseWeaponSpawner
{
    
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //生成される場所
        Vector2 position = Camera.main.transform.position;
        //カメラの上から
        position.y += Camera.main.orthographicSize;

        for(int i = 0; i < _weaponStats._spawnCount; i++)
        {
            position.x += Random.Range(-7, 7);
            CreateWeapon(position);
        }

        _spawnTimer = _weaponStats.GetRandomSpawnTimer();
    }
}
