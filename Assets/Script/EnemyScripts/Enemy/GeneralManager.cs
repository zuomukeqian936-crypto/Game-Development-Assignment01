using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private EnemyPool _enemyPool;

    [Header("Set Spawn Radius")]
    [SerializeField] private float _radius = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetEnemySpawnPosition();
    }

    /// <summary>
    /// 敵スポーン位置の計算
    /// </summary>
    private void SetEnemySpawnPosition()
    {
        // カメラの中心位置
        Vector3 center = Camera.main.transform.position;

        // ランダムな角度
        float angle = Random.Range(0f, Mathf.PI * 2f);

        // 円周上の位置を計算
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * _radius;

        // スポーン位置
        Vector3 spawnPos = center + offset;

        _enemyPool.EnemySpawn(spawnPos);
    }

    
}
