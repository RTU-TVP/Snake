using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private int _width, _height, _cellSize;
    [SerializeField] private Vector2 _position;
    [SerializeField] private PathNode _currentNode;
    [SerializeField] private PathNode _targetNode;
    
    [SerializeField] private float _speed;
    [SerializeField] private bool _isMoving;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private PathNode _nextNode;

    private List<SnakePart> _snakeParts = new List<SnakePart>();
    [SerializeField] private CircleCollider2D _headCol;
    [field:SerializeField] public List<CircleCollider2D> ColliderList { get; private set; } =  new List<CircleCollider2D>();
    
    [SerializeField] private GameObject _snakePart;
    
    [SerializeField] private int _partsOfSnake=1;
    
    public PathFinder _pathFinder { get; private set; }
    
    void Start()
    {
        _pathFinder = new PathFinder(_width, _height, _cellSize, _position);
        transform.position = new Vector3(_currentNode.X * _cellSize + _cellSize * 0.5f+_position.x, _currentNode.Y * _cellSize + _cellSize * 0.5f +_position.y, 0);
        transform.localScale = new Vector3( _cellSize, _cellSize, 0f);
        _headCol = transform.GetChild(0).GetComponent<CircleCollider2D>();
        ColliderList.Add(_headCol);
        
        FirstSnakePartCreate(1);
        for (int i = 1; i < _partsOfSnake; i++)
        {
            SnakePartCreate(1);   
        }
    }
    private void FirstSnakePartCreate(int xAdd=0, int yAdd=0)
    {
        GameObject snakePart = Instantiate(_snakePart);
        snakePart.AddComponent<SnakePart>();
        SnakePart part = snakePart.GetComponent<SnakePart>();
        part.Setter(_pathFinder.Grid,
            _pathFinder.Grid.GetValue(_currentNode.X + xAdd, _currentNode.Y+yAdd), _currentNode, _cellSize, _position, _speed);
        part._currentNode.isWalkable = true;
        part._targetNode.isWalkable = false;
        
        _snakeParts.Add(part);
        ColliderList.Add(snakePart.GetComponentInChildren<CircleCollider2D>());
    }
    private void SnakePartCreate(int xAdd=0, int yAdd=0)
    {
        GameObject snakePart = Instantiate(_snakePart);
        snakePart.AddComponent<SnakePart>();
        SnakePart part = snakePart.GetComponent<SnakePart>();
        var x = (_snakeParts.Count-1);
        var node = _snakeParts[x]._currentNode;
        part.Setter(_pathFinder.Grid,
            _pathFinder.Grid.GetValue(node.X - xAdd, node.Y-yAdd), node, _cellSize, _position, _speed);
        part._currentNode.isWalkable = true;
        part._targetNode.isWalkable = false;
        _snakeParts[x].child = part;
        _snakeParts.Add(part);
        ColliderList.Add(snakePart.GetComponentInChildren<CircleCollider2D>());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {Print();
        }
    }

    private void PlayerPositioning()
    {
        Vector2 pos = PlayerCellCoordinates.GetPlayerCellCoordinates();
        _targetNode = _pathFinder.Grid.GetValue((int)pos.x, (int)pos.y);
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
        {
           PlayerPositioning();
            if (!(_targetNode.X == _currentNode.X && _targetNode.Y  == _currentNode.Y))
            {
                _nextNode = _pathFinder.FindStep(_currentNode.X, _currentNode.Y, _targetNode.X, _targetNode.Y);
                if (_nextNode is null)
                {
                    PathNode  neighbour = FindNeighbour();

                    if (neighbour is null)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        _nextNode = neighbour;
                    }
                }
                _targetPosition = new Vector3(_nextNode.X * _cellSize + _cellSize * 0.5f+_position.x,
                        _nextNode.Y * _cellSize + _cellSize * 0.5f+_position.y, 0);
                    _isMoving = true;
            }
        }
        else
        {
            transform.position += (_targetPosition-transform.position).normalized * (_cellSize * _speed) * Time.deltaTime;
            UpdatePartsMovement();
            if (new Vector3(Mathf.Round(transform.position.x*10)/10,Mathf.Round(transform.position.y*10)/10, 0) == _targetPosition)
            {
                _currentNode = _nextNode;
                _snakeParts[0].SetTarget(_currentNode);
                _isMoving = false;
            }
        }
    }

    private PathNode FindNeighbour()
    {
        int x = _currentNode.X;
        int y = _currentNode.Y;

            PathNode neighbourNode = _pathFinder.Grid.GetValue(x + 1, y);
            if (neighbourNode is not null && neighbourNode.isWalkable) return neighbourNode;
            neighbourNode = _pathFinder.Grid.GetValue(x - 1, y);
            if (neighbourNode is not null && neighbourNode.isWalkable) return neighbourNode;
            neighbourNode = _pathFinder.Grid.GetValue(x , y+1);
            if (neighbourNode is not null && neighbourNode.isWalkable) return neighbourNode;
            neighbourNode = _pathFinder.Grid.GetValue(x , y-1);
            if (neighbourNode is not null && neighbourNode.isWalkable) return neighbourNode;
            return null;
    }

    private void UpdatePartsMovement()
    {
        var target = _currentNode;
        foreach (SnakePart snakePart in _snakeParts)
        {
            snakePart.UpdateMovement();
        }
    }

    private void Print()
    {
        for (int i = 0; i < _pathFinder.Grid.GetWidth(); i++)
        {
            for (int j = 0; j< _pathFinder.Grid.GetHeight(); j++)
            {
                if (!_pathFinder.Grid.GetValue(i, j).isWalkable)
                {
                    Debug.Log(i.ToString() + "," + j.ToString() + " is now not walkable");
                }
            }
        }
    }
}
