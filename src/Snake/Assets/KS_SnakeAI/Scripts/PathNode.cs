using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathNode
{
    private CustomGrid<PathNode> _grid;

    [field:SerializeField] public int X { get; private set; }
    [field:SerializeField] public int Y { get; private set; }
    public int gCost, hCost, fCost;
    public bool isWalkable;
    public PathNode FromNode;
    
    public PathNode(CustomGrid<PathNode> grid, int x, int y, bool isWalkable=true)
    {
        this._grid = grid;
        this.X = x;
        this.Y = y;
        this.isWalkable = isWalkable;
    }

    public override string ToString()
    {
        return X + "," + Y;
    }
    public Vector2 Positioning()
    {
        return new Vector2(X, Y);
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void XYSet(int x, int y)
    {
        X = x;
        Y = y;
    }
}