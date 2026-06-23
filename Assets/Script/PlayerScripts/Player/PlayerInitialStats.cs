using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInitialStats", menuName = "Scriptable Objects/PlayerInitialStats")]
public class PlayerInitialStats : ScriptableObject
{
    public string Title;
    public int Id;
    public int Lv;
    public string Name;

    [TextArea] public string Description;
    //攻撃力
    public float Attack;
    //防御力
    public float Defense;
    //体力
    public float HP;
    //体力最大
    public float MaxHP;
    //経験値
    public float XP;
    //経験値最大
    public float MaxXP;
    //移動速度
    public float MoveSpeed;
    //経験値所得範囲
    public float PickUpRange;
    //生存時間
    public float AliveTime;
}
