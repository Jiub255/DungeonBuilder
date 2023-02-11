using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class InventoryItemSO : ScriptableObject
{
    public itemType item;
    public int numberHeld;
}

public enum itemType 
{ 
    Gold,
    Souls,
    Iron,
    Stone
}