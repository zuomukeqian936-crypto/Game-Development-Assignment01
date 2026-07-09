//using JetBrains.Annotations;
//using NUnit.Framework;
//using System.Collections.Generic;
//using UnityEngine;

//public class SoundManager : MonoBehaviour
//{
//    //シングルトン
//    public static SoundManager Instance;

//    //再生装置
//    private AudioSource _audioSource;

//    [Header("SE Prefab")]
//    [SerializeField] private GameObject _sePrefab;

//    [Header("SE Prefab Count")]
//    [SerializeField] private int _initialSize = 20;
//    private Queue<GameObject> _sePool = new Queue<GameObject>(); 

//    private void Awake()
//    {
//        Instance = this;

//        for(int i = 0; i < _initialSize; i++)
//        {
//            GameObject seObject = Instantiate(_sePrefab);
//            _sePool.Enqueue(seObject);
//        }
//    }

//    //SEPoolの在庫確認処理
//    public GameObject GetSE()
//    {
//        if(_sePool.Count > 0)
//        {
//            GameObject seObject = _sePool.Dequeue();
//            seObject.SetActive(false);
//            return seObject;
//        }
//        else
//        {
//            GameObject seObject = Instantiate(_sePrefab);
//            seObject.SetActive(true);
//            return seObject;
//        }
//    }

//    /// <summary>
//    /// SEPoolに戻す処理
//    /// 
//    /// </summary>
//    /// <param name="seObject"></param>
//    public void Return(GameObject seObject)
//    {
//        seObject.SetActive(false);
//        _sePool.Enqueue(seObject);
//    }

//    //SE再生
//    public void PlaySE(AudioClip)
//    {
//    }
//}

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("SE Prefab")]
    [SerializeField] private GameObject _sePrefab;

    [Header("SE Prefab Count")]
    [SerializeField] private int _initialSize = 20;

    private Queue<GameObject> _sePool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        // 初期プール生成
        for (int i = 0; i < _initialSize; i++)
        {
            GameObject seObject = Instantiate(_sePrefab);
            seObject.SetActive(false);
            _sePool.Enqueue(seObject);
        }
    }

    // SE をプールから取得
    private GameObject GetSE()
    {
        if (_sePool.Count > 0)
        {
            GameObject seObject = _sePool.Dequeue();
            seObject.SetActive(true);
            return seObject;
        }
        else
        {
            // 足りない場合は追加生成
            GameObject seObject = Instantiate(_sePrefab);
            seObject.SetActive(true);
            return seObject;
        }
    }

    // SE をプールに返却
    public void Return(GameObject seObject)
    {
        seObject.SetActive(false);
        _sePool.Enqueue(seObject);
    }

    /// <summary>
    /// SE 再生（プール版）
    /// </summary>
    public void PlaySE(AudioClip clip)
    {
        GameObject seObj = GetSE();
        AudioSource audio = seObj.GetComponent<AudioSource>();

        audio.PlayOneShot(clip);

        DOVirtual.DelayedCall(clip.length, () =>
        {
            Return(seObj);
        });
    }
}

