using UnityEngine;

public class SEObjectController : MonoBehaviour
{
    private AudioSource audioSource;
    private System.Action<SEObjectController> onFinished;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // プールから取り出された時に呼ばれる
    public void Play(AudioClip clip, System.Action<SEObjectController> onFinished)
    {
        this.onFinished = onFinished;

        audioSource.clip = clip;
        audioSource.Play();

        gameObject.SetActive(true);
    }

    void Update()
    {
        // 再生が終わったらプールに返す
        if (!audioSource.isPlaying)
        {
            onFinished?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}

