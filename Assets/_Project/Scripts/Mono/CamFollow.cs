using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 2.0f;

    float _interpolation;
    Vector3 _pos;

    void Update()
    {
        //make the camera follow the player smoothly
        _interpolation = speed * Time.deltaTime;

        _pos = transform.position;
        _pos.y = Mathf.Lerp(transform.position.y, target.transform.position.y, _interpolation);
        _pos.x = Mathf.Lerp(transform.position.x, target.transform.position.x, _interpolation);

        transform.position = _pos;
    }
}
