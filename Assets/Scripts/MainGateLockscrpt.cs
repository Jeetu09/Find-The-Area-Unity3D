using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class MainGateLockscrpt : MonoBehaviour, IInteractable
{
    [Header("Audio")]
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip Correct; // AudioClip for the correct sound


    [Header("Color Settings")]
    public string targetHexColor = "#3AAA33"; // Default hex color (green)
    public float fadeDuration = 1f; // Duration for the color change
    public Image statusIndicator; // UI element to indicate status (color change)

    [Header("UI Elements")]
    public GameObject MainGateLockUI; // UI to be displayed when interacting
    public GameObject GateLockDia; // UI to be displayed when interacting
    public TMP_InputField AnswerInput; // Input field for the answer
    public Button cancelButton; // Button to close UI without saving
    public Button GateAnswerBtn; // Button to confirm and save details

    [Header("Player Control")]
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script

    [Header("Animation")]
    public Animator targetAnimator; // Animator of the object to animate
    public string animationTriggerName = "PlayAnimation"; // Name of the trigger in Animator
    public string reverseAnimationTriggerName = "ReverseAnimation"; // Name of the reverse trigger in Animator

    private bool isDetailsSaved = false; // Flag to check if details are saved

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

        // Add listener to the GateAnswerBtn to save details and play animation
        if (GateAnswerBtn != null)
        {
            GateAnswerBtn.onClick.AddListener(SaveDetailsAndAnimate);
            GateAnswerBtn.interactable = false; // Disable initially
        }
        else
        {
            Debug.LogError("GateAnswerBtn is not assigned in the Inspector.");
        }

        // Add listeners to the input fields to validate input
        if (AnswerInput != null)
        {
            AnswerInput.onValueChanged.AddListener(ValidateInputs);
        }
        else
        {
            Debug.LogError("Answer input field is not assigned in the Inspector.");
        }

        // Initialize UI as hidden
        if (MainGateLockUI != null)
        {
            MainGateLockUI.SetActive(false);
        }

        if (GateLockDia != null)
        {
            GateLockDia.SetActive(false);
        }
    }

    public void InteractRange()
    {
        // Show the NPC UI if details haven't been saved yet
        if (MainGateLockUI != null && !isDetailsSaved)
        {
            MainGateLockUI.SetActive(true); // Show the UI panel
            GateLockDia.SetActive(false); // Show the UI panel

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
        if (MainGateLockUI != null)
        {
            MainGateLockUI.SetActive(false); // Close the UI without saving
        }

        // Re-enable player controls and hide the cursor
        if (playerMovement != null)
        {
            playerMovement.SetControlEnabled(true);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ValidateInputs(string input)
    {
        // Enable GateAnswerBtn only if AnswerInput contains exactly "20"
        GateAnswerBtn.interactable = (AnswerInput.text == "20");
    }

    private void SaveDetailsAndAnimate()
    {
        // Save the entered details (for example, age)
        string playerAge = AnswerInput.text;

        Debug.Log("Player Age: " + playerAge);

        // Trigger animation on target object
        if (targetAnimator != null)
        {
            targetAnimator.SetTrigger(animationTriggerName);

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
            Debug.LogError("Animator not assigned.");
        }

        // Prevent re-enabling UI by setting the flag
        isDetailsSaved = true;

        // Close the UI after saving details
        CloseNPCUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the name is "Player" and this object has the name "Popat"
        if (other.name == "Capsule" && gameObject.name == "MainGateVerti")
        {
            // Trigger reverse animation
            Debug.Log("Dhadakla");
            if (targetAnimator != null)
            {
                targetAnimator.SetTrigger(reverseAnimationTriggerName);
            }
            else
            {
                Debug.LogError("Animator not assigned.");
            }
        }
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
