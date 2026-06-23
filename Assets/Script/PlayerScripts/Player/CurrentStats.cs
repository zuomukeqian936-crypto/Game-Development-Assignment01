using UnityEngine;

public class CurrentStats : MonoBehaviour
{
    public string Title;
    public int Id;
    public int Lv;
    public string Name;

    [TextArea] public string Description;
    //攻撃力
    public float CurrentAttack;
    //防御力
    public float CurrentDefense;
    //体力
    public float CurrentHP;
    //体力最大
    public float CurrentMaxHP;
    //経験値
    public float CurrentXP;
    //経験値最大
    public float CurrentMaxXP;
    //移動速度
    public float CurrentMoveSpeed;
    //経験値所得範囲
    public float CurrentPickUpRange;
    //生存時間
    public float CurrentAliveTime;
}
