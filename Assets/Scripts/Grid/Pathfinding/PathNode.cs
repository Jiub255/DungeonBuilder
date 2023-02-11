using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    Grid<PathNode> grid;
    public Vector2Int position;

    public int gCost, hCost, fCost;

    public bool isWalkable = true;
    public PathNode cameFromNode;

    public PathNode(Grid<PathNode> grid, Vector2Int position)
    {
        this.grid = grid;
        this.position = position;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return position.x + "," + position.y;
    }
}
