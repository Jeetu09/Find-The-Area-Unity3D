using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleRoomGateScript : MonoBehaviour
{
    [Header("Obj")]
    public GameObject collObj;

    [Header("UI")]
    public GameObject TraiangleRoomDia; // UI to be displayed when interacting
    public GameObject RectRoomToTriangleRoomDia; // UI to be displayed when interacting
    public GameObject RectRoomDia; // UI to be displayed when interacting
    private bool hasCollided = false; // Flag to prevent multiple collisions

    [Header("Animator")]
    public Animator rectroomgate; // Reference to the Animator component
    public string rectroomgatetrgr = "PlayAnimation"; // Name of the animation trigger


    public Animator TriangleRoomGateDown; // Reference to the Animator component
    public string RectRoomGateDownTrigger = "PlayAnimation"; // Name of the animation trigger

 


    void OnTriggerEnter(Collider other)
    {

        RectRoomDia.SetActive(false);
        RectRoomToTriangleRoomDia.SetActive(false);
        TraiangleRoomDia.SetActive(true);
        // Check if the object colliding is the player and prevent multiple collisions
        if (!hasCollided && other.gameObject.CompareTag("Player"))
        {
            if (TriangleRoomGateDown != null)
            {
                TriangleRoomGateDown.SetTrigger(RectRoomGateDownTrigger);
            }

            if (rectroomgate != null)
            {
                rectroomgate.SetTrigger(rectroomgatetrgr);
            }


            else
            {
                Debug.LogError("Animator not assigned.");
            }

            hasCollided = true; // Set flag to true to prevent further collisions
            collObj.SetActive(false);
        }


    }
}
