using UnityEngine;

public class Chase : MonoBehaviour
{
    [SerializeField] string targetsLayersName = "Hero";
    [SerializeField] float speed = 1f;
    [SerializeField] float outerChaseRadius = 4f;
    [SerializeField] float innerChaseRadius = 1f;

    float secondsBetweenDistanceChecks = 0.1f;
    float distanceCheckTimer;
    bool chasing;
    Transform target;
    LayerMask targetLayermask;
    Rigidbody2D rb;

    private void OnEnable()
    {
        target = null;
        targetLayermask = LayerMask.GetMask(targetsLayersName);
        chasing = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        distanceCheckTimer -= Time.deltaTime;
        if (distanceCheckTimer <= 0f)
        {
            if (target)
            {
                if (Vector2.Distance(target.position, transform.position) > outerChaseRadius)
                {
                    target = null;
                    chasing = false;
                }
            }
            if (!target || !target.gameObject.activeInHierarchy) // no target or target got killed (deactivated)
            {
                Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(
                    transform.position,
                    outerChaseRadius,
                    targetLayermask
                    );

                // could target closest, weakest, whatever, just targeting index 0 for now
                /* foreach (Collider2D collider2D in enemyColliders) { }*/
                if (enemyColliders.Length > 0)
                {
                    target = enemyColliders[0].transform;
                    chasing = true;
                }
            }
            distanceCheckTimer = secondsBetweenDistanceChecks;
        }
    }

    private void FixedUpdate()
    {
        if (chasing && Vector2.Distance(target.position, transform.position) > innerChaseRadius)
        {
            Vector3 movementVector = (target.position - transform.position).normalized;
            rb.MovePosition(transform.position + movementVector * speed * Time.fixedDeltaTime);
        }
    }
}