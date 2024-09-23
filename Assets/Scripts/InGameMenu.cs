using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    public GameObject inGameMenuUI; // Reference to the in-game menu UI
    private bool isPaused = false; // Flag to track if the game is paused

    void Update()
    {
        // Toggle the in-game menu when the Esc key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resume the game and hide the in-game menu
    public void Resume()
    {
        inGameMenuUI.SetActive(false); // Hide the menu
        Time.timeScale = 1f;           // Resume game time
        isPaused = false;              // Update paused state
    }

    // Pause the game and show the in-game menu
    public void Pause()
    {
        inGameMenuUI.SetActive(true);  // Show the menu
        Time.timeScale = 0f;           // Pause game time
        isPaused = true;               // Update paused state
    }

    // Load the Main Menu scene
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;           // Resume game time before switching scenes
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your actual Main Menu scene name
    }

    // Restart the current game scene
    public void RestartGame()
    {
        Time.timeScale = 1f;  // Ensure the game is unpaused before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reloads the currently active scene
    }
}
