using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this to access TextMeshPro elements

public class CameraSetting : MonoBehaviour
{
    public Image fadeImage; // Drag your black image here in the Unity editor
    public TextMeshProUGUI fadeText;   // Drag the TextMeshPro object here in the Unity editor

    private float fadeDuration = 1.0f; // Duration for the dark screen and text

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Coroutine to handle the fade effect
    IEnumerator FadeOut()
    {
        Color fadeImageColor = fadeImage.color;
        Color fadeTextColor = fadeText.color;
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            // Gradually change the alpha value from 1 (fully visible) to 0 (invisible)
            float alphaValue = Mathf.Lerp(1, 0, (Time.time - startTime) / fadeDuration);

            // Apply the alpha value to both image and text
            fadeImageColor.a = alphaValue;
            fadeTextColor.a = alphaValue;

            fadeImage.color = fadeImageColor;
            fadeText.color = fadeTextColor;

            yield return null;
        }

        // Ensure the fade is fully completed
        fadeImage.color = new Color(fadeImageColor.r, fadeImageColor.g, fadeImageColor.b, 0);
        fadeText.color = new Color(fadeTextColor.r, fadeTextColor.g, fadeTextColor.b, 0);

        // Disable the Image and Text components to stop rendering
        fadeImage.enabled = false;
        fadeText.enabled = false;

    }
}
