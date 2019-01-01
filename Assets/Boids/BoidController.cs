using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidController : MonoBehaviour
{
    public float minVelocity = 5;
    public float maxVelocity = 20;
    public float randomness = 1;
    public int flockSize = 20;
    
    public GameObject chasee;

    public Vector3 flockCenter;
    public Vector3 flockVelocity;

    private List<GameObject> boids;
    private Collider2D _collider;
    private GameObject prefab;

    void Awake()
    {
        chasee =  GameManager.instance.hero;
        prefab = GameManager.instance.GenAnEnemy();
        _collider = GetComponent<Collider2D>();
        boids = new List<GameObject>();
        for (var i = 0; i < flockSize; i++)
        {
            Vector2 position = new Vector2(
                Random.value * _collider.bounds.size.x,
                Random.value * _collider.bounds.size.y
            ) - (Vector2)_collider.bounds.extents;

            GameObject boid = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            boid.transform.parent = transform;
            boid.transform.localPosition = position;
            boid.GetComponent<BoidFlocking>().SetController(gameObject);
            boids.Add(boid);
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeathEvent += RemoveBoid;

    }
    private void OnDisable()
    {
        Enemy.OnEnemyDeathEvent -= RemoveBoid;
    }

    void FixedUpdate()
    {
        Vector2 theCenter = Vector3.zero;
        Vector2 theVelocity = Vector3.zero;

        foreach (GameObject boid in boids)
        {
            theCenter = theCenter + (Vector2)boid.transform.localPosition;
            theVelocity = theVelocity + boid.GetComponent<Rigidbody2D>().velocity;
        }

        flockCenter = theCenter / (flockSize);
        flockVelocity = theVelocity / (flockSize);
    }

    void RemoveBoid(GameObject gameObject)
    {
        boids.Remove(gameObject);
        Destroy(gameObject);
    }
}