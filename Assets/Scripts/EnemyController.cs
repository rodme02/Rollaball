using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public float speed = 5f;  // Speed at which the enemy moves toward the player
    public float returnSpeed = 3f;  // Speed at which the enemy returns to its spawn point
    public float stoppingDistance = 0.1f;  // Minimum distance before the enemy stops moving

    private bool isPlayerInChaseZone = false;  // Tracks if the player is in the chase zone
    private Rigidbody rb;  // Reference to the enemy's Rigidbody
    private Vector3 spawnPosition;  // Original position where the enemy spawned

    // Store chase zone bounds
    private BoxCollider chaseZoneCollider;
    private Vector3 chaseZoneMinBounds;
    private Vector3 chaseZoneMaxBounds;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Save the enemy's original spawn position
        spawnPosition = transform.position;

        // Fallback to find player by tag if not assigned in the Inspector
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Get the ChaseZone's collider (assuming it's on a child object)
        chaseZoneCollider = GetComponentInChildren<BoxCollider>();
        if (chaseZoneCollider != null)
        {
            // Store the min and max bounds of the chase zone
            chaseZoneMinBounds = chaseZoneCollider.bounds.min;
            chaseZoneMaxBounds = chaseZoneCollider.bounds.max;
        }
    }

    void FixedUpdate()
    {
        // Manually check if the player is inside the chase zone using the collider's bounds
        if (PlayerInChaseZone())
        {
            isPlayerInChaseZone = true;
            MoveTowardsPlayer();
        }
        else
        {
            if (isPlayerInChaseZone) // Only log exit once when leaving the zone
            {
                isPlayerInChaseZone = false;
            }

            // Return to the spawn point if the player is outside the chase zone
            ReturnToSpawn();
        }
    }

    private bool PlayerInChaseZone()
    {
        // Check if the player's position is inside the chase zone's bounds
        return player.position.x >= chaseZoneMinBounds.x &&
               player.position.x <= chaseZoneMaxBounds.x &&
               player.position.z >= chaseZoneMinBounds.z &&
               player.position.z <= chaseZoneMaxBounds.z;
    }

    private void MoveTowardsPlayer()
    {
        // Calculate direction toward the player (only in X and Z axes)
        Vector3 direction = (player.position - transform.position).normalized;

        // Zero out the Y axis to prevent the enemy from moving vertically
        direction.y = 0;

        // Calculate the next position
        Vector3 nextPosition = transform.position + direction * speed * Time.fixedDeltaTime;

        // Clamp the next position within the chase zone bounds
        nextPosition = new Vector3(
            Mathf.Clamp(nextPosition.x, chaseZoneMinBounds.x, chaseZoneMaxBounds.x),
            nextPosition.y, // Keep the Y position (to stay on the ground)
            Mathf.Clamp(nextPosition.z, chaseZoneMinBounds.z, chaseZoneMaxBounds.z)
        );

        // Move the enemy toward the player within the chase zone
        rb.MovePosition(nextPosition);
    }

    private void ReturnToSpawn()
    {
        // If the enemy is close enough to the spawn point, stop moving
        if (Vector3.Distance(transform.position, spawnPosition) <= stoppingDistance)
        {
            rb.velocity = Vector3.zero;  // Stop moving
            return;
        }

        // Move directly towards the spawn point
        Vector3 direction = (spawnPosition - transform.position).normalized;

        // Ensure movement is horizontal (ignore Y axis)
        direction.y = 0;

        // Move the enemy toward the spawn point at returnSpeed
        rb.MovePosition(transform.position + direction * returnSpeed * Time.fixedDeltaTime);
    }
}
