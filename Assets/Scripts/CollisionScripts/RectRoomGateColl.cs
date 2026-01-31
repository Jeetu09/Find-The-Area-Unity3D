using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectRoomGateColl : MonoBehaviour
{
    [Header("Obj")]
    public GameObject collObj1;

    [Header("UI")]
    private bool hasCollided = false; // Flag to prevent multiple collisions
    public GameObject GateLockDia; // UI to be displayed when interacting
    public GameObject SquareRoomDia; // UI to be displayed when interacting
    public GameObject ReactangleRoomDia; // UI to be displayed when interacting
    public GameObject SqrRoomFrameToLock; // UI to be displayed when interacting
    public GameObject SqrRoomToRectRoomDia; // UI to be displayed when interacting

    [Header("Animator")]
    public Animator sqrroomgate; // Reference to the Animator component
    public string sqrroomgatetrgr = "PlayAnimation"; // Name of the animation trigger


    public Animator RectRoomGateDown; // Reference to the Animator component
    public string RectRoomGateDownTrigger = "PlayAnimation"; // Name of the animation trigger



    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding is the player and prevent multiple collisions
        if (!hasCollided && other.gameObject.CompareTag("Player"))
        {
            SquareRoomDia.SetActive(false);
            SqrRoomToRectRoomDia.SetActive(false);
            ReactangleRoomDia.SetActive(true);
            ////SqrRoomFrameToLock.SetActive(true);

            //print("SqrDhadakla");

            // Trigger the animation
            if (RectRoomGateDown != null)
            {
                RectRoomGateDown.SetTrigger(RectRoomGateDownTrigger); // Play the animation via the trigger
                Debug.LogError("Gate is Opening broo");
            }

            if (sqrroomgate != null)
            {
                sqrroomgate.SetTrigger(sqrroomgatetrgr); // Play the animation via the trigger
                Debug.LogError("Gate is Opening broo");
            }

            hasCollided = true; // Set flag to true to prevent further collisions
            collObj1.SetActive(false);

        }
    }
}
