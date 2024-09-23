using UnityEngine;
using UnityEngine.UI; // For displaying the timer using UI
using System.Collections;

public class TimerController : MonoBehaviour
{
    public Text timerText;  // UI Text element to display the timer
    private float timeElapsed = 0f;  // Time in seconds
    private bool isTimerRunning = true;  // Control if the timer is active

    void Update()
    {
        // If the timer is running, update the time
        if (isTimerRunning)
        {
            // Increment the timer
            timeElapsed += Time.deltaTime;

            // Update the timer display
            UpdateTimerDisplay();
        }
    }

    // Call this method to reduce time (for example, when collecting a pickup)
    public void ReduceTime(float amount)
    {
        // Subtract time (e.g., 5 seconds) but don't let the time go below 0
        timeElapsed = Mathf.Max(0f, timeElapsed - amount);

        // Update the timer display immediately
        UpdateTimerDisplay();
    }

    // Method to update the timer text display
    void UpdateTimerDisplay()
    {
        // Format timeElapsed as minutes:seconds (00:00 format)
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        // Display the time
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
