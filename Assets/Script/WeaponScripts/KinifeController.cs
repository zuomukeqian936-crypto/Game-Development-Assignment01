using UnityEngine;

public class KinifeController : BaseWeapon
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //角度に変換
        float angle = Mathf.Atan2(_forward.y, _forward.x) * Mathf.Rad2Deg;
        //角度を代入
        transform.rotation = Quaternion.Euler(0, 0, angle + 132);
    }

    // Update is called once per frame
    void Update()
    {
        _rb2D.position += _forward * _weaponSpawnerStats.MoveSpeed * Time.deltaTime;
    }

    //トリガーが衝突したときに呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DefaultAttackEnemy(collision);
    }
}
