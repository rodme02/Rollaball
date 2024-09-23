using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject inGameMenu;     // Pause menu UI
    public GameObject gameOverMenu;   // Game over menu UI
    public GameObject winMenu;        // Win menu UI
    public GameObject HUD;            // HUD UI
    public TextMeshProUGUI scoreText; // Score text (TMP)
    public TextMeshProUGUI livesText; // Lives text (TMP)

    private int score = 0;

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

    public void ShowHUD(bool show)
    {
        HUD.SetActive(show);
    }

    // Show or hide the in-game menu
    public void ShowInGameMenu(bool show)
    {
        // Only proceed if the game is not over
        if (!gameOverMenu.activeSelf)
        {
            inGameMenu.SetActive(show);
        }
    }

    // Show the game over menu
    public void ShowGameOverMenu(bool show)
    {
        gameOverMenu.SetActive(show);
    }

    public void ShowWinMenu(bool show)
    {
        winMenu.SetActive(show);
    }

    // Hide all game UI elements
    public void HideAllGameUI()
    {
        inGameMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        HUD.SetActive(false);
    }

    // Update the score UI
    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    // Update the lives UI
    public void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + lives;

        if (lives <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
}
