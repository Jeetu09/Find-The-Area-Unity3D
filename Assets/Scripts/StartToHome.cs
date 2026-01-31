using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartToHome : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadSceneByName()
    {
        SceneManager.LoadScene("Home Page");
    }

    // Function to exit the game
    public void ExitGame()
    {
        // For exiting the game in a built version
        Application.Quit();

        // This line is for testing in the Unity editor, it won't work in a built game
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
