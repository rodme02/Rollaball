using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGamePaused { get; private set; }
    public bool isGameOver { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Play the appropriate background music based on the current scene
        PlaySceneMusic();
    }

    void PlaySceneMusic()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu")
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
        }
        else if (currentScene == "MiniGame")
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameSceneMusic);
        }
    }

    void Update()
    {
        // Handle the Escape key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Pauses the game and shows the in-game menu
    public void PauseGame()
    {
        if (isGameOver) return; // Don't pause if the game is over

        isGamePaused = true;
        Time.timeScale = 0f; // Pause the game
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);  // Play a sound effect
        UIManager.Instance.ShowInGameMenu(true);  // Show the in-game menu
    }

    // Resumes the game and hides the in-game menu
    public void ResumeGame()
    {
        if (isGameOver) return; // Don't resume if the game is over

        isGamePaused = false;
        Time.timeScale = 1f;  // Resume the game
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);  // Play a sound effect
        UIManager.Instance.ShowInGameMenu(false);  // Hide the in-game menu
    }

    // Game Over logic
    public void GameOver()
    {
        isGamePaused = true;
        isGameOver = true;
        Time.timeScale = 0f;  // Pausing the game time, should not affect music

        // Ensure the game over UI is displayed correctly
        UIManager.Instance.ShowGameOverMenu(true);

        AudioManager.Instance.StopMusic();  // Stop background music
        
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverSound); // Play game over sound

        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);  // Play main menu music
    }

    // This method will be called when the player wins
    public void WinGame()
    {
        isGamePaused = true;               // Pause the game
        isGameOver = true;            // Set game over state
        Time.timeScale = 0f;               // Stop the game time
        UIManager.Instance.ShowWinMenu(true);  // Show the win menu
        AudioManager.Instance.StopMusic();    // Stop the background music
        AudioManager.Instance.PlaySFX(AudioManager.Instance.winSound);  // Optional: play a win sound
        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);  // Play main menu music
    }

    // Call to load the main menu
    public void LoadMainMenu()
    {
        UIManager.Instance.HideAllGameUI();

        Time.timeScale = 1;
        isGameOver = false;
        isGamePaused = false;

        // Force stop and play main menu music explicitly when loading the main menu
        AudioManager.Instance.StopMusic();  // Ensure any existing music is stopped
        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);  // Play main menu music

        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);
        SceneLoader.Instance.LoadScene("MainMenu");
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        isGameOver = false;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.gameSceneMusic);
        UIManager.Instance.ShowGameOverMenu(false);
        SceneLoader.Instance.ReloadCurrentScene();
    }
}
