using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticlesComponent foodPickUp;
    public ParticlesComponent healthpickUp;
    public ParticlesComponent enemyDying;
    public ParticlesComponent humanDying;
    public ParticlesComponent heroDying;
    public ParticlesComponent humanHit;
    public ParticlesComponent enemyHit;


    Dictionary<string, ParticlesComponent> _particlesDict;

    public static ParticleManager instance;

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
        _particlesDict = new Dictionary<string, ParticlesComponent>();

        _particlesDict["FoodPickUp"] = foodPickUp;
        _particlesDict["HealthPickUp"] = healthpickUp;
        _particlesDict["EnemyDying"] = enemyDying;
        _particlesDict["HumanDying"] = humanDying;
        _particlesDict["HeroDying"] = heroDying;
        _particlesDict["HumanHit"] = humanHit;
        _particlesDict["EnemyHit"] = enemyHit;
    }

    public void CreateParticles(string key, Vector2 pos, bool pooled = false)
    {
        //if the key is present, play the corresponding particle effect
        if(!_particlesDict.ContainsKey(key))
        {
            return;
        }
        if(pooled)
        {
            var _pS = _particlesDict[key].GetPooledInstance<ParticlesComponent>();
            _pS.transform.position = pos;
            _pS.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Instantiate(_particlesDict[key], pos, Quaternion.identity);
        }

    }
}
