using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLevelCollision : MonoBehaviour
{
    private bool hasCollided = false; // Flag to prevent multiple triggers

    [Header("Animator")]
    public Animator triangleroomgate; // Reference to the Animator component
    public string triangleroomgatetrgr = "PlayAnimation"; // Name of the animation trigger

    [Header("UI")]
    public GameObject TriangleToCircleLevelDia; // UI to be displayed when interacting
    public GameObject CircleLevelDia; // UI to be displayed when interacting
    public GameObject CircleRoomColl;

    [Header("Animation")]
    public Animator CircleRoomGateDown; // Reference to the Animator component
    public string CircleRoomGateDownTrigger = "PlayAnimation"; // Name of the animation trigger

    void OnTriggerEnter(Collider other)
    {
        TriangleToCircleLevelDia.SetActive(false);
        // Ensure this function runs only once
        if (hasCollided) return;

        // Check if the colliding object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            hasCollided = true; // Mark as already triggered

            CircleLevelDia.SetActive(true);
            TriangleToCircleLevelDia.SetActive(false);

            // Trigger animation if Animator is assigned
            if (CircleRoomGateDown != null)
            {
                CircleRoomGateDown.SetTrigger(CircleRoomGateDownTrigger);
            }

            if (triangleroomgate != null)
            {
                triangleroomgate.SetTrigger(triangleroomgatetrgr);
            }

            else
            {
                Debug.LogError("Animator not assigned.");
            }
        }

        CircleRoomColl.SetActive(false);
    }
}
