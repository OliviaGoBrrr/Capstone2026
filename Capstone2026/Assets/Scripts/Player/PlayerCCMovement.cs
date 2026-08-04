using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCCMovement : MonoBehaviour
{
    [Header("Player Model")]
    public GameObject playerModel;
    // Movement Values
    [Header("Player Movement Values")]
    public float moveSpeed = 10f;
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float jumpHeight = 0.5f;
   

    // Rotation (1 = snap to rotation direction)
    [Range(0f, 180f)] public float rotationSpeed;

    // Physics
    public Vector3 playerInput;
    public Vector3 playerVelocity;
    [HideInInspector]
    public Vector3 desiredMove;

    [HideInInspector]
    public bool groundedPlayer;
    public bool prevFrameGrounded;
    public float acceleration = 10f;
    public float deccceleration = 45f;
    public float gravityValue = -9.81f;
    public bool gravityOn = true;

    [HideInInspector] public bool grappling;
    [Header("Grapple Action Values")]
    public float grappleAngle;
    public float grappleMaxDistance;
    public float grappleSpeed;
    public float grappleLockoutTime;
    [HideInInspector]
    public float grappleLockoutTimer = 0.0f;
    private Collider[] grappleColliders;
    private const int maxGrappleColliders = 10;
    public LayerMask grappleTargetLayer;
    public Vector3 grapplePoint;
    [SerializeField]
    private LineRenderer grappleLine;
    [SerializeField]
    private Transform grappleHand;

    [Header("Grapple UI")]
    private GameObject currentGrappleUI;

    [Header("Player Camera Values")]
    public CharacterController playerController;
    public Camera playerCamera;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference grappleAction;
    public InputActionReference runAction;

    [Header("Input Buffer Times")]
    public float coyoteTime = 0.1f;
    [HideInInspector] public float coyoteTimer;

    public float grappleBuffer = 0.1f;
    [HideInInspector] public float grappleBufferTimer;

    public float jumpBuffer = 0.2f;
    [HideInInspector] public float jumpBufferTimer;

    private void Awake()
    {
        if(playerController == null)
        {
            playerController = GetComponent<CharacterController>();
        }

        if(grappleLine == null)
        {
            grappleLine = GetComponent<LineRenderer>();
            grappleLine.enabled = false;
        }

        if(playerModel == null)
        {
            Debug.LogError("There is no player model added in the PlayerCCMovement Inspector");
        }

        grappleColliders = new Collider[maxGrappleColliders];
    }

    private void LateUpdate()
    {
        InputBuffers();
    }

    private void InputBuffers()
    {
        if (grappleBufferTimer > 0) { grappleBufferTimer -= Time.deltaTime; }

        if (jumpBufferTimer > 0) { jumpBufferTimer -= Time.deltaTime; }

        if (coyoteTimer > 0) { coyoteTimer -= Time.deltaTime; }

        if (!groundedPlayer && prevFrameGrounded)
        {
            coyoteTimer = coyoteTime;
        }

        prevFrameGrounded = groundedPlayer;

        if (grappleAction.action.WasPressedThisFrame())
        {
            grappleBufferTimer = grappleBuffer;
        }

        if (jumpAction.action.WasPressedThisFrame())
        {
            jumpBufferTimer = jumpBuffer;
        }
        
    }

    public void PlayerMovementLogic()
    {
        groundedPlayer = playerController.isGrounded;

        MovePlayer();

        if (gravityOn)
        {
            HandleGravity();
        }

        // Timers

        if (grappleLockoutTimer > -1.0f)
        {
            grappleLockoutTimer -= Time.deltaTime;
        }
    }

    void HandleGravity()
    {
        if (playerController.isGrounded)
        {
            playerInput.y = -1f;
        }
        else
        {
            float previousYVelocity = playerInput.y;
            float newYVelocity = playerInput.y + (gravityValue * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            playerInput.y = nextYVelocity;
        }
    }


    public void MovePlayer()
    {
        Vector2 actionInput = moveAction.action.ReadValue<Vector2>();
        if(grappling == false)
        {
            playerInput.x = actionInput.x;
            playerInput.z = actionInput.y;
        }

        Vector3 cameraRelativeMovement = ConvertToCameraSpace(playerInput);

        playerController.Move(moveSpeed * Time.deltaTime * cameraRelativeMovement);
    }

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {

        float currentYValue = vectorToRotate.y;

        Vector3 camF = Camera.main.transform.forward;
        Vector3 camR = Camera.main.transform.right;

        camF = camF.normalized;
        camR = camR.normalized;

        Vector3 camForwardZProduct = vectorToRotate.z * camF;
        Vector3 camRightXProduct = vectorToRotate.x * camR;

        Vector3 vectorRoatatedToCameraSpace = camForwardZProduct + camRightXProduct;

        if (vectorRoatatedToCameraSpace != Vector3.zero)
        {
            RotatePlayer(vectorRoatatedToCameraSpace.normalized);
        }

        vectorRoatatedToCameraSpace.y = currentYValue;

        return vectorRoatatedToCameraSpace;
    }

    public void PlayerJump()
    {
        if (groundedPlayer || coyoteTimer > 0f)
        {
            if (jumpBufferTimer > 0f || jumpAction.action.WasPressedThisFrame())
            {
                playerInput.y = jumpHeight;
            }
            else if (playerVelocity.y < 0f) // caps the falling speed of the player when on the ground
            {
                playerInput.y = -3f;
            }
        }
    }

    public void IsPlayerRunning()
    {
        if (runAction.action.IsPressed())
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    public void RotatePlayer(Vector3 targetRotation)
    {
        // Rotation calculation - will look in the direction the input action
        Vector3 adjustedTarget = new Vector3(targetRotation.x, 0, targetRotation.z);

        Quaternion target = Quaternion.LookRotation(adjustedTarget);

        // Rotates the model over time
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, target, rotationSpeed * Time.deltaTime);
    }

    public bool FindValidGrappleTarget() // returns true if valid target selected
    {
        // Doesn't find another target if the player is locked out from grappling
        if(grappleLockoutTimer < 0f)
        {
            // Checks if theres any grapple points within the player's view, and puts them in an array
            // *Seperate Note* - This may be an expensive calculation if the grapple point collider meshes are too complex
            int numColliders = Physics.OverlapSphereNonAlloc(playerCamera.transform.position, grappleMaxDistance, grappleColliders, grappleTargetLayer);

            // Temporarily saves the direction the closest grapple point
            Vector3 closestGrapple = Vector3.zero;
            float closestDot = 0f;

            if (numColliders > 0)
            {
                // Checks all colliders (within the grapple target layer) if they're within the grapple angle...
                for (int i = 0; i < numColliders; i++)
                {
                    Vector3 direction = (grappleColliders[i].transform.position - playerCamera.transform.position).normalized;

                    float dirDot = Vector3.Dot(playerCamera.transform.forward, direction);

                    // ... and which one is closest to what the player is looking at.
                    if (dirDot >= Mathf.Cos(Mathf.Deg2Rad * grappleAngle)) // if its within the search angle
                    {
                        if (dirDot > closestDot) // saves the closest grapple target
                        {
                            closestDot = dirDot;
                            closestGrapple = direction;
                        }
                    }
                }
            }
            else // Disables any grapple UI if the player runs out of range
            {
                DisableGrappleUI();
            }


            // If there was a valid grapple target found, throw a ray in its a direction to graaple to
            if (closestGrapple != Vector3.zero)
            {
                RaycastHit hit;

                // Sends a ray towards the closest grapple point
                if (Physics.Raycast(playerCamera.transform.position, closestGrapple, out hit, grappleMaxDistance, grappleTargetLayer))
                {
                    // It should find a target, but it allows the disabling of the grapple point
                    GrappleableObject target = hit.transform.GetComponent<GrappleableObject>();

                    Debug.DrawLine(target.anchorPoint.position, transform.position, Color.magenta);

                    if (target != null) // If the grapple point isn't disabled
                    {
                        // UI Appears
                        if (target.grappleUICanvas != null)
                        {
                            if (currentGrappleUI != target.grappleUICanvas.gameObject)
                            {
                                DisableGrappleUI();
                                target.grappleUICanvas.gameObject.SetActive(true);
                                currentGrappleUI = target.grappleUICanvas.gameObject;
                            }
                        }

                        // Action is taken
                        if (grappleBufferTimer > 0f && grappleLockoutTimer <= 0) // If there was an input buffered
                        {
                            StartGrapple(target);
                            Array.Clear(grappleColliders, 0, grappleColliders.Length);
                            return true;
                        }
                    }
                    else
                    {
                        DisableGrappleUI(); // Disables UI if player looks at another target
                    }
                }
            }
            else
            {
                DisableGrappleUI(); // Disables UI if player looks away from any target
            }
        }
        else
        {
            DisableGrappleUI(); // Disables UI if player grapples, and is in grapple lockout
        }

        return false;
    }


    private void StartGrapple(GrappleableObject grappleTarget)
    {
        // Turn off physics
        gravityOn = false;
        grappling = true;

        // Reset player velocity
        playerInput = Vector3.zero;

        // Set grapple location
        grapplePoint = grappleTarget.anchorPoint.transform.position;
        grappleLockoutTimer = grappleLockoutTime;

        // Linerenderer
        grappleLine.SetPosition(1, grapplePoint);
        grappleLine.enabled = true;
    }
    public void GrappleToTarget()
    {
        if (grappling)
        {
            // Find the distance between player and grapple point
            Vector3 direction = grapplePoint - transform.position;

            // Normalize to translate to velocity
            direction.Normalize();
            RotatePlayer(direction);

            playerInput = direction * grappleSpeed;

            // LineRenderer
            grappleLine.SetPosition(0, grappleHand.position);

            if (Vector3.Distance(transform.position, grapplePoint) < 1.0f)
            {
                transform.position = grapplePoint;
                playerInput.y = 0f;

                CancelGrapple();
            }
        }
    }
    public void CancelGrapple()
    {
        // Reset and clear everything
        gravityOn = true;
        grappling = false;

        grapplePoint = Vector3.zero;

        // Linerenderer
        grappleLine.enabled = false;
    }

    private void DisableGrappleUI()
    {
        if(currentGrappleUI != null)
        {
            currentGrappleUI.SetActive(false);
            currentGrappleUI = null;
        }
    } 

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        grappleAction.action.Enable();
        runAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        grappleAction.action.Disable();
        runAction.action.Disable();
    }


    /* LEGACY MOVEMENT 
public void PlayerMove()
{
    // Get the x,z direction the player is inputting
    Vector2 input = moveAction.action.ReadValue<Vector2>();

    Vector2 inputNorm = input.normalized;

    // Rotate the player
    Vector3 camF = playerCamera.transform.forward;
    Vector3 camR = playerCamera.transform.right;

    camF.y = 0f;
    camR.y = 0f;

    desiredMove = (camF * input.y + camR * input.x).normalized;

    // Jump & Fall States

    // Rotates the player if they're inputting an action
    if(desiredMove != Vector3.zero)
    {
        RotatePlayer(desiredMove);
    }

    Vector3 targetVelcoity = desiredMove * moveSpeed;

    playerVelocity = Vector3.MoveTowards(playerVelocity, new Vector3(targetVelcoity.x, playerVelocity.y, targetVelcoity.z), acceleration * Time.deltaTime);
}
*/

}
