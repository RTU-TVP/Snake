using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private int _width, _height, _cellSize;
    [SerializeField] private PathNode _currentNode;
    [SerializeField] private PathNode _targetNode;
    
    [SerializeField] private float _speed;
    [SerializeField] private bool _isMoving;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private PathNode _nextNode;

    private List<SnakePart> _snakeParts = new List<SnakePart>();
    [SerializeField] private GameObject _snakePart;
    
    private PathFinder _pathFinder;
    
    void Start()
    {
        _pathFinder = new PathFinder(_width, _height);
        transform.position = new Vector3(_currentNode.X * _cellSize + _cellSize * 0.5f, _currentNode.Y * _cellSize + _cellSize * 0.5f, 0);

        for (int i = 1; i < 3; i++)
        {
            SnakePartCreate(i);   
        }
    }

    private void SnakePartCreate(int xAdd=0, int yAdd=0)
    {
        GameObject snakePart = Instantiate(_snakePart);
        snakePart.AddComponent<SnakePart>();
        SnakePart part = snakePart.GetComponent<SnakePart>();
        part.Setter(_pathFinder.Grid,
            _pathFinder.Grid.GetValue(_currentNode.X - xAdd, _currentNode.Y-yAdd), _currentNode, _cellSize, _speed);
        _snakeParts.Add(part);
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
        {
            if (!(_targetNode.X == _currentNode.X && _targetNode.Y  == _currentNode.Y))
            {
                _nextNode = _pathFinder.FindStep(_currentNode.X, _currentNode.Y, _targetNode.X, _targetNode.Y);
                UpdatePartsTarget();
                _targetPosition = new Vector3(_nextNode.X * _cellSize + _cellSize * 0.5f,
                    _nextNode.Y * _cellSize + _cellSize * 0.5f, 0);
                _isMoving = true;
            }
        }
        else
        {
            transform.position += (_targetPosition-transform.position).normalized * (_cellSize * _speed) * Time.deltaTime;
            UpdatePartsMovement();
            if (new Vector3(Mathf.Round(transform.position.x*100)/100,Mathf.Round(transform.position.y*100)/100, 0) == _targetPosition)
            {
                _currentNode = _nextNode;
                _isMoving = false;
            }
        }
    }

    private void UpdatePartsMovement()
    {
        var target = _currentNode;
        foreach (SnakePart snakePart in _snakeParts)
        {
            snakePart.UpdateMovement();
        }
    }

    private void UpdatePartsTarget()
    {
        var target = _currentNode;
        foreach (SnakePart snakePart in _snakeParts)
        {
            snakePart.EndMove();
            snakePart.SetTarget(target);
            target = snakePart._currentNode;
        }
    }
}
