using UnityEngine;

public class ShadowCat : MonoBehaviour
{
    public Transform player; // Assign player in inspector
    public float speed = 5f;
    public float followDelay = 1f; // 1 second delay
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public Transform shootPoint;

    private bool canFollow = false;

    void Start()
    {
        // Start follow delay
        Invoke("EnableFollow", followDelay);
        // Start shooting periodically
        InvokeRepeating("ShootAtPlayer", followDelay, shootInterval);
    }

    void EnableFollow()
    {
        canFollow = true;
    }

    void Update()
    {
        if (canFollow && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                0.5f * speed * Time.deltaTime
            );
        }
    }

    void ShootAtPlayer()
    {
        if (player == null || projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        Vector2 direction = ((Vector2)player.position - (Vector2)shootPoint.position).normalized;

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * 10f;

        Destroy(proj, 5f); // destroys projectile after 3 seconds
    }
}