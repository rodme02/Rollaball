using UnityEngine;

public class DeathZoneScript : MonoBehaviour
{
    public Transform respawnPoint; // Set the player's respawn position

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Reset the player's position to the respawn point
            other.transform.position = respawnPoint.position;

            // Get the Rigidbody component of the player (ball)
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // If the Rigidbody exists, reset its velocity to stop the ball from moving
            if (rb != null)
            {
                rb.velocity = Vector3.zero;     // Reset linear velocity
                rb.angularVelocity = Vector3.zero; // Reset rotational/angular velocity
            }
        }
    }
}
