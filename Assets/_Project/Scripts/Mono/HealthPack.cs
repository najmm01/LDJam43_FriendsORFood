using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HealthPack : MonoBehaviour
{
    public delegate void OnHealthPickup(float val);
    public static event OnHealthPickup OnHealthPickupEvent;

    public HealthData healthData;

    [Header("Set dynamically")]
    public float healthValue;

    private void Start()
    {
        //set a sprite and a value randomly on initialization
        GetComponentInChildren<SpriteRenderer>().sprite = healthData.healthSprites[Random.Range(0, healthData.healthSprites.Length)];
        healthValue = Random.Range(healthData.minValue, healthData.maxValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //HealthPack is on "Food" Physics layer and interacts only with the "Hero" and "Allies" layer
        if (GameManager.instance.medicDead)
        {
            SoundManager.instance.PlayAudio("FailedHealthPickUp", healthData.pickUpFailedVolume);
            return;
        }
        GameManager.instance.Score += 500;
        SoundManager.instance.PlayAudio("HealthPickUp", healthData.pickUpVolume);
        ParticleManager.instance.CreateParticles("HealthPickUp", transform.position);

        //Invoke the OnHealthPickupEvent and destroy the HealthPack
        OnHealthPickupEvent?.Invoke(healthValue / GameManager.instance.humansAliveCount);
        Destroy(gameObject);
    }
}
