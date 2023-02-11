using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    [SerializeField] InventoryItemSO itemToDrop;
    [SerializeField] int relativeChanceToDrop;

    public InventoryItemSO ItemToDrop { get { return itemToDrop; } }
    public int RelativeChanceToDrop { get { return relativeChanceToDrop; } }
}

[CreateAssetMenu]
public class LootTableSO : ScriptableObject
{
    [SerializeField] List<LootItem> possibleItemsToDrop;

    public InventoryItemSO RandomlySelectLoot()
    {
        int cumulativeProbability = 0;
        int totalProbability = 0;

        for (int i = 0; i < possibleItemsToDrop.Count; i++)
        {
            totalProbability += possibleItemsToDrop[i].RelativeChanceToDrop;
        }

        int randomNumber = Random.Range(0, totalProbability);

        for (int i = 0; i < possibleItemsToDrop.Count; i++)
        {
            cumulativeProbability += possibleItemsToDrop[i].RelativeChanceToDrop;
            if (randomNumber < cumulativeProbability)
            {
                return possibleItemsToDrop[i].ItemToDrop; // crashing here?
            }
        }

        return null;
    }
}