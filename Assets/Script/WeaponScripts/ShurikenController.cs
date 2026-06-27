using UnityEngine;

public class ShurikenController : BaseWeapon
{

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
        DefaultAttackEnemy(collision);
    }
}
