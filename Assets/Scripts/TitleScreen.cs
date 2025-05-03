using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Text leaderboardText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text leaderboard1Text;
    [SerializeField] private Text leaderboard2Text;
    [SerializeField] private Text leaderboard3Text;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Button submitNameButton;

    private float startTime;
    private bool isTimerRunning = false;

    void Start()
    {
        // Disable start button initially
        startButton.interactable = false;

        // Assign the button click events
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        submitNameButton.onClick.AddListener(SubmitName);

        // Limit name input to 3 characters and capitalize
        nameInputField.characterLimit = 3;
        nameInputField.onValueChanged.AddListener(delegate { CapitalizeName(); });

        // Display the leaderboard
        DisplayLeaderboard();
    }

    void Update()
    {
        // Update the timer display if the timer is running
        if (isTimerRunning)
        {
            float timeElapsed = Time.time - startTime;
            timerText.text = "Time: " + timeElapsed.ToString("F2") + "s";
        }
    }

    // Method to start the game
    void StartGame()
    {
        // Start the timer
        startTime = Time.time;
        isTimerRunning = true;

        // Load the main game scene (replace "GameScene" with your scene's name)
        SceneManager.LoadScene("Level1");
    }

    // Method to submit player's name
    void SubmitName()
    {
        string playerName = nameInputField.text.ToUpper();

        if (playerName.Length == 3)
        {
            GameManager.CurrentPlayerName = playerName;
            startButton.interactable = true; // Enable start button after valid name is entered
        }
    }

    // Capitalize name input
    void CapitalizeName()
    {
        nameInputField.text = nameInputField.text.ToUpper();
    }

    // Method to display the leaderboard
   void DisplayLeaderboard()
    {
        // Display the sorted leaderboard times and names from GameManager
        List<float> leaderboardTimes = GameManager.LeaderboardTimes;
        List<string> leaderboardNames = GameManager.LeaderboardNames;

        // Always place the first entry in the number one slot, and move blanks to the bottom
        if (leaderboardTimes.Count > 0)
        {
            leaderboard1Text.text = leaderboardTimes[0].ToString("F2") + " - " + leaderboardNames[0];
        }
        else
        {
            leaderboard1Text.text = "---";
        }

        if (leaderboardTimes.Count > 1)
        {
            leaderboard2Text.text = leaderboardTimes[1].ToString("F2") + " - " + leaderboardNames[1];
        }
        else
        {
            leaderboard2Text.text = "---";
        }

        if (leaderboardTimes.Count > 2)
        {
            leaderboard3Text.text = leaderboardTimes[2].ToString("F2") + " - " + leaderboardNames[2];
        }
        else
        {
            leaderboard3Text.text = "---";
        }
    }


    // Method to exit the game
    void ExitGame()
    {
        // Quit the application
        Application.Quit();

        // If running in the editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
