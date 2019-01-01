using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailElement : ChainMember
{
    float _spacing;
    float _speed;
    Vector2 _targetPos;
    GameManagerData _gameData;
    Animator _animator;

    private void Start()
    {
        _gameData = GameManager.instance.data;
        _animator = GetComponent<Animator>();
    }

    public void Move(float xVal, float yVal)
    {
        _spacing = _gameData.chainSpacing;
        _speed = _gameData.allyLerpSpeed;

        //calculate the targetPos and then call NearEdges to check if movement is allowed       
        _targetPos = transform.position;
        if (xVal == 0)
            _targetPos.x = leader.oldPos.x;
        else if (xVal > 0)
            _targetPos.x = leader.oldPos.x - _spacing;
        else if (xVal < 0)
            _targetPos.x = leader.oldPos.x + _spacing;

        if (yVal == 0)
            _targetPos.y = leader.oldPos.y;
        else if (yVal > 0)
            _targetPos.y = leader.oldPos.y - _spacing;
        else if (yVal < 0)
            _targetPos.y = leader.oldPos.y + _spacing;

        if (NearEdges())
        {
            StopMoving();
            return;
        }

        //since movement is allowed, therefore store the previous position in oldPos and update the current position to targetPos
        _animator.SetTrigger("Walk");
        oldPos = transform.position;
        transform.position = Vector2.Lerp(transform.position, _targetPos, _speed * Time.deltaTime);

    }

    public void StopMoving()
    {
        _animator.SetTrigger("Steady");
    }

    bool NearEdges()
    {

        if (_targetPos.x > _gameData.roomWidth
            || _targetPos.x < 0
            || _targetPos.y > _gameData.roomHeight
            || _targetPos.y < 0)
        {
            return true;
        }

        return false;
    }
}
