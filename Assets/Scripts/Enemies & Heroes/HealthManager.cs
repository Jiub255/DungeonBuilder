using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int currentHealth;

    SpriteRenderer spriteRenderer;

    [SerializeField] LootTableSO lootTable;
    [SerializeField] InventorySO inventorySO;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        DropLoot();
        gameObject.SetActive(false);
    }

    void DropLoot()
    {
        if (lootTable)
        {
            InventoryItemSO loot = lootTable.RandomlySelectLoot();

            if (loot)
            {
                inventorySO.AddToInventory(loot, 1); 
            }
        }
    }
}