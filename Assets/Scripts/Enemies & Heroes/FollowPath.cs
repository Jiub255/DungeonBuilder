using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] GridMaker gridMaker;
    [SerializeField] Transform target;
    [SerializeField] float speed = 5f;

    Pathfinding pathfinding;
    List<PathNode> path;
    Grid<PathNode> grid;

    int currentNodeIndex = 1;
    Vector2Int nextNodePosition;
    float distanceToNextNode;

    bool chasing = true;

    private void Start()
    {
        pathfinding = gridMaker.pathfinding;

        grid = pathfinding.GetGrid();

        path = GetPath();
        if (path != null)
        {
            nextNodePosition = path[currentNodeIndex].position;
        }
    }

    private void Update()
    {
        if (chasing)
        {
            if (path != null)
            {
                distanceToNextNode = Vector2.Distance(transform.position, GetWorldPosition(nextNodePosition));

                if (distanceToNextNode < 0.1f)
                {
                    currentNodeIndex++;
                    if (currentNodeIndex >= path.Count)
                    {
                        chasing = false;
                    }
                    else
                    {
                        nextNodePosition = path[currentNodeIndex].position;
                    }
                }
            }

            Vector3 movementVector = (GetWorldPosition(nextNodePosition) - transform.position).normalized;
            transform.Translate(movementVector * speed * Time.deltaTime);
        }
    }

    List<PathNode> GetPath()
    {
        if (target != null)
        {
            Vector2Int seekerGridPosition = GetGridPosition(transform.position);
            Vector2Int targetGridPosition = GetGridPosition(target.position);
            List<PathNode> path = pathfinding.FindPath(seekerGridPosition, targetGridPosition);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(
                        new Vector3(path[i].position.x, path[i].position.y) * 1f + Vector3.one * .5f,
                        new Vector3(path[i + 1].position.x, path[i + 1].position.y) * 1f + Vector3.one * .5f,
                        Color.green, 3f, false);
                }
            }
            return path;
        }

        return null;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        Vector2Int position = new Vector2Int(
            Mathf.FloorToInt((worldPosition - grid.originPosition).x / grid.cellSize),
            Mathf.FloorToInt((worldPosition - grid.originPosition).y / grid.cellSize));
        return position;
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        Vector3 position = new Vector3(
            grid.cellSize * gridPosition.x + grid.originPosition.x,
            grid.cellSize * gridPosition.y + grid.originPosition.y, 
            0f);
        return position;
    }
}