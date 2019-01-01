using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData Data", menuName = "SOB/EnemyData Data", order = 5)]
public class EnemyData : ScriptableObject
{
    public float minDamage;
    public float maxDamage;
    public float minHealth;
    public float maxHealth;
    public float bulletDamage;

    public float hitVolume, deathVolume, humanHitVolume;
}
