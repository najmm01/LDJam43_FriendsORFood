using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Food : MonoBehaviour
{
    public FoodData foodData;

    [Header("Set dynamically")]
    public float foodValue;

    private void Start()
    {
        //set a sprite and a value randomly on initialization
        GetComponentInChildren<SpriteRenderer>().sprite = foodData.foodSprites[Random.Range(0, foodData.foodSprites.Length)];
        foodValue = Random.Range(foodData.minValue, foodData.maxValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //the food is on "Food" Physics 2D layer and interacts only with the "Hero" and "Allies" layer
        GameManager.instance.Score += 200;
        SoundManager.instance.PlayAudio("FoodPickUp", foodData.pickUpVolume);
        ParticleManager.instance.CreateParticles("FoodPickUp", transform.position);
        GameManager.instance.ReduceHunger(foodValue);
        Destroy(gameObject);
    }
}
