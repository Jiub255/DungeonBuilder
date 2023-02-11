using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OrderedPair
{
    public int xCoordinate;
    public int yCoordinate;
}

[CreateAssetMenu]
public class TilesSO : ScriptableObject
{
    public Dictionary<OrderedPair, TileBase> tileDictionary;

    public void UpdateTileDictionary(OrderedPair orderedPair, TileBase tileBase)
    {
        if (tileDictionary.ContainsKey(orderedPair))
        {
            ChangeDictionaryEntry(orderedPair, tileBase);
        }
        else
        {
            AddToDictionary(orderedPair, tileBase);
        }
    }

    private void ChangeDictionaryEntry(OrderedPair orderedPair, TileBase tileBase)
    {
        tileDictionary[orderedPair] = tileBase;
    }

    private void AddToDictionary(OrderedPair orderedPair, TileBase tileBase)
    {
        tileDictionary.Add(orderedPair, tileBase);
    }
}