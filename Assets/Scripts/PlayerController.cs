using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // Input System

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float speed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;

    // Lives System
    public int maxLives = 3;
    private int currentLives;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentLives = maxLives;
        Time.timeScale = 1f;

        // Initialize the UI
        UIManager.Instance.UpdateScore(0);
        UIManager.Instance.UpdateLives(currentLives);
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.isGamePaused)
        {
            return; // Prevent any actions if the game is paused or over
        }
    }

    void OnMove(InputValue movementValue)
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.isGamePaused) return;

        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    public void OnJump()
    {
        if (CheckGrounded() && !GameManager.Instance.isGameOver && !GameManager.Instance.isGamePaused)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpSound);
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f || GameManager.Instance.isGameOver) return;

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pickupSound);
            UIManager.Instance.UpdateScore(1);
            TimerController timerController = FindObjectOfType<TimerController>();
            if (timerController != null)
            {
                timerController.ReduceTime(2f);
            }
        }
        else if (other.gameObject.CompareTag("GroundWin"))
        {
            GameManager.Instance.WinGame();  // Trigger the win condition
        }
        else if (other.gameObject.CompareTag("DeathZone"))
        {
            LoseLife();  // Call method when player falls into death zone
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LoseLife();  // Call method when player collides with enemy
        }
    }

    private void LoseLife()
    {
        if (GameManager.Instance.isGameOver) return;

        currentLives--;
        UIManager.Instance.UpdateLives(currentLives);

        if (currentLives > 0)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.deathSound);
            ResetPlayerPosition();
        }
        else
        {
            ResetPlayerPosition();
            GameManager.Instance.GameOver();
        }
    }

    private void ResetPlayerPosition()
    {
        transform.position = new Vector3(0, 1, 0);  // Respawn at a specific point
        rb.velocity = Vector3.zero;
    }

    private bool CheckGrounded()
    {
        float distanceToGround = 0.6f;
        RaycastHit hit;

        Vector3[] rayOrigins = new Vector3[]
        {
            transform.position,
            transform.position + transform.forward * 0.2f,
            transform.position - transform.forward * 0.2f,
            transform.position + transform.right * 0.2f,
            transform.position - transform.right * 0.2f
        };

        foreach (Vector3 origin in rayOrigins)
        {
            if (Physics.Raycast(origin, Vector3.down, out hit, distanceToGround))
            {
                if (hit.collider != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
