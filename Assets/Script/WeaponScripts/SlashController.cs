using Unity.VisualScripting;
using UnityEngine;

public class SlashController : BaseWeapon
{
    //トリガーが衝突したときに呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        attackEnemy(collision);
    }
}
