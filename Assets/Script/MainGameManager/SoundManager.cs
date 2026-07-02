using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //シングルトン
    public static SoundManager Instance;

    //再生装置
    private AudioSource _audioSource;

    [SerializeField] private List<AudioClip> _audioClipsBGM;
    [SerializeField] private List<AudioClip> _audioClipSE;

    private void Awake()
    {
        //セット
        if(null == Instance)
        {
            //オーディオ設定
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;

            //オブジェクトをセットする
            Instance = this;

            //シーンをまたいでもオブジェクトを削除しない
            DontDestroyOnLoad(this.gameObject);
        }

        //２回目以降に生成されたオブジェクトを削除する
        else
        {
            Destroy(this.gameObject);
        }
    }

    //BGM再生
    public void PlayBGM(int index)
    {
        _audioSource.clip = _audioClipsBGM[index];
        _audioSource.Play();
    }

    //SE再生
    public void PlaySE(int index)
    {
        _audioSource.PlayOneShot(_audioClipSE[index]);
    }
}
