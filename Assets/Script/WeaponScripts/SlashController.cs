using Unity.VisualScripting;
using UnityEngine;

public class SlashController : BaseWeapon
{
    private SoundManager _soundManager;
    private WeaponAudioSettings _audioSettings;

    void Start()
    {
        if (_soundManager == null)
        {
            _soundManager = FindAnyObjectByType<SoundManager>();
        }

        if (_audioSettings == null)
            if (_audioSettings == null)
            {
                _audioSettings = Resources.Load<WeaponAudioSettings>("WeaponAudioSettings");
            }
    }

    //トリガーが衝突したときに呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _soundManager.PlaySE(_audioSettings.SlashClip, 0.3f);
            DefaultAttackEnemy(collision);
        }
    }
}
