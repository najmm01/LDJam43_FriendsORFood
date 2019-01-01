using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public EnemyData data;
    internal float damageValue;
    internal float healthValue;

    public delegate void OnEnemyDeath(GameObject boid);
    public static event OnEnemyDeath OnEnemyDeathEvent;

    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    Human _human;

    private void Start()
    {
        damageValue = Random.Range(data.minDamage, data.maxDamage);
        healthValue = Random.Range(data.minHealth, data.maxHealth);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _spriteRenderer.flipX = _rigidbody2D.velocity.x > 0;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the collision is with a Human and is the human is not invincible, play human hit sound, play blood particle effect & decrease Human's health      
        if (_human = collision.transform.GetComponent<Human>())
        {
            if (_human.isInvincible)
            {
                return;
            }
            SoundManager.instance.PlayAudio("HumanHit", data.humanHitVolume);
            ParticleManager.instance.CreateParticles("HumanHit", _human.transform.position);
            _human.Health -= damageValue;
        }
    }

    public void OnDamage()
    {
        //update player score, play enemy hit sound, play particle effect and decrease enemy health
        GameManager.instance.Score += 50;
        SoundManager.instance.PlayExplosion("EnemyHit", data.hitVolume);
        ParticleManager.instance.CreateParticles("EnemyHit", transform.position, true);
        healthValue -= data.bulletDamage;
        
        //if health goes beneath zero, kill the enemy 
        if (healthValue <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //update player score, play enemy death audio and invoke the enemy death event 
        GameManager.instance.Score += 200;
        SoundManager.instance.PlayExplosion("EnemyDying", data.deathVolume);
        ParticleManager.instance.CreateParticles("EnemyDying", transform.position, true);
        OnEnemyDeathEvent?.Invoke(gameObject);

    }

}
