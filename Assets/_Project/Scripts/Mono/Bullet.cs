using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : PooledObject
{
    float _lifeTime;
    public void Setup(float lifeTime, float speed)
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        _lifeTime = lifeTime;
        StopAllCoroutines();
        StartCoroutine(RemoveBullet()); 
    }

    IEnumerator RemoveBullet()
    {
        //pool the bullet after its lifetime
        yield return new WaitForSeconds(_lifeTime);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //the "Bullets" physics 2D layer is set to interact with only the "Enemies" layer
        collision.transform.GetComponent<Enemy>().OnDamage();
        StopAllCoroutines();
        StartCoroutine(RemoveBullet());
    }
}
