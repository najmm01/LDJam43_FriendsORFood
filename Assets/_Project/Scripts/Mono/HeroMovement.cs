using UnityEngine;

public class HeroMovement : ChainMember
{
    [Header("Set in inspector")]
    public HeroData heroData;
    public Transform tailParent;

    GameManagerData _gameData;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    Tail _tail;

    float _xValue, _yValue;

    private void Start()
    {
        _gameData = GameManager.instance.data;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _tail = Tail.instance;

    }

    private void Update()
    {
        CheckForInput();
        Move();
    }

    void CheckForInput()
    {
        //set the X and Y movement values based on the input and the gridSpeed
        _xValue = Input.GetAxisRaw("Horizontal") * heroData.gridSpeed.x * Time.deltaTime;
        _yValue = Input.GetAxisRaw("Vertical") * heroData.gridSpeed.y * Time.deltaTime;

    }

    void Move()
    {
         //if hero is near edges or there was movement, don't do anything
        if (NearEdges() || (_xValue == 0 && _yValue == 0))
        {
            _animator.SetTrigger("Steady");
            _tail.StopMoving();
            return;
        }

        //update the hero position and move tail elements
        oldPos = transform.position;
        _tail.MoveTailElements(_xValue, _yValue);
        _animator.SetTrigger("Walk");
        transform.Translate(new Vector3(_xValue, _yValue, 0) );
        //flip the player sprite based on the sign of X movement
        _spriteRenderer.flipX = _xValue < 0;

    }

    bool NearEdges()
    {
        if (_xValue > 0)
        {
            if (transform.position.x + _xValue > _gameData.roomWidth)
                return true;
        }
        else if (transform.position.x + _xValue < 0)
        {
            return true;
        }

        if (_yValue > 0)
        {
            if (transform.position.y + _yValue > _gameData.roomHeight)
                return true;
        }
        else if (transform.position.y + _yValue < 0)
        {
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        Human.OnHumanDeathEvent += RemoveHuman;

    }
    private void OnDisable()
    {
        Human.OnHumanDeathEvent -= RemoveHuman;
    }

    void RemoveHuman(bool isMedic)
    {
        GameManager.instance.OnHumanDeath(isMedic);
        Tail.instance.SetupChainMembers();

    }
}
