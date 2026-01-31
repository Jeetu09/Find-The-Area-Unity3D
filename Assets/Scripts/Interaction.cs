
using UnityEngine;

public interface IInteractable
{
    void InteractRange(); // The method for general interaction
}

public class Interaction : MonoBehaviour
{
    public Transform InteractorSource;
    public Transform Grabber;
    public GameObject TargateE;        // UI element for generic interaction
    public GameObject TargateQ;        // UI element for wall cube interaction
    public float InteractRange = 2f;

    private NumberGenerator currentAttachedCube;
    private NumberGeneatorForRectRoom currentAttachedRect;
    private bool isAttached = false;  // Flag to track if something is already attached

    private void Start()
    {
        if (TargateE != null)
        {
            TargateE.SetActive(false);
        }
        else
        {
            Debug.LogError("TargateE UI element is not assigned.");
        }

        if (TargateQ != null)
        {
            TargateQ.SetActive(false);
        }
        else
        {
            Debug.LogError("TargateQ UI element is not assigned.");
        }

        if (InteractorSource == null || Grabber == null)
        {
            Debug.LogError("InteractorSource or Grabber is not assigned.");
        }

        StartCoroutine(HandleInput());
    }

    private System.Collections.IEnumerator HandleInput()
    {
        while (true)
        {
            if (InteractorSource == null) yield break; // Exit if InteractorSource is missing

            Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);
            bool isHitInteractable = Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange) &&
                                     hitInfo.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactObj);

            // Check if the object has the "Attachable" tag
            bool isAttachableObject = hitInfo.collider != null && hitInfo.collider.CompareTag("Attachable");

            // Toggle TargateE based on whether the object is interactable and attachable
            if (isHitInteractable)
            {
                if (isAttachableObject)
                {
                    if (TargateQ != null) TargateQ.SetActive(false);  // Disable TargateQ
                    if (TargateE != null) TargateE.SetActive(true);   // Enable TargateE
                }
                else
                {
                    if (TargateE != null) TargateE.SetActive(true);
                    if (TargateQ != null) TargateQ.SetActive(false);
                }
            }
            else
            {
                if (TargateE != null) TargateE.SetActive(false);
                if (TargateQ != null) TargateQ.SetActive(false);
            }

            // Interaction logic for "Attachable" objects
            if (isHitInteractable && Input.GetKeyDown(KeyCode.E) && !isAttached && isAttachableObject)
            {
                HandleAttachableObjectInteraction(hitInfo);
            }
            else if (isHitInteractable && Input.GetKeyDown(KeyCode.E) && !isAttachableObject)
            {
                HandleNonAttachableObjectInteraction(hitInfo);
            }

            // Enable TargateQ only when focusing on WallCube and an object is attached
            if (isAttached)
            {
                EnableTargateQOnWallCubeFocus(ray);
            }

            if (Input.GetKeyDown(KeyCode.Q) && isAttached)
            {
                HandleCubeDetachment(ray);
                HandleRectDetachment(ray);
            }

            yield return null;
        }
    }

    private void EnableTargateQOnWallCubeFocus(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.CompareTag("WallCube"))
            {
                // Enable TargateQ if focusing on WallCube
                if (TargateQ != null) TargateQ.SetActive(true);
            }
            else
            {
                // Disable TargateQ if not focusing on WallCube
                if (TargateQ != null) TargateQ.SetActive(false);
            }
        }
        else
        {
            // Disable TargateQ if no WallCube is in focus
            if (TargateQ != null) TargateQ.SetActive(false);
        }
    }


    private void HandleAttachableObjectInteraction(RaycastHit hitInfo)
    {
        // Handling interaction with attachable objects
        NumberGenerator numberGenerator = hitInfo.collider.gameObject.GetComponent<NumberGenerator>();
        NumberGeneatorForRectRoom numberGeneratorRect = hitInfo.collider.gameObject.GetComponent<NumberGeneatorForRectRoom>();

        // Handle attachable cubes
        if (numberGenerator != null)
        {
            HandleCubeAttachment(numberGenerator);
        }
        else if (numberGeneratorRect != null)
        {
            HandleCubeAttachment(numberGeneratorRect);
        }
        else
        {
            // If it's an attachable object but not a cube, handle it here
            HandleSpecialAttachableInteraction(hitInfo);
        }
    }

    private void HandleNonAttachableObjectInteraction(RaycastHit hitInfo)
    {
        DoorLock doorLock = hitInfo.collider.gameObject.GetComponent<DoorLock>();
        MainGateLockscrpt mainGateLock = hitInfo.collider.gameObject.GetComponent<MainGateLockscrpt>();
        SquareRoomLock squareRoom = hitInfo.collider.gameObject.GetComponent<SquareRoomLock>();
        RoboSquare HelperRoboSquare = hitInfo.collider.gameObject.GetComponent<RoboSquare>();
        Rectanglescrpt RectangleRoom = hitInfo.collider.gameObject.GetComponent<Rectanglescrpt>();
        RectangleRoboscrpt HelperRoborectangle = hitInfo.collider.gameObject.GetComponent<RectangleRoboscrpt>();
        TriangleScript TriangleRoomLockObj = hitInfo.collider.gameObject.GetComponent<TriangleScript>();
        TriangleRobo HelperRoboTriangle = hitInfo.collider.gameObject.GetComponent<TriangleRobo>();
        CircleRobo HelperRoboCircle = hitInfo.collider.gameObject.GetComponent<CircleRobo>();
        WheelRoatation WheelAnimationButton = hitInfo.collider.gameObject.GetComponent<WheelRoatation>();
        CircleLevelScript CircleRoomLockObj = hitInfo.collider.gameObject.GetComponent<CircleLevelScript>();


        // Prevent interaction with SquareRoom if actionTriggered is false
        if (squareRoom != null)
        {
            if (!NumberGenerator.IsActionTriggered())
            {
                Debug.Log("SquareRoom interaction is restricted until detachCallCount reaches 8.");
                return;
            }
        }

        if (RectangleRoom != null)
        {
            if (!NumberGeneatorForRectRoom.IsActionRTriggered())
            {
                Debug.Log("RectangleRoom interaction is restricted until detachCallCount reaches 12.");
                return;
            }
        }

        if (doorLock != null) doorLock.InteractRange();
        else if (mainGateLock != null) mainGateLock.InteractRange();
        else if (WheelAnimationButton != null) WheelAnimationButton.InteractRange();
        else if (squareRoom != null) squareRoom.InteractRange();
        else if (HelperRoboSquare != null) HelperRoboSquare.InteractRange();
        else if (HelperRoboTriangle != null) HelperRoboTriangle.InteractRange();
        else if (HelperRoboCircle != null) HelperRoboCircle.InteractRange();
        else if (HelperRoborectangle != null) HelperRoborectangle.InteractRange();
        else if (RectangleRoom != null) RectangleRoom.InteractRange();
        else if (TriangleRoomLockObj != null) TriangleRoomLockObj.InteractRange();
        else if (CircleRoomLockObj != null) CircleRoomLockObj.InteractRange();
    }


    private void HandleCubeAttachment(NumberGenerator numberGenerator)
    {
        if (numberGenerator != null && !numberGenerator.IsPlacedInWallCube())
        {
            if (currentAttachedCube != null)
            {
                currentAttachedCube.Detach(null);
                ResetCubePhysics(currentAttachedCube);
            }

            currentAttachedCube = numberGenerator;
            numberGenerator.AttachTo(Grabber);
            isAttached = true;  // Set attached flag to true
        }
        else
        {
            Debug.Log("Cube is already placed in a wall cube and cannot be re-attached.");
        }
    }

    private void HandleCubeAttachment(NumberGeneatorForRectRoom numberGeneratorRect)
    {
        if (numberGeneratorRect != null && !numberGeneratorRect.IsPlacedInWallCube())
        {
            if (currentAttachedRect != null)
            {
                currentAttachedRect.Detach(null);
                ResetRectPhysics(currentAttachedRect);
            }

            currentAttachedRect = numberGeneratorRect;
            numberGeneratorRect.AttachTo(Grabber);
            isAttached = true;  // Set attached flag to true
        }
        else
        {
            Debug.Log("Cube is already placed in a wall cube and cannot be re-attached.");
        }
    }

    private void HandleSpecialAttachableInteraction(RaycastHit hitInfo)
    {
        // Handle any special interaction for attachable objects that don't fall under cubes
        Debug.Log("Interacting with a special attachable object.");
    }
    private void HandleCubeDetachment(Ray ray)
    {
        if (currentAttachedCube != null)
        {
            // Check if the player is focusing on a WallCube
            if (Physics.Raycast(ray, out RaycastHit hitInfoQ, InteractRange))
            {
                if (hitInfoQ.collider.CompareTag("WallCube"))
                {
                    // Enable TargateQ when the player is focusing on the WallCube
                    if (TargateQ != null) TargateQ.SetActive(true);

                    // Detach the cube only when Q is pressed
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        currentAttachedCube.Detach(hitInfoQ.collider.transform);
                        currentAttachedCube = null;
                        isAttached = false; // Reset attached flag

                        // Ensure TargateQ is disabled after detachment
                        if (TargateQ != null) TargateQ.SetActive(false);
                    }
                }
                else
                {
                    // Disable TargateQ if not focusing on WallCube
                    if (TargateQ != null) TargateQ.SetActive(false);
                }
            }
        }
    }

    private void HandleRectDetachment(Ray ray)
    {
        if (currentAttachedRect != null)
        {
            // Check if the player is focusing on a WallCube
            if (Physics.Raycast(ray, out RaycastHit hitInfoQ, InteractRange))
            {
                if (hitInfoQ.collider.CompareTag("WallCube"))
                {
                    // Enable TargateQ when the player is focusing on the WallCube
                    if (TargateQ != null) TargateQ.SetActive(true);

                    // Detach the rectangle only when Q is pressed
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        currentAttachedRect.Detach(hitInfoQ.collider.transform);
                        currentAttachedRect = null;
                        isAttached = false; // Reset attached flag

                        // Ensure TargateQ is disabled after detachment
                        if (TargateQ != null) TargateQ.SetActive(false);
                    }
                }
                else
                {
                    // Disable TargateQ if not focusing on WallCube
                    if (TargateQ != null) TargateQ.SetActive(false);
                }
            }
        }
    }


    private void ResetCubePhysics(NumberGenerator cube)
    {
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    private void ResetRectPhysics(NumberGeneatorForRectRoom rect)
    {
        Rigidbody rb = rect.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
