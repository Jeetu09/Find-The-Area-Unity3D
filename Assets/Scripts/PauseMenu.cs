using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the pause menu UI panel
    public Button CancelButton;    // Reference to the CancelButton (UI Button)
    public Button RestartButton;  // Reference to the RestartButton (UI Button)
    private bool isPaused = false; // Keeps track if the game is paused or not

    private PlayerMovement playerMovement; // Reference to the player's movement script

    private void Start()
    {
        // Ensure the pause menu is initially hidden
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        // Find the PlayerMovement script in the scene
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Make sure the cursor is hidden when the game starts
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Assign the ResumeGame function to the CancelButton's onClick event
        if (CancelButton != null)
        {
            CancelButton.onClick.AddListener(ResumeGame);
        }

        // Assign the RestartScene function to the RestartButton's onClick event
        if (RestartButton != null)
        {
            RestartButton.onClick.AddListener(RestartScene);
        }
    }

    private void Update()
    {
        // Toggle the pause menu when H is pressed
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && !isPaused)
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        // Deactivate the pause menu
        pauseMenuUI.SetActive(false);

        // Re-enable player movement
        if (playerMovement != null)
        {
            playerMovement.SetControlEnabled(true);
        }

        // Lock the cursor and hide it again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Unpause the game
        Time.timeScale = 1f; // Resume normal time flow
        isPaused = false;
    }

    public void PauseGame()
    {
        // Activate the pause menu
        pauseMenuUI.SetActive(true);

        // Disable player movement
        if (playerMovement != null)
        {
            playerMovement.SetControlEnabled(false);
        }

        // Unlock the cursor and show it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause the game
        Time.timeScale = 0f; // Stop time flow
        isPaused = true;
    }

    // Function to reload the current scene
    public void RestartScene()
    {
        // Reload the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Unpause the game to ensure proper state
        Time.timeScale = 1f;
    }

    // Function to exit the game
    public void ExitGame()
    {
        // Resume the game to avoid potential issues when loading the home page
        Time.timeScale = 1f;

        // Load the "Home Page" scene
        SceneManager.LoadScene("Home Page");
    }
}
