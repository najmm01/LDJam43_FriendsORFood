using UnityEngine;

public class Tail : MonoBehaviour
{
    public GameManagerData data;
    public Transform heroTransform;

    TailElement[] _tailElements;
    int _tailIndex, _tailLength;

    public static Tail instance;

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

    public void SetupChainMembers()
    {
       
        //populate chain members
        _tailElements = GetComponentsInChildren<TailElement>();
        _tailLength = _tailElements.Length;
        if (_tailLength == 0)
        {
            return;
        }

        //set the first chain member's leader as hero's transform
        _tailElements[0].leader = heroTransform.GetComponent<ChainMember>();
        Vector2 currentPos = heroTransform.position;
        _tailElements[0].transform.position = currentPos - Vector2.right * data.chainSpacing;
        currentPos = _tailElements[0].transform.position;
        for (int i = 1; i < _tailLength; i++)
        {
            _tailElements[i].leader = _tailElements[i - 1];
            _tailElements[i].transform.position = currentPos - Vector2.right * data.chainSpacing;
            currentPos = _tailElements[i].transform.position;
        }
    }

    public void MoveTailElements(float _xValue, float _yValue)
    {

        for (_tailIndex = 0; _tailIndex < _tailLength; _tailIndex++)
        {
            _tailElements[_tailIndex].Move(_xValue, _yValue);
        }
    }

    public void StopMoving()
    {

        for (_tailIndex = 0; _tailIndex < _tailLength; _tailIndex++)
        {
            _tailElements[_tailIndex].StopMoving();
        }
    }

}
