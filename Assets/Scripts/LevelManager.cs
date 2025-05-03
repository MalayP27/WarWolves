using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Text timerText;

    private float startTime;
    private bool isTimerRunning = false;

    void Start()
    {
        // Start the timer when the level begins
        startTime = Time.time;
        isTimerRunning = true;
    }

    void Update()
    {
        // Update the timer display if the timer is running
        if (isTimerRunning)
        {
            float timeElapsed = Time.time - startTime;
            timerText.text = "Time: " + timeElapsed.ToString("F2") + "s";
        }

        // Check if level is complete (replace with actual completion logic)
        if (CheckIfLevelCompleted())
        {
            isTimerRunning = false;
            UpdateLeaderboard(timeElapsed: Time.time - startTime);
            SceneManager.LoadScene("TitleScreen");
        }
    }

    // Placeholder method to check if the level is complete
    bool CheckIfLevelCompleted()
    {
        // Replace this with the actual logic for determining when the level is complete
        return false;
    }

    // Method to update the leaderboard based on completion time
    void UpdateLeaderboard(float timeElapsed)
    {
        // Here you could save the new leaderboard data for later display in TitleScreen
        Debug.Log($"New Player - {timeElapsed:F2} seconds");
    }
}
