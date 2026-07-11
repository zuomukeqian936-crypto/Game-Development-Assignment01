using UnityEngine;

public class AxeController : BaseWeapon
{
    private SoundManager _soundManager;
    private WeaponAudioSettings _weaponAudioSettings;

    void Start()
    {
        _soundManager = FindAnyObjectByType<SoundManager>();
        _weaponAudioSettings = Resources.Load<WeaponAudioSettings>("WeaponAudioSettings");
    }
    // Update is called once per frame
    void Update()
    {
        //回転処理
        transform.Rotate(new Vector3(0, 0, -1000 * Time.deltaTime));
    }

    //トリガーが衝突したときに呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _soundManager.PlaySE(_weaponAudioSettings.AxeClip, 0.3f, 0.65f);
            DefaultAttackEnemy(collision);
        }
    }
}
