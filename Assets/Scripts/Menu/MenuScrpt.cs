using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScrpt : MonoBehaviour
{
    public GameObject MenuBar; // The UI object to toggle
    public PlayerMovement playerMovement; // Reference to PlayerMovement script

    private bool isMenuActive = false; // Tracks whether the menu is active

    void Start()
    {
        if (MenuBar != null)
        {
            MenuBar.SetActive(false); // Ensure the menu is initially inactive
        }

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script is not assigned! Please assign it in the inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        isMenuActive = !isMenuActive;

        // Toggle the menu visibility
        if (MenuBar != null)
        {
            MenuBar.SetActive(true);
        }

        // Enable or disable player movement
        if (playerMovement != null)
        {
            playerMovement.SetControlEnabled(!isMenuActive);
        }
    }
}
