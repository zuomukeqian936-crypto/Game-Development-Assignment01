using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawnerController : BaseWeaponSpawner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (isSpawnTimerNotElapsed()) return;

        //次のタイマー
        _spawnTimer = _weaponStats.GetRandomSpawnTimer();

        //敵がいない
        if (1 > _enemySpawner.GetEnemies().Count) return;

        for(int i = 0; i < (int)_weaponStats._spawnCount; i++)
        {
            //武器生成
            ArrowController ctrl = (ArrowController)CreateWeapon(transform.position);

            //ランダムでターゲットを設定
            List<EnemyController> enemies = _enemySpawner.GetEnemies();
            int rnd = Random.Range(0, enemies.Count);
            EnemyController target = enemies[rnd];

            ctrl._target = target;
        }
    }
}
