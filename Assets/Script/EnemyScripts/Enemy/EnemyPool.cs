using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyInistantiateTotalCount _enemyCount;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject _enemyPrefabs;

    public List<GameObject> _pool = new List<GameObject>();
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        StartCoroutine(CreatePool());
    }


    /// <summary>
    /// 敵キャラクタ生成処理（10体生成ごとに画面更新処理）　
    /// 始まって画面停止を起こさないための処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreatePool()
    {
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));

        for (int i = 0; i < _enemyCount.enemyCount; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefabs, spawnPos, Quaternion.identity);
            enemy.SetActive(false);
            _pool.Add(enemy);

            if (i % 10 == 0)
            {
                yield return null;
            }          
        }
       //Debug.Log(_enemyPool.Count);
    }

    /// <summary>
    /// 敵をスポーンさせる処理　（GeneralManagerからpositionを取得）
    /// </summary>
    /// <param name="spawnPos"></param>
    public void EnemySpawn(Vector3 spawnPos)
    {
        foreach (GameObject enemy in _pool)
        {
            if(enemy != _enemyPrefabs.activeSelf)
            {
                transform.position = spawnPos;
                enemy.SetActive(true);
                return;
            }
        } 
    }
}
