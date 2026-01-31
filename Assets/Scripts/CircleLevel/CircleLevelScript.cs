using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CircleLevelScript : MonoBehaviour, IInteractable
{

    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip Correct; // AudioClip for the correct sound


    [Header("Color Settings")]
    public string targetHexColor = "#3AAA33"; // Default hex color (green)
    public string additionalRenderer = "#3AAA33"; // Default hex color (green)
    public float fadeDuration = 1f; // Duration for the color change
    public Image statusIndicator; // UI element to indicate status (color change)
    public Image statusIndicatortwo; // Second UI element to indicate status

    [Header("UI")]
    public GameObject CircleLevelDial; // UI to be displayed when interacting
    public GameObject CircleGateToFinishDia; // UI to be displayed when interacting
    public GameObject TriangleDia; // UI to be displayed when interacting

    [Header("Animations")]
    public Animator CircleRoomGateUnlocked; // Reference to the Animator component
    public string CircleRoomGateUnlockedTrigger = "PlayAnimation"; // Name of the animation trigger

    [Header("UI Elements")]
    public GameObject MainGateLockUI; // UI to be displayed when interacting
    public TMP_InputField AnswerInput; // Input field for the answer
    public Button cancelButton; // Button to close UI without saving
    public Button GateAnswerBtn; // Button to confirm and save details

    [Header("Player Control")]
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script

    [Header("Animation")]
    public Animator targetAnimator; // Animator of the object to animate
    private bool isDetailsSaved = false; // Flag to check if details are saved
    public string wheelRollTrigger = "WheelRoll"; // Define the trigger for the animation

    [Header("Color Change")]
    public Renderer targetRenderer; // Renderer of the object to change color
    public Color correctAnswerColor = Color.green; // Color to apply if answer is correct

    private void Start()
    {

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); // Get the AudioSource component from the current GameObject
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on the GameObject.");
            }
        }

        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement not found in the scene.");
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(CloseNPCUI);
        }
        else
        {
            Debug.LogError("Cancel button is not assigned in the Inspector.");
        }

        if (GateAnswerBtn != null)
        {
            GateAnswerBtn.onClick.AddListener(SaveDetailsAndAnimate);
            GateAnswerBtn.interactable = false;
        }
        else
        {
            Debug.LogError("GateAnswerBtn is not assigned in the Inspector.");
        }

        if (AnswerInput != null)
        {
            AnswerInput.onValueChanged.AddListener(ValidateInputs);
        }
        else
        {
            Debug.LogError("Answer input field is not assigned in the Inspector.");
        }

        if (MainGateLockUI != null)
        {
            MainGateLockUI.SetActive(false);
        }

        if (targetAnimator != null)
        {
            targetAnimator.ResetTrigger(wheelRollTrigger); // Ensure the trigger is reset initially
        }
    }

    public void InteractRange()
    {
        if (MainGateLockUI != null && !isDetailsSaved)
        {
            MainGateLockUI.SetActive(true);

            if (playerMovement != null)
            {
                playerMovement.SetControlEnabled(false);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        TriangleDia.SetActive(false);
        CircleLevelDial.SetActive(false);
    }

    private void CloseNPCUI()
    {
        if (MainGateLockUI != null)
        {
            MainGateLockUI.SetActive(false);
            TriangleDia.SetActive(false);
        }

        if (playerMovement != null)
        {
            playerMovement.SetControlEnabled(true);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ValidateInputs(string input)
    {
        GateAnswerBtn.interactable = (AnswerInput.text == "7.9");
    }

    private void SaveDetailsAndAnimate()
    {
        string playerAnswer = AnswerInput.text;
        Debug.Log("Player Answer: " + playerAnswer);
        TriangleDia.SetActive(false);

        // Trigger animation if animator is assigned
        if (playerAnswer == "7.9" && targetRenderer != null)
        {
            targetRenderer.material.color = correctAnswerColor; // Change color on correct answer
            CircleGateToFinishDia.SetActive(true);

            if (CircleRoomGateUnlocked != null)
            {
                CircleRoomGateUnlocked.SetTrigger(CircleRoomGateUnlockedTrigger);

                if (audioSource != null && Correct != null)
                {
                    audioSource.PlayOneShot(Correct); // Correct way to play the sound
                }
                else
                {
                    Debug.LogError("AudioSource or AudioClip is missing.");
                }


                if (statusIndicator != null && statusIndicatortwo != null)
                {
                    if (ColorUtility.TryParseHtmlString(targetHexColor, out Color targetColor) &&
                        ColorUtility.TryParseHtmlString(additionalRenderer, out Color additionalColor))
                    {
                        // Start fade operations for both indicators in parallel
                        StartCoroutine(FadeColor(statusIndicator, targetColor));
                        StartCoroutine(FadeColor(statusIndicatortwo, additionalColor));
                    }
                    else
                    {
                        if (!ColorUtility.TryParseHtmlString(targetHexColor, out _))
                        {
                            Debug.LogError("Invalid hex color for targetHexColor.");
                        }
                        if (!ColorUtility.TryParseHtmlString(additionalRenderer, out _))
                        {
                            Debug.LogError("Invalid hex color for additionalRenderer.");
                        }
                    }
                }
                CircleLevelDial.SetActive(false);
                TriangleDia.SetActive(false);
            }
        }
        else if (targetRenderer == null)
        {
            Debug.LogError("Target Renderer is not assigned.");
        }

        isDetailsSaved = true;
        CloseNPCUI();
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
