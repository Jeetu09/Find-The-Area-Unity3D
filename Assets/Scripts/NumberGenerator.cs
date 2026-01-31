using UnityEngine;

public class NumberGenerator : MonoBehaviour, IInteractable
{
    public Transform InteractorSource;
    public GameObject SquareRoomDia;
    public GameObject SqrRoomFrameToLock;
    private bool isAttached = false;
    //private bool attachecounter = false;
    private Rigidbody rb;

    private Transform currentParent;

    private bool isPlacedInWallCube = false;
    private static int detachCallCount = 0;
    private static bool actionTriggered = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (SqrRoomFrameToLock != null)
        {
            SqrRoomFrameToLock.SetActive(false);
        }
    }

    public void AttachTo(Transform target)
    {
        if (isAttached) return;

        currentParent = target;
        transform.position = target.position;
        transform.rotation = target.rotation;
        transform.SetParent(target);
        isAttached = true;

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        Debug.Log("Object attached to: " + target.name);

        //attachecounter = true;
    }

    public void InteractRange()
    {
        if (InteractorSource != null && !isAttached && !isPlacedInWallCube)
        {
            AttachTo(InteractorSource);
        }
        else if (isPlacedInWallCube)
        {
            Debug.Log("Cube is placed in a wall cube and cannot be re-attached.");
        }
    }

    public void Detach(Transform wallCubeTransform)
    {
        transform.SetParent(null);
        isAttached = false;
        currentParent = null;

        if (wallCubeTransform != null)
        {
            transform.position = wallCubeTransform.position;
            transform.rotation = wallCubeTransform.rotation;
            isPlacedInWallCube = true;
        }
        else
        {
            Debug.LogError("Wall cube transform is null! Cannot place the cube.");
        }

        Debug.Log("Object detached and moved to wall cube position");

        detachCallCount++;

        if (detachCallCount == 8 && !actionTriggered)
        {
            actionTriggered = true;
            SquareRoomDia.SetActive(false);
            SqrRoomFrameToLock.SetActive(true);
            Debug.Log("Action triggered for detach call count = 8.");
        }
    }

    public bool IsPlacedInWallCube()
    {
        return isPlacedInWallCube;
    }

    public static bool IsActionTriggered()
    {
        return actionTriggered; // Return the status of the flag
    }
}
