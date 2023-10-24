using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> _grid;
    private int _x, _y;

    public int gCost, hCost, fCost;
    public PathNode fromNode;
    public PathNode(UnityEngine.Grid<PathNode> grid, int x, int y)
    {
        this._grid = grid;
        this._x = x;
        this._y = y;
    }

    public override string ToString()
    {
        return _x + "," + _y;
    }
}
