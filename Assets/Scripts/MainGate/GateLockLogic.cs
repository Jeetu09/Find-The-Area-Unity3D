using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLockLogic : MonoBehaviour
{
    public GameObject collObj1;
    public GameObject GateToSqrDia; // UI to be displayed when interacting
    private bool hasCollided = false; // Flag to prevent multiple collisions
    public Animator animator; // Reference to the Animator component
    public string animationTriggerName = "PlayAnimation"; // Name of the animation trigger

    private void start()
    {
        if (GateToSqrDia != null)
        {
            GateToSqrDia.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding is the player and prevent multiple collisions
        if (!hasCollided && other.gameObject.CompareTag("Player"))
        {
            print("Helloooo");
            GateToSqrDia.SetActive(true);


            // Trigger the animation
            if (animator != null)
            {
                animator.SetTrigger(animationTriggerName); // Play the animation via the trigger
            }
            else
            {
                Debug.LogError("Animator not assigned.");
            }

            hasCollided = true; // Set flag to true to prevent further collisions
            collObj1.SetActive(false);
        }
    }
}
