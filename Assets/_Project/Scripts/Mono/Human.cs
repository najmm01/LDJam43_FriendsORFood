using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    public GameObject hud;
    public delegate void OnHumanDeath(bool isMedic);
    public static event OnHumanDeath OnHumanDeathEvent;

    public Slider healthBar;
    private float health;

    public bool isHero;
    public bool isMedic;

    internal bool isInvincible;

    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            healthBar.value = value / 100;

            if (value <= 0)
            {
                Die();
            }
        }
    }

    private void Start()
    {
        Health = 100;
    }

    public void Die()
    {
        //disable the linked HUD for the Human
        hud.SetActive(false);

        //if the player is a Hero, call OnHeroDead on GameManager, disable it to avoid NULL reference exceptions and return
        //otherwise destroy the Human gameobject
        if (isHero)
        {
            SoundManager.instance.PlayAudio("HeroDying", 2);
            ParticleManager.instance.CreateParticles("HeroDying", transform.position);
            GameManager.instance.OnHeroDead();
            gameObject.SetActive(false);
            return;
        }
        else
        {
            SoundManager.instance.PlayAudio("HumanDying", 2);
            ParticleManager.instance.CreateParticles("HumanDying", transform.position);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            OnHumanDeathEvent?.Invoke(isMedic);
        }
       
    }

    private void OnEnable()
    {
        HealthPack.OnHealthPickupEvent += IncreaseHealth;

    }
    private void OnDisable()
    {
        HealthPack.OnHealthPickupEvent -= IncreaseHealth;
    }

    //This gets called when a healthPack is picked, which then issues an OnHealthPickupEvent
    void IncreaseHealth(float val)
    {
        Health += val;

    }

    //This method sets isInvincible to true if the gameobject is true and starts the Invincibility coroutine
    public void StartInvincibility(float time)
    {
        if(!gameObject.activeSelf)
        {
            return;
        }
        StopAllCoroutines();
        isInvincible = true;
        StartCoroutine(Invincibility(time));
    }

    //This routine animates the opacity of the Human sprite during the invinciblity period governed by duration
    private IEnumerator Invincibility(float duration)
    {
        Color _color;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            _color = sprite.color;
            _color.a = GameManager.instance.data.opacityCurve.Evaluate(t);
            sprite.color = _color;
            yield return null;
        }

        _color = sprite.color;
        _color.a = 1;
        sprite.color = _color;

        isInvincible = false;
    }
}
