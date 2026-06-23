using UnityEngine;

public class AxeController : BaseWeapon
{
    // Update is called once per frame
    void Update()
    {
        //回転処理
        transform.Rotate(new Vector3(0, 0, -1000 * Time.deltaTime));
    }

    //トリガーが衝突したときに呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        attackEnemy(collision);
    }
}
