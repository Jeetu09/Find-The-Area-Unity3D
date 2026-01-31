using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class Rectanglescrpt : MonoBehaviour, IInteractable
{

    [Header("Audio")]
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip Correct; // AudioClip for the correct sound



    [Header("Color Settings")]
    public string targetHexColor = "#3AAA33"; // Default hex color (green)
    public float fadeDuration = 1f; // Duration for the color change
    public Image statusIndicator; // UI element to indicate status (color change)

    [Header("Animation")]
    public Animator RectGateUnlock;
    public string animationTriggerName = "PlayAnimation";

    public Animator TriannleRoomGateUp;
    public string TriangleGateUpTrigger = "PlayAnimation";

    [Header("Pop-up")]
    public GameObject RectangleRoomFrameToLockDia;
    public GameObject RectangleRoomtoTriangleRoom;

    [Header("UI Elements")]
    public GameObject MainGateLockUI; // UI to be displayed when interacting
    public TMP_InputField AnswerInput; // Input field for the answer
    public Button cancelButton; // Button to close UI without saving
    public Button GateAnswerBtn; // Button to confirm and save details

    [Header("Player Control")]
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script

    [Header("Target Object")]
    public Renderer targetRenderer; // Renderer of the object to change color
    public Color newColor = Color.green; // The color to change to

    [Header("Random Number Display")]
    public RandomNumberDisplay randomNumberDisplay; // Reference to RandomNumberDisplay

    private bool isDetailsSaved = false; // Flag to check if details are saved

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
            GateAnswerBtn.interactable = false; // Disable initially
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

        if (randomNumberDisplay == null)
        {
            Debug.LogError("RandomNumberDisplay is not assigned in the Inspector.");
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

        RectangleRoomFrameToLockDia.SetActive(false);
    }

    private void CloseNPCUI()
    {
        if (MainGateLockUI != null)
        {
            MainGateLockUI.SetActive(false);
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
        if (randomNumberDisplay != null)
        {
            int lenght = 4 * randomNumberDisplay.randomNumber;
            int bradth = 3 * randomNumberDisplay.randomNumber;
            int correctAnswer = lenght * bradth;
            
            GateAnswerBtn.interactable = AnswerInput.text == correctAnswer.ToString();
        }
    }

    private void SaveDetailsAndAnimate()
    {
        if (targetRenderer != null)
        {
            if (RectGateUnlock != null)
            {
                RectGateUnlock.SetTrigger(animationTriggerName);

                // Play the correct sound using AudioSource
                if (audioSource != null && Correct != null)
                {
                    audioSource.PlayOneShot(Correct); // Correct way to play the sound
                }
                else
                {
                    Debug.LogError("AudioSource or AudioClip is missing.");
                }


                if (statusIndicator != null)
                {
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
            RectangleRoomFrameToLockDia.SetActive(false);
            RectangleRoomtoTriangleRoom.SetActive(true);

            targetRenderer.material.color = newColor;
            TriannleRoomGateUp.SetTrigger(TriangleGateUpTrigger);
        }
        else
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
