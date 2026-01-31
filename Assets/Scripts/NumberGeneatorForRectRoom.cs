

using UnityEngine;

public class NumberGeneatorForRectRoom : MonoBehaviour, IInteractable
{
    [Header("UI")]
    public GameObject RectangleRoomFrameToLockDia;
    public GameObject RectangleRoomDia;
    public Transform InteractorSource;

    private bool isAttached = false;
    private Rigidbody rb;
    private Transform currentParent;
    private bool isPlacedInWallCube = false;
    private static int detachCallCount = 0;
    private static bool actionRTriggered = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
    }

    public void InteractRange()
    {
        if (detachCallCount >= 11)
        {
            Debug.Log("Interaction is locked for RectangleRoom as detachCallCount has reached the limit.");
            return;
        }

        if (InteractorSource != null && !isAttached && !isPlacedInWallCube)
        {
            AttachTo(InteractorSource);
        }
        else if (isPlacedInWallCube)
        {
            Debug.Log("Cube is placed in a wall cube and cannot be re-attached.");
        }

        RectangleRoomFrameToLockDia.SetActive(false);
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

        if (detachCallCount == 11 && !actionRTriggered)
        {
            actionRTriggered = true;
            RectangleRoomDia.SetActive(false);
            RectangleRoomFrameToLockDia.SetActive(true);
        }
    }

    public bool IsPlacedInWallCube()
    {
        return isPlacedInWallCube;
    }

    public static int GetDetachCallCount()
    {
        return detachCallCount;
    }

    public static bool IsActionRTriggered()
    {
        return actionRTriggered; // Return the status of the flag
    }

}
