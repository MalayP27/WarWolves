using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;

    private bool isGamePaused = false;

    void Start()
    {
        // Assign button actions
        if (pauseMenuUI != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
            restartButton.onClick.AddListener(RestartGame);
            exitButton.onClick.AddListener(ExitGame);
            pauseMenuUI.SetActive(false); // Hide pause menu initially
        }
    }

    void Update()
    {
        // Check for the 'Escape' key to pause/unpause the game
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

    // Method to pause the game
    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // Pause the game
        pauseMenuUI.SetActive(true); // Show pause menu
    }

    // Method to resume the game
    void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // Resume the game
        pauseMenuUI.SetActive(false); // Hide pause menu
    }

    // Method to restart the game
    void RestartGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // Resume normal time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current level
    }

    // Method to exit the game
    void ExitGame()
    {
        Time.timeScale = 1f; // Resume normal time scale before exiting
        SceneManager.LoadScene("TitlePage"); // Return to the title screen
    }
}
