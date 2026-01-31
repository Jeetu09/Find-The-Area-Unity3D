using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    public Animator doorAnimator; // Reference to the Animator for the door

    private void Start()
    {
        // Ensure the Animator is assigned
        if (doorAnimator == null)
        {
            Debug.LogError("Door Animator is not assigned in the Inspector.");
        }
        else
        {
            doorAnimator.speed = 0; // Set initial animation speed to 0
        }
    }

    // Method to change the animation speed to 1 when the button is clicked
    public void PlayDoorAnimation()
    {
        if (doorAnimator != null)
        {
            doorAnimator.speed = 1; // Change the speed of the animation to 1
            doorAnimator.SetTrigger("TROpen"); // Trigger the OpenDoor animation
            Debug.Log("Animation speed changed to 1 and playing door animation");
        }
        else
        {
            Debug.LogError("Door Animator is not assigned.");
        }
    }
}
