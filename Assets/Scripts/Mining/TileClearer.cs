using UnityEngine;
using UnityEngine.Tilemaps;

public class TileClearer : MonoBehaviour
{
    // imitate stardew ripoff tile stuff here

    [SerializeField] Tilemap caveWallsTilemap;
    Vector3Int tilePositionMouseIsAbove;
    bool selectable; // make hard rock unselectable, and ground tiles
    [SerializeField] MarkerManager markerManager;

    public GridMaker gridMaker;
    Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = gridMaker.pathfinding;
    }

    private void Update()
    {
        SetMouseGridPosition();
        CheckIfTileIsSelectable();
        Marker();
        if (Input.GetMouseButtonDown(0))
        {
            ClearTile();
        }
    }

    private void SetMouseGridPosition()
    {
        tilePositionMouseIsAbove = GetGridPosition(Input.mousePosition, true);
    }

    void CheckIfTileIsSelectable()
    {
        selectable = caveWallsTilemap.HasTile(tilePositionMouseIsAbove);
        markerManager.Show(selectable); // dont pass true, pass true/false depending
    }

    void Marker()
    {
        markerManager.markedCellPosition = tilePositionMouseIsAbove;
    }

    private void ClearTile()
    {
        if (caveWallsTilemap.GetTile(tilePositionMouseIsAbove))
        {
            caveWallsTilemap.SetTile(tilePositionMouseIsAbove, null);

            // redo just the one tile's walkability in grid
            pathfinding.SetWalkabilityOfTile(tilePositionMouseIsAbove.x, 
                tilePositionMouseIsAbove.y, pathfinding.GetGrid().originPosition);
            
            // redo grid
          //  pathfinding.SetWalkabilityOfGrid();
        }
        // add 1 to stone (or something)
    }

    private Vector3Int GetGridPosition(Vector2 position, bool mousePosition) // whats the bool about?
    {
        Vector3 worldPosition;

        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3Int gridPosition = caveWallsTilemap.WorldToCell(worldPosition);

        return gridPosition;
    }
}