using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    public static event UnityAction pickedUpItem;
    [SerializeField] List<InventoryItemSO> inventoryList = new List<InventoryItemSO>();

    public List<InventoryItemSO> InventoryList { get { return inventoryList; } }

    public void AddToInventory(InventoryItemSO item, int amountToAdd) // implement amount later
    {
        if (inventoryList.Contains(item))
        {
            item.numberHeld += amountToAdd;
        }
        else
        {
            inventoryList.Add(item);
            item.numberHeld = amountToAdd;
        }
        pickedUpItem?.Invoke();
    }
}