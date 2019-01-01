using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Data", menuName = "SOB/Health Data", order = 6)]
public class HealthData : ScriptableObject
{
    public Sprite[] healthSprites;
    public float minValue;
    public float maxValue;
    public float pickUpVolume;
    public float pickUpFailedVolume;
}
