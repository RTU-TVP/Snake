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
    [SerializeField] private bool _isMoving;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] public SnakePart child;
    
    private CustomGrid<PathNode> _grid;
    
    public void Setter(CustomGrid<PathNode> grid,  PathNode currentNode,PathNode targetNode, int cellSize, float speed)
    {
        _currentNode = currentNode;
        _targetNode = targetNode;
        _cellSize = cellSize;
        _speed = speed;
        _grid = grid;
        _targetPosition = new Vector3(_targetNode.X * _cellSize + _cellSize * 0.5f,
            _targetNode.Y * _cellSize + _cellSize * 0.5f, 0);
        _grid.GetValue(_currentNode.X, _currentNode.Y).isWalkable = false;
        transform.position = new Vector3(_currentNode.X * _cellSize + _cellSize * 0.5f, _currentNode.Y * _cellSize + _cellSize * 0.5f, 0);
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
        _targetPosition = new Vector3(_targetNode.X * _cellSize + _cellSize * 0.5f,
            _targetNode.Y * _cellSize + _cellSize * 0.5f, 0);
    }

    public void UpdateMovement()
    {
        transform.position += (_targetPosition-transform.position).normalized * (_cellSize * _speed) * Time.deltaTime;
    }
}
