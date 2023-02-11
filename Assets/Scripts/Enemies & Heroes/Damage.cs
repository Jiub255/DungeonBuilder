using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] List<string> targetLayerNames = new List<string>();
    [SerializeField] int damage = 1;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float secondsBetweenAttacks = 2f;

    float attackTimer;
    List<LayerMask> targetLayermasks = new List<LayerMask>();

    private void OnEnable()
    {
        targetLayermasks.Clear();
        for (int i = 0; i < targetLayerNames.Count; i++)
        {
            targetLayermasks.Add(LayerMask.GetMask(targetLayerNames[i]));
        }
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            for (int i = 0; i < targetLayermasks.Count; i++)
            {
                Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(
                    transform.position,
                    attackRadius,
                    targetLayermasks[i]
                    );

                foreach (Collider2D collider2D in enemyColliders)
                {
                    collider2D.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
                    // Debug.Log(gameObject.name + "damaged " + collider2D.gameObject.name + " for " + damage + " damage.");
                }
            }
            attackTimer = secondsBetweenAttacks;
        }
    }
}