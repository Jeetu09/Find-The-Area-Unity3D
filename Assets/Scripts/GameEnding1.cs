using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding1 : MonoBehaviour
{
    // Method to exit the current scene and load "Home Page"
    public void ExitGame()
    {
        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load the "Home Page" scene
        SceneManager.LoadScene("Home Page", LoadSceneMode.Single);
    }
}
