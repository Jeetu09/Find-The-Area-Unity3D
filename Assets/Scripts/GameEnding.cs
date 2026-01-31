using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
public class GameEnding : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource; // Reference to the first AudioSource
    public AudioClip Correct; // AudioClip for the correct sound
    [SerializeField] AudioSource secondAudioSource; // Reference to the second AudioSource
    public AudioClip SecondAudio; // AudioClip for the second sound

    public GameObject EndingUI;
    public TMP_InputField nameInput;  // Input field for name from DoorLock script
    public GameObject NameOfPlayer;   // UI Text that will display the name
    public GameObject StatusBarUI;

    [Header("UI")]
    public GameObject CircleGateToFinishDia; // UI to be displayed when interacting
    public Image gameEndingBackground; // Assign in Inspector
    public TextMeshProUGUI gameEndingText; // Assign in Inspector

    [Header("Animation")]
    public Animator animator; // Assign Animator in Inspector
    public string animationTriggerName = "PlayAnimation"; // Name of the trigger in Animator

    private bool hasCollided = false; // Flag to prevent multiple collisions
    private PlayerMovement playerMovement; // Reference to the player's movement script

    private bool isSecondAudioMuted = false; // Track if SecondAudio is muted

    void Start()
    {
        secondAudioSource.clip = SecondAudio;
        secondAudioSource.Play();
        EndingUI.SetActive(false);  // Initially hide the ending UI
    }

    void Update()
    {
        // Check for 'M' key press to toggle mute
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleSecondAudioMute();
        }
    }

    void ToggleSecondAudioMute()
    {
        isSecondAudioMuted = !isSecondAudioMuted; // Toggle the mute state

        // Set volume based on mute state
        secondAudioSource.volume = isSecondAudioMuted ? 0 : 1;

        // Log the mute state for debugging
        Debug.Log(isSecondAudioMuted ? "SecondAudio is muted" : "SecondAudio is unmuted");
    }

    void OnTriggerEnter(Collider other)
    {
        CircleGateToFinishDia.SetActive(false);

        if (!hasCollided && other.gameObject.CompareTag("Gold"))
        {
            hasCollided = true; // Set flag to true to prevent further collisions
            StartCoroutine(FadeInUI());
            StatusBarUI.SetActive(false);
            // Play the correct sound using AudioSource
            if (audioSource != null && Correct != null)
            {
                audioSource.PlayOneShot(Correct); // Play the first sound
            }
            else
            {
                Debug.LogError("AudioSource or AudioClip is missing.");
            }

            // Play the second sound using the second AudioSource after a delay and fade it out
            if (secondAudioSource != null && SecondAudio != null)
            {
                StartCoroutine(PlaySecondAudioAfterDelay(1.0f)); // Play after 1 second delay
            }
            else
            {
                Debug.LogError("Second AudioSource or AudioClip is missing.");
            }
        }
    }

    // Coroutine to play the second audio after a delay and fade it out
    private System.Collections.IEnumerator PlaySecondAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay time
        secondAudioSource.PlayOneShot(SecondAudio); // Play the second audio clip

        // Fade out the second audio over a period of 3 seconds
        float fadeDuration = 3.0f; // Duration of the fade-out
        float startTime = Time.time;
        float startVolume = secondAudioSource.volume; // Store the starting volume

        while (Time.time < startTime + fadeDuration)
        {
            // Gradually decrease the volume over time
            float t = (Time.time - startTime) / fadeDuration; // Calculate progress
            secondAudioSource.volume = Mathf.Lerp(startVolume, 0, t); // Fade to 0 (mute)

            yield return null;
        }

        // Ensure the volume is set to 0 after the fade-out
        secondAudioSource.volume = 0;
    }

    // Coroutine to fade in UI elements
    private System.Collections.IEnumerator FadeInUI()
    {
        float fadeDuration = 3.0f; // Duration of fade-in
        Color bgColor = gameEndingBackground.color;
        Color textColor = gameEndingText.color;

        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            // Calculate the fade-in progress
            float alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / fadeDuration);
            bgColor.a = alpha;
            textColor.a = alpha;

            // Apply alpha to both UI elements
            gameEndingBackground.color = bgColor;
            gameEndingText.color = textColor;

            yield return null;
        }

        // Ensure UI elements are fully visible
        gameEndingBackground.color = new Color(bgColor.r, bgColor.g, bgColor.b, 1);
        gameEndingText.color = new Color(textColor.r, textColor.g, textColor.b, 1);

        // Call the scene load function after the fade-in effect completes
        LastUI();
    }

    public void LastUI()
    {
        EndingUI.SetActive(true);

        // Enable and unlock the mouse pointer
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true; // Makes the cursor visible

        // Disable player movement (similar to the PauseMenu script)
        playerMovement = FindObjectOfType<PlayerMovement>(); // Ensure we have the player movement reference
        if (playerMovement != null)
        {
            playerMovement.SetControlEnabled(false); // Disable player controls
        }

        // Show the name of the player
        UpdatePlayerName();
    }

    public void UpdatePlayerName()
    {
        // Ensure nameInput is not null
        if (nameInput != null && NameOfPlayer != null)
        {
            string playerName = nameInput.text;  // Get the player's name from the input field

            if (!string.IsNullOrEmpty(playerName))
            {
                // Update the UI Text with the player's name
                NameOfPlayer.GetComponent<TextMeshProUGUI>().text = "\"" + playerName + "\"";
            }
        }
    }

    public void SavePlayerName()
    {
        // Get the name entered in the input field
        string playerName = nameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            Debug.Log("\" " + playerName + " \""); // Log the entered name

            // Close the UI after saving the name
            EndingUI.SetActive(false);

            // Re-enable player movement and hide the cursor
            if (playerMovement != null)
            {
                playerMovement.SetControlEnabled(true);
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Debug.LogWarning("Name cannot be empty.");
        }
    }
}
