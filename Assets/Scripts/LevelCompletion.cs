using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompletion : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private string enemyTag = "enemy";
    [SerializeField] private Text timerText;

    private bool keyCollected = false;
    private GameObject[] enemies;
    private float startTime;

    private void Start()
    {
        // Find all enemies at the start of the level
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // Hide the level complete UI initially
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }

        // Assign button click events
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(GoToNextLevel);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitToTitleScreen);
        }

        // Start the timer
        startTime = Time.time;
    }

    private void Update()
    {
        // Update the timer display
        float timeElapsed = Time.time - startTime;
        timerText.text = "Time: " + timeElapsed.ToString("F2") + "s";
    }

    // Method called by KeyCollectible script when the key is picked up
    public void KeyCollected()
    {
        keyCollected = true;
    }

    // Property to access if key is collected
    public bool KeyCollectedStatus => keyCollected;

    // Method to determine if all enemies are defeated
    public bool AreAllEnemiesDefeated()
    {
        // GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag(enemyTag);
        // foreach (GameObject enemy in currentEnemies)
        // {
        //     if (enemy.activeInHierarchy)
        //     {
        //         return false;
        //     }
        // }
        return true;
    }

    public void LevelComplete()
    {
        // Calculate time taken to complete the level
        float timeElapsed = Time.time - startTime;

        // Only update leaderboard if current scene is "Level1"
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            GameManager.AddTimeToLeaderboard(timeElapsed);
        }

        // Show level complete UI
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;
    }

    // Method to load the next level
    private void GoToNextLevel()
    {
        Time.timeScale = 1f; // Resume normal time scale
        SceneManager.LoadScene(nextLevelName);
    }

    // Method to exit to the title screen
    private void ExitToTitleScreen()
    {
        Time.timeScale = 1f; // Resume normal time scale
        SceneManager.LoadScene("TitlePage");
    }
}
