using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] InventorySO inventorySO;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI soulsText;
    [SerializeField] TextMeshProUGUI ironText;
    [SerializeField] TextMeshProUGUI stoneText;

    [SerializeField] InventoryItemSO gold;
    [SerializeField] InventoryItemSO souls;
    [SerializeField] InventoryItemSO iron;
    [SerializeField] InventoryItemSO stone;

    private void OnEnable()
    {
        UpdateHUD();
        InventorySO.pickedUpItem += UpdateHUD;
    }

    public void UpdateHUD()
    {
        UpdateGold();
        UpdateSouls();
        UpdateIron();
        UpdateStone();
    }

    public void UpdateGold()
    {
        // or just store the amount in the inventoryitemso, dont bother with inventory for these?
        if (inventorySO.InventoryList.Contains(gold))
        {
            int index = inventorySO.InventoryList.FindIndex(x => x == gold);
            goldText.text = inventorySO.InventoryList[index].numberHeld.ToString();
        }
        else
        {
            goldText.text = "0";
        }
    }

    public void UpdateSouls()
    {
        if (inventorySO.InventoryList.Contains(souls))
        {
            int index = inventorySO.InventoryList.FindIndex(x => x == souls);
            soulsText.text = inventorySO.InventoryList[index].numberHeld.ToString();
        }
        else
        {
            soulsText.text = "0";
        }
    }

    public void UpdateIron()
    {
        if (inventorySO.InventoryList.Contains(iron))
        {
            int index = inventorySO.InventoryList.FindIndex(x => x == iron);
            ironText.text = inventorySO.InventoryList[index].numberHeld.ToString();
        }
        else
        {
            ironText.text = "0";
        }
    }

    public void UpdateStone()
    {
        if (inventorySO.InventoryList.Contains(stone))
        {
            int index = inventorySO.InventoryList.FindIndex(x => x == stone);
            stoneText.text = inventorySO.InventoryList[index].numberHeld.ToString();
        }
        else
        {
            stoneText.text = "0";
        }
    }

    private void OnDisable()
    {
        InventorySO.pickedUpItem -= UpdateHUD;
    }
}