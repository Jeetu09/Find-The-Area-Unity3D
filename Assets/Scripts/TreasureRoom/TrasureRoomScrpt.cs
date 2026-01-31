using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoomScript : MonoBehaviour
{
    [Header("Specific Objects to Turn Green")]
    public Renderer object1; // First specific object
    public Renderer object2; // Second specific object
    public Renderer object3; // Third specific object
    public Renderer object4; // Fourth specific object

    [Header("Animation")]
    public Animator treasureRoomAnimator; // Animator for the treasure room
    public string animationTrigger = "OpenTreasure"; // Trigger name for the animation

    public Color requiredColor = Color.green; // Color we want to check for (green)
    private bool animationTriggered = false; // Flag to prevent multiple triggers

    void Update()
    {
        // Only proceed if the animation hasn't been triggered yet
        if (!animationTriggered && AllSpecificObjectsAreGreen())
        {
            // Trigger the animation if all specific objects are green
            if (treasureRoomAnimator != null)
            {
                treasureRoomAnimator.SetTrigger(animationTrigger);
                animationTriggered = true; // Prevent re-triggering
            }
            else
            {
                Debug.LogError("Treasure room animator is not assigned.");
            }
        }
    }

    // Function to check if all specific objects are green
    private bool AllSpecificObjectsAreGreen()
    {
        // Check if each object is assigned and if it's green
        if (object1 == null || object2 == null || object3 == null || object4 == null)
        {
            Debug.LogError("One or more specific target objects are not assigned.");
            return false;
        }

        return object1.material.color == requiredColor &&
               object2.material.color == requiredColor &&
               object3.material.color == requiredColor &&
               object4.material.color == requiredColor;
    }
}
