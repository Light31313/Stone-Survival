using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyBlueprint : ScriptableObject
{
    public EnemyType type;
    public float attackDamage;
    public float health;
    public float speed;
    public float knockTime;
    public float knockRes;
    public float knockPower;
    public int experience;
    public GameObject dropItem;
    public float dropRate;
}
