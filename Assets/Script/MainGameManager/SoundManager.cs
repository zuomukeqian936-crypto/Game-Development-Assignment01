using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("References")]
    [SerializeField] private WeaponAudioSettings _weaponAudioSettings;

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

    /// <summary>
    /// ボタンを押したときの処理
    /// </summary>
    public void OnClickSE()
    {
        PlaySE(_weaponAudioSettings.ClickClip);
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
    public void PlaySE(AudioClip clip, float volume = 1, float pitch = 1)
    {
        GameObject seObj = GetSE();
        AudioSource audio = seObj.GetComponent<AudioSource>();

        audio.PlayOneShot(clip);
        audio.volume = volume;
        audio.pitch = pitch;

        DOVirtual.DelayedCall(clip.length, () =>
        {
            Return(seObj);
        });
    }
}

