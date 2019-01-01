using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip gunShot;
    public AudioClip foodPickUp;
    public AudioClip healthpickUp;
    public AudioClip healthPickUpFail;
    public AudioClip humanHit;
    public AudioClip enemyHit;
    public AudioClip enemyDying;
    public AudioClip humanDying;
    public AudioClip heroDying;
    public AudioClip gameOverSfx;

    public AudioSource explosionAudioSource;
    public AudioSource gunshotSource;

    AudioSource _source;
    public Dictionary<string, AudioClip> _soundDict;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _soundDict = new Dictionary<string, AudioClip>();

        _soundDict["GunShot"] = gunShot;
        _soundDict["FoodPickUp"] = foodPickUp;
        _soundDict["HealthPickUp"] = healthpickUp;
        _soundDict["FailedHealthPickUp"] = healthPickUpFail;
        _soundDict["EnemyDying"] = enemyDying;
        _soundDict["HumanDying"] = humanDying;
        _soundDict["HeroDying"] = heroDying;
        _soundDict["HumanHit"] = humanHit;
        _soundDict["EnemyHit"] = enemyHit;
        _soundDict["GameOverSFX"] = gameOverSfx;

    }

    public void PlayAudio(string key, float volume)
    {
        if (!_soundDict.ContainsKey(key) || _source == null)
        {
            return;
        }
        _source.volume = volume;
        _source.PlayOneShot(_soundDict[key]);

    }

    public void PlayExplosion(string key, float volume)
    {
        if (!_soundDict.ContainsKey(key) || _source == null)
        {
            return;
        }
        explosionAudioSource.volume = volume;
        explosionAudioSource.PlayOneShot(_soundDict[key]);

    }

    public void PlayGunShot(float volume)
    {

        gunshotSource.volume = volume;
        gunshotSource.PlayOneShot(_soundDict["GunShot"]);

    }

}
