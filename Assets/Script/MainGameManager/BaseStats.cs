using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseStats 
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

    //追加データのタイプ
    [Serializable] 
    public class BonusStats
    {
        public BonusType Type;
        public StatsTypes Key;
        public float Value;
    }
    /// <summary>
    /// StatsTypeと紐づけにインデクサ使用
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public float this[StatsTypes key]
    {
        get
        {
            if (key == StatsTypes.Attack) return Attack;
            else if (key == StatsTypes.Defense) return Defense;
            else if (key == StatsTypes.MoveSpeed) return MoveSpeed;
            else if (key == StatsTypes.HP) return HP;
            else if (key == StatsTypes.MaxHp) return MaxHP;
            else if (key == StatsTypes.XP) return XP;
            else if (key == StatsTypes.MaxXP) return MaxXP;
            else if (key == StatsTypes.PickUpRange) return PickUpRange;
            else if (key == StatsTypes.AliveTime) return AliveTime;
            else return 0;
        }

        set
        {
            if (key == StatsTypes.Attack) Attack = value;
            else if (key == StatsTypes.Defense) Defense = value;
            else if (key == StatsTypes.MoveSpeed) MoveSpeed = value;
            else if (key == StatsTypes.HP) HP = value;
            else if (key == StatsTypes.MaxHp) MaxHP = value;
            else if (key == StatsTypes.XP) XP = value;
            else if (key == StatsTypes.MaxXP) MaxXP = value;
            else if (key == StatsTypes.PickUpRange) PickUpRange = value;
            else if (key == StatsTypes.AliveTime) AliveTime = value;
        }
    }
    //ボーナス値の計算
    protected float applyBonus(float currentValue, float value, BonusType type)
    {
        if(BonusType.Bonus == type)
        {
            return currentValue * value;
        }
        else if(BonusType.Boost == type)
        {
            return currentValue * (1 + value) ;
        }
        return currentValue;
    }

    //ボーナス追加
    protected void addBonue(BonusStats bonus)
    {
        float value = applyBonus(this[bonus.Key], bonus.Value, bonus.Type);

        if(StatsTypes.HP == bonus.Key)
        {
            value = Mathf.Clamp(value, 0, MaxHP);
        }
        else if(StatsTypes.XP == bonus.Key)
        {
            value = Mathf.Clamp(value, 0, MaxXP);
        }

        this[bonus.Key] = value;
    }
    //コピーしたデータを返す
    public BaseStats GetCopy()
    {
        return (BaseStats)MemberwiseClone();
    }

}
