using System;
using UnityEngine;

public class Grid<TGridObject>
{
    public const int sortingOrderDefault = 5000;

    int width;
    int height;
    public float cellSize;
    public Vector3 originPosition;
    public TGridObject[,] gridArray;
    Vector2Int gridArrayPosition;
    TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, 
        Func<Grid<TGridObject>, Vector2Int, TGridObject> createGridObject) // check for wall tiles here?
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArrayPosition = new Vector2Int(x, y);
                gridArray[x, y] = createGridObject(this, gridArrayPosition);

                // need the x,y from pathnode to work here?
                debugTextArray[x, y] = CreateWorldText(gridArrayPosition.ToString(), null, 
                    GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 0.1f,
                    40, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public Vector2Int GetXY(Vector3 worldPosition)
    {
        Vector2Int position = new Vector2Int(Mathf.FloorToInt((worldPosition - originPosition).x / cellSize), 
            Mathf.FloorToInt((worldPosition - originPosition).y / cellSize));
        return position;
    }

    public TGridObject GetGridObject(Vector2Int position)
    {
        if (position.x >= 0 && position.y >= 0 && position.x < width && position.y < height)
        {
            return gridArray[position.x, position.y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        Vector2Int position = GetXY(worldPosition);
        return GetGridObject(position);
    }

    public void SetGridObject(Vector2Int position, TGridObject value)
    {
        if (position.x >= 0 && position.y >= 0 && position.x < width && position.y < height)
        {
            gridArray[position.x, position.y] = value;
            debugTextArray[position.x, position.y].text = gridArray[position.x, position.y].ToString();
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        SetGridObject(GetXY(worldPosition), value);
    }

    public int GetWidth()
    {
        return gridArray.GetLength(0);
    }

    public int GetHeight()
    {
        return gridArray.GetLength(1);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(string text, Transform parent = null, 
        Vector3 localPosition = default(Vector3), float characterSize = 0.1f, int fontSize = 40, 
        Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, 
        TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, characterSize, fontSize, (Color)color, textAnchor, 
            textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, 
        float characterSize, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, 
        int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.characterSize = characterSize;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}