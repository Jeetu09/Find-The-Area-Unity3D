using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CircleRobo : MonoBehaviour, IInteractable
{
    [Header("UI Elements")]
    public GameObject HelpingRoboSqrUI; // UI to be displayed when interacting
    public Button cancelButton; // Button to close UI without saving

    [Header("Player Control")]
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script

    [Header("Focus Control")]
    public Transform HelpingRoboRectObj; // Object that should focus on the target
    public Transform targetObject; // Target object that HelpingRoboRectObj should focus on


    private bool isDetailsSaved = false; // Flag to check if details are saved

    private void Start()
    {
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



        // Add listeners to the input fields to validate input


        // Initialize UI as hidden
        if (HelpingRoboSqrUI != null)
        {
            HelpingRoboSqrUI.SetActive(false);
        }
    }

    public void InteractRange()
    {
        // Show the NPC UI if details haven't been saved yet
        if (HelpingRoboSqrUI != null && !isDetailsSaved)
        {
            HelpingRoboSqrUI.SetActive(true); // Show the UI panel

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
        if (HelpingRoboSqrUI != null)
        {
            HelpingRoboSqrUI.SetActive(false); // Close the UI without saving
        }

        // Re-enable player controls and hide the cursor
        if (playerMovement != null)
        {
            playerMovement.SetControlEnabled(true);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Ensure HelpingRoboRectObj always faces the target on the Y-axis only
        if (HelpingRoboRectObj != null && targetObject != null)
        {
            Vector3 direction = targetObject.position - HelpingRoboRectObj.position;
            direction.y = 0; // Keep the rotation only on the Y-axis

            // Calculate the look rotation and add a 180-degree offset on the Y-axis
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation *= Quaternion.Euler(0, 180, 0); // Adjust to face the front of the object

            // Apply the adjusted rotation to HelpingRoboRectObj
            HelpingRoboRectObj.rotation = targetRotation;
        }
    }



    private void SaveDetailsAndAnimate()
    {
        CloseNPCUI();
    }
}
