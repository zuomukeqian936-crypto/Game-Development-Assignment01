using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyInitialStats : ScriptableObject
{
    public float hp = 100;
    public float speed = 5f;
    public int Attack = 5;
}
