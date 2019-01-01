using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Data", menuName = "SOB/Food Data", order = 3)]
public class FoodData : ScriptableObject
{
    public Sprite[] foodSprites;
    public float minValue;
    public float maxValue;
    public float pickUpVolume;
}
