using UnityEngine;
using System.Collections.Generic;

public class AudioSourcePool : MonoBehaviour
{
    [SerializeField] private SEObjectController prefab;
    [SerializeField] private int poolSize = 10;

    private Queue<SEObjectController> pool = new Queue<SEObjectController>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public void PlaySE(AudioClip clip)
    {
        var obj = pool.Dequeue();
        obj.Play(clip, ReturnToPool);
    }

    private void ReturnToPool(SEObjectController obj)
    {
        pool.Enqueue(obj);
    }
}


