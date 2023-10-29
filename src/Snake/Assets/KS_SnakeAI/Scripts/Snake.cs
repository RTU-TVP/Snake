using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Snake : MonoBehaviour
{
    [SerializeField] private int _width, _height, _cellSize;
    [SerializeField] private Vector2 _position;
    [SerializeField] private PathNode _currentNode;
    [SerializeField] private PathNode _targetNode;
    
    [SerializeField] private float _speed;
    [SerializeField] private bool _isMoving;
    [SerializeField] public Vector3 TargetPosition{ get; private set; }
    [SerializeField] private PathNode _nextNode;

    private List<SnakePart> _snakeParts = new List<SnakePart>();
    [SerializeField] private BoxCollider2D _headCol;
    [field:SerializeField] public List<BoxCollider2D> ColliderList { get; private set; } =  new List<BoxCollider2D>();
    
    [SerializeField] private GameObject _snakePart;    
    [SerializeField] private GameObject _snakeTail;
    
    [SerializeField] private GameObject _turnSpriteRD;  
    [SerializeField] private GameObject _turnSpriteLD;  
    [SerializeField] private GameObject _turnSpriteRU;  
    [SerializeField] private GameObject _turnSpriteLU;  

    [SerializeField] private int _partsOfSnake=1;
    private float _partsAddTimer;
    [SerializeField] private float _partAddTimerCap = 0;
     private float _stanTimer=0;
    [SerializeField] private float _stanTimerCap=0;
    [SerializeField] private bool _classicMode=true;
    public PathFinder PathFinder { get; private set; }
    private Vector2 _direction;
    public void CurrentPosSetter(Vector2 pos)
    {
        _currentNode.XYSet((int)pos.x, (int)pos.y);
    }
    
    void OnEnable()
    {
        _partsAddTimer = _partAddTimerCap;

        PathFinder = new PathFinder(_width, _height, _cellSize, _position);
        transform.position = new Vector3(_currentNode.X * _cellSize + _cellSize * 0.5f+_position.x, _currentNode.Y * _cellSize + _cellSize * 0.5f +_position.y, 0);
        transform.localScale = new Vector3( _cellSize, _cellSize, 0f);
        _headCol = transform.GetChild(0).GetComponent<BoxCollider2D>();
        ColliderList.Add(_headCol);
        CreationParts(1);
        _direction = new Vector2(-1, 0);
    }

    private void AddToTail()
    {
        TailChanger(_snakePart);
        SnakePartCreate(0);
        TailChanger(_snakeTail);
    }

    private void TailChanger(GameObject prefab)
    {
        _snakeParts[_snakeParts.Count - 1].GetComponentInChildren<SpriteRenderer>().sprite = prefab.GetComponentInChildren<SpriteRenderer>().sprite;
        _snakeParts[_snakeParts.Count - 1].GetComponentInChildren<BoxCollider2D>().offset = prefab.GetComponentInChildren<BoxCollider2D>().offset;
        _snakeParts[_snakeParts.Count - 1].GetComponentInChildren<BoxCollider2D>().size = prefab.GetComponentInChildren<BoxCollider2D>().size;
    }

    private void CreationParts(int k)
    {
        FirstSnakePartCreate(k);
        for (int i = 1; i < _partsOfSnake; i++)
        {
            SnakePartCreate(k);
        }
    }

    public Vector2 GetDirection()
    {
        return (_nextNode.Positioning() - _currentNode.Positioning()).normalized;
    }

    private void FirstSnakePartCreate(int xAdd=0, int yAdd=0)
    {
        GameObject snakePart = Instantiate(_snakePart);
        snakePart.AddComponent<SnakePart>();
        SnakePart part = snakePart.GetComponent<SnakePart>();
        part.Setter(PathFinder.Grid,
            PathFinder.Grid.GetValue(_currentNode.X + xAdd, _currentNode.Y+yAdd), _currentNode, _cellSize, _position, _speed);
        part.CurrentNode.isWalkable = true;
        part.TargetNode.isWalkable = false;
        part.transform.GetComponentInChildren<ImageRotationP>().Snake = part.GetComponent<SnakePart>();
        _snakeParts.Add(part);
        ColliderList.Add(snakePart.GetComponentInChildren<BoxCollider2D>());
    }
    private void SnakePartCreate(int xAdd=0, int yAdd=0)
    {
        GameObject snakePart = Instantiate(_snakePart);
        snakePart.AddComponent<SnakePart>();
        SnakePart part = snakePart.GetComponent<SnakePart>();
        var x = (_snakeParts.Count-1);
        var node = _snakeParts[x].CurrentNode;
        part.Setter(PathFinder.Grid,
            PathFinder.Grid.GetValue(node.X + xAdd, node.Y+yAdd), node, _cellSize, _position, _speed);
        part.CurrentNode.isWalkable = true;
        part.TargetNode.isWalkable = false;
        part.transform.GetComponentInChildren<ImageRotationP>().Snake = part.GetComponent<SnakePart>();
        _snakeParts[x].child = part;
        _snakeParts.Add(part);
        ColliderList.Add(snakePart.GetComponentInChildren<BoxCollider2D>());
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
        _targetNode = PathFinder.Grid.GetValue((int)pos.x, (int)pos.y);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Stone")
        {
            _stanTimer = _stanTimerCap;
            Destroy(collision.gameObject);
            if (_classicMode) FindFirstObjectByType<DeleteHeart>().DestroyHeart();
        }
        if (collision.gameObject.tag == "Bonus")
        {
            Destroy(collision.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bonus")
        {
            Destroy(collision.gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (_stanTimer <= 0)
        {
            if (_partsAddTimer >= _partAddTimerCap) {
                AddToTail();
                _partsAddTimer = 0;
            }
            else _partsAddTimer += Time.deltaTime;

            if (!_isMoving) {
                PlayerPositioning();
                
                if (!(_targetNode.X == _currentNode.X && _targetNode.Y == _currentNode.Y)) {
                    _nextNode = PathFinder.FindStep(_currentNode.X, _currentNode.Y, _targetNode.X, _targetNode.Y);
                    if (_nextNode is null) {
                        PathNode neighbour = FindNeighbour();

                        if (neighbour is null) Destroy(gameObject);
                        else _nextNode = neighbour;
                    }

                    var x = GetDirection();
                    if (x != _direction)
                    {
                        StartCoroutine(SpawnTurnSprite(_direction, x));
                        _direction = x;
                    }

                    TargetPosition = new Vector3(_nextNode.X * _cellSize + _cellSize * 0.5f + _position.x,
                        _nextNode.Y * _cellSize + _cellSize * 0.5f + _position.y, 0);
                    _isMoving = true;
                    transform.GetComponentInChildren<ImageRotation>().UpdateImageRotation();
                }
            }
            else {
                transform.position += (TargetPosition - transform.position).normalized * (_cellSize * _speed) *
                                      Time.deltaTime;
                UpdatePartsMovement();
                if (new Vector3(Mathf.Round(transform.position.x * 10) / 10,
                        Mathf.Round(transform.position.y * 10) / 10, 0) == TargetPosition) {
                    _currentNode = _nextNode;
                    _snakeParts[0].SetTarget(_currentNode);
                    _isMoving = false;
                }
            }
        }
        
        else _stanTimer -= Time.deltaTime;
    }

    private PathNode FindNeighbour()
    {
        int x = _currentNode.X;
        int y = _currentNode.Y;

            PathNode neighbourNode = PathFinder.Grid.GetValue(x + 1, y);
            if (neighbourNode is not null && neighbourNode.isWalkable) return neighbourNode;
            neighbourNode = PathFinder.Grid.GetValue(x - 1, y);
            if (neighbourNode is not null && neighbourNode.isWalkable) return neighbourNode;
            neighbourNode = PathFinder.Grid.GetValue(x , y+1);
            if (neighbourNode is not null && neighbourNode.isWalkable) return neighbourNode;
            neighbourNode = PathFinder.Grid.GetValue(x , y-1);
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


    private IEnumerator SpawnTurnSprite(Vector2 rotateFrom, Vector2 rotateTo)
    {
        yield return new WaitForSeconds(0.1f);
        GameObject turn = null;
        turn = TurnImageRotation(rotateFrom, rotateTo, turn);
        yield return new WaitForSeconds(_snakeParts.Count * 0.212f);
        Destroy(turn);
    }

    private GameObject TurnImageRotation(Vector2 rotateFrom, Vector2 rotateTo, GameObject turn)
    {
        if ((rotateFrom == Vector2.right && rotateTo == Vector2.down) || (rotateFrom == Vector2.up && rotateTo == Vector2.left))
            turn = Instantiate(_turnSpriteRD, new Vector3(_currentNode.X + 0.5f, _currentNode.Y + 0.5f,-2f),
                Quaternion.identity);
        
        else if ((rotateFrom == Vector2.left && rotateTo == Vector2.down) || (rotateFrom == Vector2.up && rotateTo == Vector2.right))
            turn = Instantiate(_turnSpriteLD, new Vector3(_currentNode.X + 0.5f, _currentNode.Y + 0.5f,-2f),
                Quaternion.identity);
        
        else if ((rotateFrom == Vector2.right && rotateTo == Vector2.up) || (rotateFrom == Vector2.down && rotateTo == Vector2.left))
            turn = Instantiate(_turnSpriteRU, new Vector3(_currentNode.X + 0.5f, _currentNode.Y + 0.5f,-2f),
                Quaternion.identity);
        
        
        else if ((rotateFrom == Vector2.left && rotateTo == Vector2.up) || (rotateFrom == Vector2.down && rotateTo == Vector2.right))
            turn = Instantiate(_turnSpriteLU, new Vector3(_currentNode.X + 0.5f, _currentNode.Y + 0.5f, -2f),
                Quaternion.identity);
        
        
        return turn;
    }

    private void Print()
    {
        for (int i = 0; i < PathFinder.Grid.GetWidth(); i++)
        {
            for (int j = 0; j< PathFinder.Grid.GetHeight(); j++)
            {
                if (!PathFinder.Grid.GetValue(i, j).isWalkable)
                {
                    Debug.Log(i.ToString() + "," + j.ToString() + " is now not walkable");
                }
            }
        }
    }
}
