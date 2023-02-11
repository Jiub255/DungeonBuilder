using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Griddd : MonoBehaviour
{
    public bool displayGridGizmos;

    [SerializeField] Tilemap unwalkableTilemap; // do list instead for multiple tilemaps? no, do tilebase stuff
    [SerializeField] Tilemap walkableTilemap;
    [SerializeField] int blurTileRadius = 1;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    //public TerrainType[] walkableRegions; // do similar, but list (or array?) of a class containing tilebase and movementpenalty
    public List<TileBaseAndPenalty> tilesAndPenaltiesList;

    public int obstacleProximityPenalty = 1;

   // LayerMask walkableMask;

    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.FloorToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.FloorToInt(gridWorldSize.y / nodeDiameter); // does this need floor instead of round?
        
/*        foreach (TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.mask.value;
        }*/
        
        CreateGrid();
    }

    public int MaxSize { get { return gridSizeX * gridSizeY; } }

    // need to call this each time you remove a cavewall tile
    // maybe there's a better way? without rebuilding entire grid each click?
    // yes. call Node(walkable, point you just clicked)
    // do it once the next part is set up
    [ContextMenu("Update Grid")]
    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridSizeX / 2 - 
            Vector2.up * gridSizeY / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) +
                    Vector2.up * (y * nodeDiameter + nodeRadius);

                Vector3Int tilePosition = new Vector3Int(Mathf.FloorToInt(worldPoint.x), 
                    Mathf.FloorToInt(worldPoint.y), 0);
               
                bool walkable = (unwalkableTilemap.GetTile(tilePosition) == null);

                int movementPenalty = 0;

                TileBase tileBase = walkableTilemap.GetTile(tilePosition);
                // USE NODE CLASS FOR PENALTY INFO, SET IT HERE? (FOR EACH INDIVIDUAL TILE IN GRID)
                // lists for each penalty amount? put whichever tilebase(s) have that amount in there?

                foreach (TileBaseAndPenalty tbp in tilesAndPenaltiesList)
                {
                    if (tbp.tileBase == tileBase)
                    {
                        movementPenalty = tbp.movementPenalty;
                    }
                }

                if (!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        BlurPenaltyMap(blurTileRadius);
    }

    void BlurPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        // horizontal pass
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -blurSize; x <= blurSize; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, blurSize);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - blurSize - 1, 0 , gridSizeX);
                int addIndex = Mathf.Clamp(x + blurSize, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - 
                    grid[removeIndex,y].movementPenalty + grid[addIndex,y].movementPenalty;
            }
        }
        // vertical pass
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -blurSize; y <= blurSize; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, blurSize);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - blurSize - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + blurSize, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] -
                    penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) // middle node, not a neighbor
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && // in case it's on the edge
                    checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x / gridWorldSize.x) + 0.5f;
        float percentY = (worldPosition.y / gridWorldSize.y) + 0.5f;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt(Mathf.Min(gridSizeX * percentX, gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Min(gridSizeY * percentY, gridSizeY - 1));

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, 
                    n.movementPenalty));

                Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter));
            }
        }
    }

/*    [System.Serializable]
    public class TerrainType
    {
        public LayerMask mask;
        public int penalty;
    }*/

    [System.Serializable]
    public class TileBaseAndPenalty
    {
        public TileBase tileBase;
        public int movementPenalty;
    }
}