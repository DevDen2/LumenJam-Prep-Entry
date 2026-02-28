using System.Collections.Generic;
using UnityEngine;

public class ShadowCat : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float followDelay = 1f;
    public float speed = 5f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootInterval = 2f;
    public float projectileSpeed = 10f;

    public float catchUpAcceleration = 0.1f;

    private List<Vector2> playerPositions = new List<Vector2>();
    private float recordInterval = 0.05f;
    private float recordTimer = 0f;
    private float shootTimer = 0f;

    void Update()
    {
        RecordPlayerPosition();
        FollowRecordedPath();
        HandleShooting();
        speed += catchUpAcceleration * Time.deltaTime;
        if (player.position.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void RecordPlayerPosition()
    {
        recordTimer += Time.deltaTime;

        if (recordTimer >= recordInterval)
        {
            playerPositions.Add(player.position);
            recordTimer = 0f;
        }
    }

    void FollowRecordedPath()
    {
        int delayFrames = Mathf.RoundToInt(followDelay / recordInterval);

        if (playerPositions.Count > delayFrames)
        {
            Vector2 targetPosition = playerPositions[playerPositions.Count - delayFrames];

            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
        }
    }

    void HandleShooting()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    void ShootAtPlayer()
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Random angle between 60° and 120° (upward cone)
        float randomAngle = Random.Range(60f, 120f);

        float radians = randomAngle * Mathf.Deg2Rad;

        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * projectileSpeed;

        Destroy(proj, 3f);
    }
}