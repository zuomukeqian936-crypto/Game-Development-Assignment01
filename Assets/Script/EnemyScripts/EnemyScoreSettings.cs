using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScoreSettings", menuName = "Scriptable Objects/Score Settings")]
public class EnemyScoreSettings : ScriptableObject
{
    [Header("Enemy Score Settings"),Tooltip("敵討伐時のスコア設定")]
    public float SpriteScore = 5;
    public float SlimeScore = 10;
    public float ZombieScore = 20;
    public float GhostScore = 30;
    public float SkeletonScore = 40;
    public float FlyingBookScore = 50;
    public float BossScore = 100;

    [Header("Time Score Settings"), Tooltip("時間経過のスコア設定")]
    public float EarlyScore = 3.5f;
    public float MidScore = 5.7f;
    public float LastScore = 7.3f;
}
