using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // Input System
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public float jumpForce = 10f;
    private bool isGrounded;

    // UI Elements
    public TextMeshProUGUI countText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI livesText;  // Text to display remaining lives
    public GameObject winTextObject;
    public GameObject gameOverMenu;    // Reference to the Game Over menu
    public GameObject inGameMenu;      // Reference to the In-Game Menu

    // Timer Variables
    private float timeElapsed = 0f;
    private bool isTimerRunning = true;

    // Lives System
    public int maxLives = 3;           // Maximum number of lives
    private int currentLives;          // Player's current lives

    // Game State Tracking
    private bool isGameOver = false;   // Track if the game is over
    private bool isPaused = false;     // Track if the game is paused (for in-game menu)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        currentLives = maxLives;       // Initialize with max lives
        SetCountText();
        SetLivesText();
        winTextObject.SetActive(false);
        gameOverMenu.SetActive(false); // Hide the Game Over menu initially
        UpdateTimerDisplay();
        inGameMenu.SetActive(false);   // Hide the in-game menu at the start
    }

    void Update()
    {
        // Block all input if the game is over
        if (isGameOver)
        {
            Time.timeScale = 0f;  // Ensure the game remains paused during game over
            return;               // Exit Update immediately to block all inputs, including Escape
        }

        // Increment the timer if it's running
        if (isTimerRunning)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerDisplay();
        }

        // Check for In-Game Menu toggle (Escape key) but prevent opening if the game is over
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // If already paused, resume the game
            }
            else
            {
                PauseGame();  // If not paused, open the in-game menu
            }
        }
    }


    void PauseGame()
    {
        isPaused = true;
        inGameMenu.SetActive(true);    // Show the in-game menu
        Time.timeScale = 0f;           // Pause the game
    }

    void ResumeGame()
    {
        isPaused = false;
        inGameMenu.SetActive(false);   // Hide the in-game menu
        Time.timeScale = 1f;           // Resume the game
    }

    void OnMove(InputValue movementValue)
    {
        if (isGameOver || isPaused) return;  // Block movement during Game Over or pause

        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    public void OnJump()
    {
        if (CheckGrounded() && !isGameOver && !isPaused)  // Block jump if paused or game is over
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f || isGameOver) return;  // Prevent movement when the game is paused or over

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            // Reduce the timer by 5 seconds when a pickup is collected
            ReduceTime(2f);
        }
        else if (other.gameObject.CompareTag("DeathZone"))
        {
            LoseLife(); // Call method when player falls into death zone
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
            isTimerRunning = false; // Stop the timer
            Time.timeScale = 0f;    // Pause the game on win
        }
    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + currentLives.ToString();
    }

    void LoseLife()
    {
        if (isGameOver) return;  // Prevent anything from happening if the game is already over

        currentLives--;          // Decrease the life count
        SetLivesText();

        if (currentLives > 0)
        {
            // Reset the player's position and timer, but keep lives and pickups
            ResetPlayerPosition();
        }
        else
        {
            GameOver();          // Trigger Game Over if no lives are left
        }
    }

    void ResetPlayerPosition()
    {
        // Reset player position (you can customize this based on your level)
        transform.position = new Vector3(0, 1, 0);  // Respawn at a specific point
        rb.velocity = Vector3.zero;  // Reset any velocity or movement
        SetCountText();
        UpdateTimerDisplay();
    }

    void GameOver()
    {
        isPaused = true;             // Set paused state
        isGameOver = true;           // Set game over state
        isTimerRunning = false;      // Stop the timer
        gameOverMenu.SetActive(true); // Show the Game Over menu
        inGameMenu.SetActive(false);  // Ensure the In-Game Menu is disabled
        Time.timeScale = 0f;         // Completely pause the game
    }

    // Reduce time by the specified amount, but don't let the time go below 0
    void ReduceTime(float amount)
    {
        timeElapsed = Mathf.Max(0f, timeElapsed - amount);
        UpdateTimerDisplay();
    }

    // Update the timer UI display
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private bool CheckGrounded()
    {
        float distanceToGround = 1.0f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceToGround))
        {
            return hit.collider != null;
        }
        return false;
    }

    // Called from the UI to restart the game
    public void TryAgain()
    {
        isPaused = false;     // Reset paused state
        isGameOver = false;   // Reset game over state
        Time.timeScale = 1f;  // Resume normal game speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Restart the current scene
    }

    // Called from the UI to return to the Main Menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;  // Resume normal game speed
        SceneManager.LoadScene("MainMenu"); // Replace with your actual Main Menu scene name
    }
}
