using UnityEngine;

public class BoomerangController : BaseWeapon
{
    private SoundManager _soundManager;
    private WeaponAudioSettings _weaponAudioSettings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _soundManager = FindAnyObjectByType<SoundManager>();
        _weaponAudioSettings = Resources.Load<WeaponAudioSettings>("WeaponAudioSettings");

        //ランダムな方向に向かって飛ばす
        _forward = new Vector2 (Random.Range(-1f,-1f), Random.Range(-1f,1f));
        _forward.Normalize ();

        //飛ばす処理
        Vector2 force = new Vector2(-_forward.x * _weaponSpawnerStats.MoveSpeed, -_forward.y * _weaponSpawnerStats.MoveSpeed);
        _rb2D.AddForce(force);
    }

    // Update is called once per frame
    void Update()
    {
        //回転処理
        transform.Rotate(new Vector3(0,0, 5000 * Time.deltaTime));

        //移動処理
        _rb2D.AddForce(_forward * _weaponSpawnerStats.MoveSpeed * Time.deltaTime);
    }

    //トリガーが衝突したときに呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _soundManager.PlaySE(_weaponAudioSettings.BoomerangClip, 0.3f, 1f);
            DefaultAttackEnemy(collision);
        }
        
    }
}
