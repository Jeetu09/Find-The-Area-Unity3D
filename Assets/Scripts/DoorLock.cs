using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class DoorLock : MonoBehaviour, IInteractable
{
    [Header("Audio")]
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip Correct; // AudioClip for the correct sound

    [Header("UI Elements")]
    public GameObject NPCDiaUI; // UI to be displayed when interacting
    public GameObject NPCAndPlayerDia;
    public TMP_InputField nameInput; // Input field for name
    public TMP_InputField ageInput; // Input field for age
    public Button cancelButton; // Button to close UI without saving
    public Button NameAndAgeInputButton; // Button to confirm and save name and age

    [Header("Player Control")]
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script

    [Header("Animations")]
    public Animator targetAnimator; // Animator for the first object
    public Animator secondaryAnimator; // Animator for the second object
    public string animationTriggerName = "PlayAnimation"; // Name of the trigger in Animator

    [Header("Color Settings")]
    public string targetHexColor = "#3AAA33"; // Default hex color (green)
    public float fadeDuration = 1f; // Duration for the color change
    public Image statusIndicator; // UI element to indicate status (color change)

    private bool isDetailsSaved = false; // Flag to check if details are saved

    //public PauseMenu pauseMenu()
    //{
    //    Debug.Log("Yes It calld")
    //}

    private void Start()
    {
        // Ensure the audioSource is assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); // Get the AudioSource component from the current GameObject
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on the GameObject.");
            }
        }

        // Find and assign the PlayerMovement component in the scene
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement not found in the scene.");
        }

        // Add listener to the cancel button to close the UI
        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(CloseNPCUI);
        }
        else
        {
            Debug.LogError("Cancel button is not assigned in the Inspector.");
        }

        // Add listener to the NameAndAgeInputButton to save details and play animation
        if (NameAndAgeInputButton != null)
        {
            NameAndAgeInputButton.onClick.AddListener(SaveDetailsAndAnimate);
            NameAndAgeInputButton.interactable = false; // Disable initially
        }
        else
        {
            Debug.LogError("NameAndAgeInputButton is not assigned in the Inspector.");
        }

        // Add listeners to validate and sanitize inputs
        if (nameInput != null && ageInput != null)
        {
            nameInput.onValueChanged.AddListener(SanitizeNameInput);
            ageInput.onValueChanged.AddListener(SanitizeAgeInput);
        }
        else
        {
            Debug.LogError("Name or Age input field is not assigned in the Inspector.");
        }

        // Initialize UI as hidden
        if (NPCDiaUI != null)
        {
            NPCDiaUI.SetActive(false);
        }
    }

    public void InteractRange()
    {
        // Show the NPC UI if details haven't been saved yet
        if (NPCDiaUI != null && !isDetailsSaved)
        {
            NPCDiaUI.SetActive(true); // Show the UI panel
            NPCAndPlayerDia.SetActive(false);

            // Disable player controls and show the cursor
            if (playerMovement != null)
            {
                playerMovement.SetControlEnabled(false);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void CloseNPCUI()
    {
        if (NPCDiaUI != null)
        {
            NPCDiaUI.SetActive(false); // Close the UI without saving
        }

        // Re-enable player controls and hide the cursor
        if (playerMovement != null)
        {
            playerMovement.SetControlEnabled(true);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SanitizeNameInput(string input)
    {
        // Allow only alphabets using regular expressions
        string sanitizedInput = Regex.Replace(input, @"[^a-zA-Z\s]", ""); // Removes any non-alphabetic characters
        if (input != sanitizedInput)
        {
            nameInput.text = sanitizedInput;
        }

        ValidateInputs(null);
    }

    private void SanitizeAgeInput(string input)
    {
        // Allow only integers using regular expressions
        string sanitizedInput = Regex.Replace(input, @"[^0-9]", ""); // Removes any non-numeric characters
        if (input != sanitizedInput)
        {
            ageInput.text = sanitizedInput;
        }

        ValidateInputs(null);
    }

    private void ValidateInputs(string _)
    {
        // Enable NameAndAgeInputButton only if both fields have valid text
        if (!string.IsNullOrEmpty(nameInput.text) && !string.IsNullOrEmpty(ageInput.text))
        {
            NameAndAgeInputButton.interactable = true;
        }
        else
        {
            NameAndAgeInputButton.interactable = false;
        }
    }

    private void SaveDetailsAndAnimate()
    {
        // Save the entered name and age
        string playerName = nameInput.text;
        string playerAge = ageInput.text;

        Debug.Log("Player Name: " + playerName + ", Age: " + playerAge);

        // Trigger animation on the first object (targetAnimator)
        if (targetAnimator != null)
        {
            targetAnimator.SetTrigger(animationTriggerName);

            // Gradually change the UI element color to the target color
            if (statusIndicator != null)
            {
                // Play the correct sound using AudioSource
                if (audioSource != null && Correct != null)
                {
                    audioSource.PlayOneShot(Correct); // Correct way to play the sound
                }
                else
                {
                    Debug.LogError("AudioSource or AudioClip is missing.");
                }

                if (ColorUtility.TryParseHtmlString(targetHexColor, out Color targetColor))
                {
                    StartCoroutine(FadeColor(statusIndicator, targetColor));
                }
                else
                {
                    Debug.LogError("Invalid hex color provided.");
                }
            }
        }
        else
        {
            Debug.LogError("Primary Animator not assigned.");
        }

        // Start a coroutine to wait for the first animation to finish before triggering the second animation
        if (secondaryAnimator != null)
        {
            StartCoroutine(PlaySecondaryAnimationAfterFirst());
        }
        else
        {
            Debug.LogError("Secondary Animator not assigned.");
        }

        // Prevent re-enabling UI by setting flag
        isDetailsSaved = true;

        // Close the UI after saving details
        CloseNPCUI();
    }

    private IEnumerator PlaySecondaryAnimationAfterFirst()
    {
        // Wait for the first animation to finish
        AnimatorStateInfo stateInfo = targetAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // Wait for the duration of the first animation
        yield return new WaitForSeconds(animationLength);

        // Trigger the second animation after the first is completed
        secondaryAnimator.SetTrigger(animationTriggerName);
    }

    private IEnumerator FadeColor(Image image, Color targetColor)
    {
        Color initialColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        image.color = targetColor; // Ensure the final color is exactly the target color
    }
}
