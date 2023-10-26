using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    public static PathFinder Instance { get; private set; }
    public CustomGrid<PathNode> Grid { get; private set; }

    private List<PathNode> _openList;
    private List<PathNode> _closedList;
    public PathFinder(int width, int height)
    {
        Instance = this;
        Grid = new CustomGrid<PathNode> (width, height, 10f, Vector2.zero, 
                ((CustomGrid<PathNode> g, int x, int y) => new PathNode(g, x, y)));
    }

    public PathNode FindStep(int startX, int startY, int endX, int endY)
    {
        return FindPath(startX, startY, endX, endY)[1];
    }

    private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = Grid.GetValue(startX, startY);
        PathNode endNode = Grid.GetValue(endX, endY);
        
        _openList = new List<PathNode> { startNode };
        _closedList = new List<PathNode>();

        for (int i = 0; i < Grid.GetWidth(); i++)
        {
            for (int j = 0; j < Grid.GetHeight(); j++)
            {
                PathNode pathNode = Grid.GetValue(i, j);
                pathNode.gCost = Int32.MaxValue;
                pathNode.CalculateFCost();
                pathNode.FromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (_openList.Count>0)
        {
            PathNode currentNode = GetLowestFNode(_openList);
            if (currentNode == endNode)
            {
                return CalculatedPath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            var x = GetNeighbours(currentNode);
            foreach (PathNode neighbourNode in x)
            {
                if (_closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.FromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                    if (!_openList.Contains(neighbourNode)) _openList.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbours(PathNode currentNode)
    {
        List<PathNode> neighbours = new List<PathNode>();
        if (currentNode.X - 1 >= 0) neighbours.Add(Grid.GetValue(currentNode.X-1, currentNode.Y));
        if (currentNode.X + 1 < Grid.GetWidth())neighbours.Add(Grid.GetValue(currentNode.X+1, currentNode.Y));
        if (currentNode.Y - 1 >= 0) neighbours.Add(Grid.GetValue(currentNode.X, currentNode.Y-1));
        if (currentNode.Y + 1 < Grid.GetHeight()) neighbours.Add(Grid.GetValue(currentNode.X, currentNode.Y+1));
        return neighbours;
    }

    private void SetWalkable(int x, int y, bool w)
    {
        Grid.GetValue(x, y).isWalkable = w;
    }

    private List<PathNode> CalculatedPath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentPathNode = endNode;
        while (currentPathNode.FromNode != null)
        {
            path.Add(currentPathNode.FromNode);
            currentPathNode = currentPathNode.FromNode;
        }

        path.Reverse();
        return path;
    }
    
    private void SetNodeWalkable(PathNode node, bool setting)
    {
        Grid.GetValue(node.X, node.Y).isWalkable = setting;
    }
    
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        return xDistance + yDistance;
    }

    private PathNode GetLowestFNode(List<PathNode> pathNodes)
    {
        PathNode lowestFNode = pathNodes[0];
        for (int i = 1; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].fCost < lowestFNode.fCost) lowestFNode = pathNodes[i];
        }

        return lowestFNode;
    }
}
