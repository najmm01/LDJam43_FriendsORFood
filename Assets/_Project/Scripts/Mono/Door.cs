using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Set Dynamically")]
    public int linkedSpawnIndex;

    CircleCollider2D _collider;

    private void Start()
    {
        //if door is open(has a collider), set it up
        if(_collider = GetComponent<CircleCollider2D>())
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
            GetComponent<CircleCollider2D>().radius = 8;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //"Doors" physics 2D layer only interacts with the "Hero" layer
        DungeonGenerator.instance.NextLevel(linkedSpawnIndex);
    }


}
