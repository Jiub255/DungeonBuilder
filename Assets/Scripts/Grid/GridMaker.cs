using UnityEngine;

public class GridMaker : MonoBehaviour
{
    public Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = new Pathfinding(50, 50, 1f, new Vector3(-25, -25, 0));
    }
}