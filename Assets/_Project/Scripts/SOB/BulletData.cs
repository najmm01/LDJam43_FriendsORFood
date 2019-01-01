using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Data Data", menuName = "SOB/Bullet Data", order = 4)]
public class BulletData : ScriptableObject
{

    public float maxBulletSpeed, minBulletSpeed;
    public float bulletLifeTime;
    public float shotVolume;

}
