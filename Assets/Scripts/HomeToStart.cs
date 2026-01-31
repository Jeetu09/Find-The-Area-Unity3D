using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeToStart : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadSceneByName()
    {
        SceneManager.LoadScene("Outdoor");
    }
}
