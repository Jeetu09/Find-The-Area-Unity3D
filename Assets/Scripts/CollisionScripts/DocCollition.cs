using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocCollition : MonoBehaviour
{
    public GameObject GateLockDia; // UI to be displayed when interacting
    private bool hasCollided = false; // Flag to prevent multiple collisions

    private void start()
    {
        if (GateLockDia != null)
        {
            GateLockDia.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding is the player and prevent multiple collisions
        if (!hasCollided && other.gameObject.CompareTag("Player"))
        {
            print("Player On Doc");
            GateLockDia.SetActive(true);
            hasCollided = true; // Set flag to true to prevent further collisions
        }
    }
}
