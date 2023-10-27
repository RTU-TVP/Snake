using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SnakePart : MonoBehaviour
{
    [SerializeField] private int _cellSize;

    [field:SerializeField] public PathNode _currentNode { get; private set; }
    [field:SerializeField] public PathNode _targetNode { get; private set; }

    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _position;
    [SerializeField] private bool _isMoving;
    [SerializeField] public Vector3 TargetPosition { get; private set; }
    [SerializeField] public SnakePart child;
    
    private CustomGrid<PathNode> _grid;
    
    public void Setter(CustomGrid<PathNode> grid,  PathNode currentNode,PathNode targetNode, int cellSize, Vector2 pos, float speed)
    {
        _currentNode = currentNode;
        _targetNode = targetNode;
        _cellSize = cellSize;
        _position = pos;
        _speed = speed;
        _grid = grid;
        TargetPosition = new Vector3(_targetNode.X * _cellSize + _cellSize * 0.5f + pos.x,
            _targetNode.Y * _cellSize + _cellSize * 0.5f+pos.y, 0);
        _grid.GetValue(_currentNode.X, _currentNode.Y).isWalkable = false;
        transform.position = new Vector3(_currentNode.X * _cellSize + _cellSize * 0.5f+ pos.x, _currentNode.Y * _cellSize + _cellSize * 0.5f+ pos.y, 0);
        transform.localScale = new Vector3( _cellSize, _cellSize, 0f);
    }
    

    public void SetTarget(PathNode target)
    {
        _currentNode.isWalkable = true;
        
        _currentNode = _targetNode;        

        if (child is not null)
        {
            child.SetTarget(_currentNode);
            _currentNode.isWalkable = false;
        }
        
        _targetNode = target;
        _targetNode.isWalkable = false;
        TargetPosition = new Vector3(_targetNode.X * _cellSize + _cellSize * 0.5f+ _position.x,
            _targetNode.Y * _cellSize + _cellSize * 0.5f+ _position.y, 0);
        
        transform.GetComponentInChildren<ImageRotationP>().UpdateImageRotation();
    }
    public Vector2 GetDirection()
    {
        return (_targetNode.Positioning() - _currentNode.Positioning()).normalized;
    }
    public void UpdateMovement()
    {
        transform.position += (TargetPosition-transform.position).normalized * (_cellSize * _speed) * Time.deltaTime;
    }
}
