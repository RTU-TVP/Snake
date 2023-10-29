using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SnakePart : MonoBehaviour
{
    [SerializeField] private int _cellSize;

    [field:SerializeField] public PathNode CurrentNode { get; private set; }
    [field:SerializeField] public PathNode TargetNode { get; private set; }

    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _position;
    [SerializeField] public Vector3 TargetPosition { get; private set; }
    [SerializeField] public SnakePart child;

    private Color _childColorCopy;
    private SpriteRenderer _childSpriteRenderer;
    private CustomGrid<PathNode> _grid;
    
    public void Setter(CustomGrid<PathNode> grid,  PathNode currentNode,PathNode targetNode, int cellSize, Vector2 pos, float speed)
    {
        _childColorCopy = GetComponentInChildren<SpriteRenderer>().color;
        _childSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        CurrentNode = currentNode;
        TargetNode = targetNode;
        _cellSize = cellSize;
        _position = pos;
        _speed = speed;
        _grid = grid;
        TargetPosition = new Vector3(TargetNode.X * _cellSize + _cellSize * 0.5f + pos.x,
            TargetNode.Y * _cellSize + _cellSize * 0.5f+pos.y, 0);
        _grid.GetValue(CurrentNode.X, CurrentNode.Y).isWalkable = false;
        transform.position = new Vector3(CurrentNode.X * _cellSize + _cellSize * 0.5f+ pos.x, CurrentNode.Y * _cellSize + _cellSize * 0.5f+ pos.y, 0);
        transform.localScale = new Vector3( _cellSize, _cellSize, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bonus")
        {
            Destroy(collision.gameObject);
        }
    }

    public void SetTarget(PathNode target)
    {
        CurrentNode.isWalkable = true;
        
        CurrentNode = TargetNode;        

        if (child is not null)
        {
            child.SetTarget(CurrentNode);
            CurrentNode.isWalkable = false;
        }
        
        TargetNode = target;
        TargetNode.isWalkable = false;
        TargetPosition = new Vector3(TargetNode.X * _cellSize + _cellSize * 0.5f+ _position.x,
            TargetNode.Y * _cellSize + _cellSize * 0.5f+ _position.y, 0);
        
        transform.GetComponentInChildren<ImageRotationP>().UpdateImageRotation();
    }
    public Vector2 GetDirection()
    {
        return (TargetNode.Positioning() - CurrentNode.Positioning()).normalized;
    }
    public void UpdateMovement()
    {
        transform.position += (TargetPosition-transform.position).normalized * (_cellSize * _speed) * Time.deltaTime;
    }
    
    private IEnumerator Turning()
    {
        _childColorCopy.a = 0;
        _childSpriteRenderer.color = _childColorCopy;
        yield return new WaitForSeconds(0.08f);
        _childColorCopy.a = 255;
        _childSpriteRenderer.color = _childColorCopy;
    }
}
