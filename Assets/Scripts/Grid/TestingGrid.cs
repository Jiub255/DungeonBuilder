using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TestingGrid : MonoBehaviour
{
    public Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = new Pathfinding(10, 10, 1f, Vector3.zero); 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            Vector2Int gridPosition = pathfinding.GetGrid().GetXY(mouseWorldPosition);
            List<PathNode> path = pathfinding.FindPath(Vector2Int.zero, gridPosition);
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
        }
    }
}