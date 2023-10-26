using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BlockPosition : MonoBehaviour
{
    [SerializeField] private CustomGrid<PathNode> _grid;
    private void Start()
    {
        _grid.GetValue((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f)).isWalkable = false;
    }

    private void OnDestroy()
    {
        _grid.GetValue((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f)).isWalkable = true;
    }
}
