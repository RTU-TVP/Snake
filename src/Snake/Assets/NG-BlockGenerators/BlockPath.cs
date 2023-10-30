using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPath : MonoBehaviour
{
    [SerializeField] private CustomGrid<PathNode> _grid;
    private void Start()
    {
        _grid = FindObjectOfType<Snake>().GetComponent<Snake>().PathFinder.Grid;
        _grid.GetValue((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f)).isWalkable = false;
    }

    private void OnDestroy()
    {
        _grid.GetValue((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f)).isWalkable = true;
    }
}