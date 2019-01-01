using UnityEngine;

public class CrosshairHandler : MonoBehaviour
{
    public Transform crossHair;

    Vector2 _pos;
    Camera _cam;
    private void Start()
    {      
        _cam = Camera.main;
        Cursor.visible = true;
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
    }

    void Update()
    {
        // make the crosshair sprite follow the mouse position
        //the casting to Vector2 is to set the Z value on RHS to 0
        crossHair.position = (Vector2)_cam.ScreenToWorldPoint(Input.mousePosition);

    }

    void OnApplicationFocus(bool hasFocus)
    {
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
    }
}
