using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqrRoomGateLogic : MonoBehaviour
{
    [Header("Obj")]
    public GameObject collObj1;
    private bool hasCollided = false; // Flag to prevent multiple collisions
    public GameObject GateLockDia; // UI to be displayed when interacting
    public GameObject SquareRoomDia; // UI to be displayed when interacting
    public GameObject SqrRoomFrameToLock; // UI to be displayed when interacting
    public Animator sqrRoomAnimator; // Reference to the Animator component
    public string animationTriggerName = "PlayAnimation"; // Name of the animation trigger

    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding is the player and prevent multiple collisions
        if (!hasCollided && other.gameObject.CompareTag("Player"))
        {

            GateLockDia.SetActive(false);
            SquareRoomDia.SetActive(true);
            //SqrRoomFrameToLock.SetActive(true);

            print("SqrDhadakla");

            // Trigger the animation
            if (sqrRoomAnimator != null)
            {
                sqrRoomAnimator.SetTrigger(animationTriggerName); // Play the animation via the trigger
                Debug.LogError("Gate is Opening broo");
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
