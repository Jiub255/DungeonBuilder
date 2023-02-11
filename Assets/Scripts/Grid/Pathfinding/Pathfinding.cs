using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Pathfinding
{
    const int MOVE_STRAIGHT_COST = 100000000; // over the top accuracy
    const int MOVE_DIAGONAL_COST = 141421356;

    Grid<PathNode> grid;
    List<PathNode> openList;
    HashSet<PathNode> closedList; // changed List to HashSet, supposed to be quicker.
    // from comments, "Another optimization tip is to use the generic Hashset<>
    // instead of List<> for the closed node list,
    // since you only need to check whether it contains a given node or not." Me no know.

    public Tilemap wallsTilemap; 

    public Pathfinding(int width, int height, float cellSize, Vector3 originPosition)
    {
        grid = new Grid<PathNode>(width, height, cellSize, originPosition,
            (Grid<PathNode> g, Vector2Int p) => new PathNode(g, p));

        // should get wallsTilemap reference
        foreach (Tilemap t in Object.FindObjectsOfType<Tilemap>())
        {
            if (t.gameObject.layer == 3) // layer 3 is "Unwalkable"
            {
                wallsTilemap = t;
            }
        }

        SetWalkabilityOfGrid(originPosition);
    }

    public void SetWalkabilityOfGrid(Vector3 originPosition)
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                SetWalkabilityOfTile(x, y, originPosition);
            }
        }
    }

    public void SetWalkabilityOfTile(int x, int y, Vector3 originPosition)
    {
        Vector3Int tilePosition = new Vector3Int((int)originPosition.x + x, (int)originPosition.y + y, 0);
        bool walkable = (wallsTilemap.GetTile(tilePosition) == null);
        grid.gridArray[x, y].isWalkable = walkable; // indexoutofrangeexception when clicking dirt tile not in first quadrant
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    public List<PathNode> FindPath(Vector2Int start, Vector2Int end)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start(); 

        PathNode startNode = grid.GetGridObject(start);
        PathNode endNode = grid.GetGridObject(end);

        openList = new List<PathNode> { startNode };
        closedList = new HashSet<PathNode>(); 

        // clear/initialize grid?
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Vector2Int nodePosition = new Vector2Int(x, y);
                PathNode pathNode = grid.GetGridObject(nodePosition);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        // initialize start node
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // while there are opens left to check
        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList); // expensive part?
            if (currentNode == endNode)
            {
                // reached final node
                sw.Stop();
                UnityEngine.Debug.Log("Path found in " + sw.ElapsedMilliseconds + "milliseconds");

                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighborNode in GetNeighborList(currentNode))
            {
                if (closedList.Contains(neighborNode))
                {
                    continue;
                }
                if (!neighborNode.isWalkable)
                {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);
                if (tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        // out of nodes on the openList
        return null;
    }

    List<PathNode> GetNeighborList(PathNode currentNode)
    {
        List<PathNode> neighbors = new List<PathNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) // middle node, not a neighbor
                    continue;

                Vector2Int neighborPosition = 
                    new Vector2Int((int)currentNode.position.x + x, (int)currentNode.position.y + y);

                if (neighborPosition.x >= 0 && neighborPosition.x < grid.GetWidth() && // in case it's on the edge
                    neighborPosition.y >= 0 && neighborPosition.y < grid.GetHeight())
                {
                    neighbors.Add(GetNode(neighborPosition));
                }
            }
        }

        return neighbors;
    }

    PathNode GetNode(Vector2Int position)
    {
        return grid.GetGridObject(position);
    }

    List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.position.x - b.position.x);
        int yDistance = Mathf.Abs(a.position.y - b.position.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    // heap optimization here?
    PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}