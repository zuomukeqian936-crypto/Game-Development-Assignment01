using UnityEngine;

public class ShieldSpawnerController : BaseWeaponSpawner
{
    
    void Update()
    {
        //オブジェクトのカウント
        _weapons.RemoveAll(item => item == null);

        //一つでも残っていたら終了
        if (0 < _weapons.Count) return;

        //全部なくなったらタイマー消化
        if (isSpawnTimerNotElapsed()) return;

        //武器生成
        for(int i = 0; i < _weaponStats._spawnCount; i++)
        {
            ShieldController ctrl = (ShieldController)CreateWeapon(transform.position,transform);

            //初期角度
            ctrl._angle = 360f / _weaponStats._spawnCount * i;
        }

        //次のタイマー
        _spawnTimer = _weaponStats.GetRandomSpawnTimer();
    }
}
