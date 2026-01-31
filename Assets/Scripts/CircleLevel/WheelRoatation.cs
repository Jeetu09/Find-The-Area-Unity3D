using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRoatation : MonoBehaviour, IInteractable
{

    [Header("Animations")]
    public Animator WheelAnimation; // Reference to the Animator component
    public string WheelANimationTrigger = "PlayAnimation"; // Name of the animation trigger

    // Start is called before the first frame update
    public void InteractRange()
    {
        if (WheelAnimation != null)
        {
            WheelAnimation.SetTrigger(WheelANimationTrigger); // Activate animation on interaction
        }
        else
        {
            Debug.LogError("Animator not assigned.");
        }
    }
}
