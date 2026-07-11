using UnityEngine;

public class ShurikenController : BaseWeapon
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
        //回転
        transform.Rotate(new Vector3(0, 0, 1000 * Time.deltaTime));

        //移動
        _rb2D.position += _forward * _weaponSpawnerStats.MoveSpeed * Time.deltaTime;
    }

    //トリガーが衝突したときに呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            _soundManager.PlaySE(_weaponAudioSettings.ShurikenClip, 0.3f, 0.8f);
            DefaultAttackEnemy(collision);
        }
    }
}
