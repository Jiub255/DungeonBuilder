using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfindingg : MonoBehaviour
{
    PathRequestManager requestManager;
    Griddd grid;

    private void Awake()
    {
        grid = GetComponent<Griddd>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector2 startPosition, Vector2 targetPosition)
    {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }

    IEnumerator FindPath(Vector2 startPosition, Vector2 targetPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        if (startNode.walkable && targetNode.walkable)
        {
            // OPEN: THE SET OF NODES TO BE EVALUATED
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            // CLOSED: THE SET OF NODES ALREADY EVALUATED
            HashSet<Node> closedSet = new HashSet<Node>();
            // ADD THE START NODE TO OPEN
            openSet.Add(startNode);

            // LOOP
            while (openSet.Count > 0)
            {
                // CURRENT = NODE IN OPEN WITH THE LOWEST F COST
                // REMOVE CURRENT FROM OPEN
                Node currentNode = openSet.RemoveFirst();

                // ADD CURRENT TO CLOSED
                closedSet.Add(currentNode);

                // IF CURRENT IS TARGET NODE (PATH HAS BEEN FOUND)
                    // RETURN
                if (currentNode == targetNode)
                {
                    sw.Stop();
                    print("Path found in " + sw.ElapsedMilliseconds + "milliseconds");
                    pathSuccess = true;

                    break;
                }

                // FOREACH NEIGHBOR OF THE CURRENT NODE
                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    // IF NEIGHBOR IS NOT TRAVERSABLE OR NEIGHBOR IS IN CLOSED
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                    {
                        // SKIP TO THE NEXT NEIGHBOR
                        continue;
                    }

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) + 
                        neighbor.movementPenalty;
                    // IF NEW PATH TO NEIGHBOR IS SHORTER OR NEIGHBOR IS NOT IN OPEN
                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        // SET F COST OF NEIGHBOR
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        // SET PARENT OF NEIGHBOR TO CURRENT
                        neighbor.parent = currentNode;

                        // IF NEIGHBOR IS NOT IN OPEN
                        if (!openSet.Contains(neighbor))
                        {
                            // ADD NEIGHBOR TO OPEN
                            openSet.Add(neighbor);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }

        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
           
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector2[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2
                (path[i - 1].gridX - path[i].gridX,
                path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}