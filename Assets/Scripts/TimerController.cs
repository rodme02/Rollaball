using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // Reference to the TMP Text for the timer

    private float timeElapsed = 0f;    // Tracks time in seconds
    private bool isTimerRunning = true;

    void Update()
    {
        if (isTimerRunning && !GameManager.Instance.isGamePaused)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Method to reduce time (called when a pickup is collected)
    public void ReduceTime(float amount)
    {
        // Subtract time but don't let it go below zero
        timeElapsed = Mathf.Max(0f, timeElapsed - amount);
        UpdateTimerDisplay();
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
        UpdateTimerDisplay();
    }
}
