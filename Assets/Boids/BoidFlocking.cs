using UnityEngine;
using System.Collections;

public class BoidFlocking : MonoBehaviour
{
    private GameObject Controller;
    private bool inited = false;
    private float minVelocity;
    private float maxVelocity;
    private float randomness;
    private GameObject chasee;
    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine("BoidSteering");
    }

    IEnumerator BoidSteering()
    {
        while (true)
        {
            if (inited)
            {
                rigidBody.velocity = rigidBody.velocity + Calc() * Time.deltaTime;

                // enforce minimum and maximum speeds for the boids
                float speed = rigidBody.velocity.magnitude;
                if (speed > maxVelocity)
                {
                    rigidBody.velocity = rigidBody.velocity.normalized * maxVelocity;
                }
                else if (speed < minVelocity)
                {
                    rigidBody.velocity = rigidBody.velocity.normalized * minVelocity;
                }
            }

            float waitTime = Random.Range(0.3f, 0.5f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector2 Calc()
    {
        Vector2 randomize = new Vector2((Random.value * 2) - 1, (Random.value * 2) - 1);

        randomize.Normalize();
        BoidController boidController = Controller.GetComponent<BoidController>();
        Vector2 flockCenter = boidController.flockCenter;
        Vector2 flockVelocity = boidController.flockVelocity;
        Vector2 follow = chasee.transform.localPosition;

        flockCenter = flockCenter - (Vector2)transform.localPosition;
        flockVelocity = flockVelocity - rigidBody.velocity;
        follow = follow - (Vector2)transform.localPosition;

        return (flockCenter + flockVelocity + follow * 2 + randomize * randomness);
    }

    public void SetController(GameObject theController)
    {
        Controller = theController;
        BoidController boidController = Controller.GetComponent<BoidController>();
        minVelocity = boidController.minVelocity;
        maxVelocity = boidController.maxVelocity;
        randomness = boidController.randomness;
        chasee = boidController.chasee;
        inited = true;
    }
}