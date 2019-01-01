using UnityEngine;

public class Firearm : MonoBehaviour
{
    public Transform muzzle;
    public Bullet bulletPrefab;
    public BulletData bulletData;
    public ParticleSystem gunShot;

    Bullet _bullet;

    private void Update()
    {
        RotateGun();
        CheckForFire();
    }

    void RotateGun()
    {
        //make the gun point towards mouse cursor
        var pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = (pos - (Vector2)transform.position).normalized;
    }

    void CheckForFire()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            //play the gunshot sound
            SoundManager.instance.PlayGunShot(bulletData.shotVolume);
            gunShot.Play();

            //Create the bullet object as a pooled object with a random velocity at the muzzle transform
            _bullet = bulletPrefab.GetPooledInstance<Bullet>();
            _bullet.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
            _bullet.Setup(bulletData.bulletLifeTime, Random.Range(bulletData.minBulletSpeed, bulletData.maxBulletSpeed));
        }
    }

}
