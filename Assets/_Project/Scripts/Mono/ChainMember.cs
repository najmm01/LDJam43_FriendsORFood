using UnityEngine;

public class ChainMember : MonoBehaviour
{
    [Header("Set Dynamically")]
    public ChainMember leader; //this stores the reference to the transform of what this chainMember is supposed to follow
  
    internal Vector2 oldPos;  //this stores the previous position if the position has been updated

}
