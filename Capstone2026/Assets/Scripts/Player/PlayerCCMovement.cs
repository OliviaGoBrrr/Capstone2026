using System;
using DG.Tweening;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCCMovement : MonoBehaviour
{
    [Header("Player Model")]
    public GameObject playerModel;
    public Animator animator;
    // Movement Values
    [Header("Player Movement Values")]
    public float moveSpeed = 10f;
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float jumpHeight = 0.5f;

    public float pushPullPenalty = 0.5f;

    // Rotation (1 = snap to rotation direction)
    public float currentPlayerRotation;
    [Range(0f, 180f)] public float rotationSpeed;

    // Physics
    public Vector3 playerInput;
    public Vector3 playerVelocity;
    private float timeFalling = 0f;

    [HideInInspector]
    public bool groundedPlayer;
    public bool prevFrameGrounded;
    [HideInInspector]
    public bool running { private set; get; }

    public bool gravityOn = true;
    public float gravityValue = -9.81f;


    [HideInInspector] public bool grappling;
    [HideInInspector] bool grappleArmAtPoint;
    [Header("Grapple Action Values")]
    public float grappleAngle;
    public float grappleMaxDistance;
    public float grappleSpeed;
    public float grappleLockoutTime;
    [HideInInspector]
    public float grappleLockoutTimer = 0.0f;
    private Collider[] grappleColliders;
    private const int maxGrappleColliders = 10;
    public LayerMask obstacleLayerMask;
    public LayerMask grappleTargetLayer;
    public Vector3 grapplePoint;
    [SerializeField]
    private GameObject grappleArmPrefab;
    private GameObject grappleArmCopy;
    [SerializeField]
    private GameObject grappleArmOnModel;
    [SerializeField]
    private float grappleArmSpeed = 60f;
    [SerializeField]
    private LineRenderer grappleLine;
    [SerializeField]
    private Transform grappleShootPoint;

    [Header("Grapple UI")]
    private GameObject currentGrappleUI;

    [Header("Player Component Values")]
    public CharacterController playerController;
    public CinemachineCamera playerCamera;
    public PlayerManager playerManager;

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

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void LateUpdate()
    {
        InputBuffers();

        if(Input.GetKeyDown(KeyCode.P)) {animator.SetTrigger("Wave");}
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
        // Stops player from inputting movement/rotation
        if (!playerController.enabled) { return; }
        groundedPlayer = playerController.isGrounded;

        if (grappling)
        {
            HandleGrapple();
        }
        else
        {
            MovePlayer();
        }

        if (gravityOn)
        {
            HandleGravity();
        }


        HandlePlayerAnimations();

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
            timeFalling = 0f;
        }
        else
        {
            float previousYVelocity = playerInput.y;
            float newYVelocity = playerInput.y + (gravityValue * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            playerInput.y = nextYVelocity;

            timeFalling += Time.deltaTime;
        }
    }

    public void MovePlayer()
    {
        Vector2 actionInput = moveAction.action.ReadValue<Vector2>();
        Vector3 cameraRelativeMovement;
        if (grappling == false)
        {
            playerInput.x = actionInput.x; // left and right
            playerInput.z = actionInput.y; // forward and backward

            cameraRelativeMovement = ConvertToCameraSpace(playerInput);
        }
        else
        {
            // Disables camera movement while grappling
            // Otherwise, causes the player to fly if they face away from the grapple target
            cameraRelativeMovement = playerInput;
        }

        if (!playerController.enabled) { return; }

        // If the player is not inputting movement, idle animation, else running/walking
        float targetAnimSpeed;
        float animSpeed = animator.GetFloat("Speed");

        if (actionInput == Vector2.zero) { targetAnimSpeed = 0f; }
        else { targetAnimSpeed = moveSpeed; }

        // Lerp between animations
        float newAnimSpeed = animSpeed + ((targetAnimSpeed - animSpeed) * Time.deltaTime * 2f);
        animator.SetFloat("Speed", newAnimSpeed);

        // Move player
        float checkSpeed = moveSpeed;
        if (playerManager.isPushPulling)
        {
            checkSpeed = moveSpeed * pushPullPenalty;
        }

        playerController.Move(checkSpeed * Time.deltaTime * cameraRelativeMovement);

    }

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {

        float currentYValue = vectorToRotate.y;

        Vector3 camF = playerCamera.transform.forward;

        Vector3 camR = playerCamera.transform.right;

        // Sets camera's upo transform to Y, making it parallel to the ground
        camF.y = 0f;
        camR.y = 0f;

        // Normalize camera transforms to derive direction along X and Z axis
        camF.Normalize();
        camR.Normalize();

        Vector3 camForwardZProduct = vectorToRotate.z * camF; // forward and backward
        Vector3 camRightXProduct = vectorToRotate.x * camR; // left and right

        // Add vectors together to translate player movement
        Vector3 vectorRoatatedToCameraSpace = camForwardZProduct + camRightXProduct;

        if (vectorRoatatedToCameraSpace != Vector3.zero)
        {
            RotatePlayer(vectorRoatatedToCameraSpace, rotationSpeed);
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
                animator.SetTrigger("Jump");
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
            if (!running) { 
                playerManager.ChangeFOV.Invoke(playerManager.settings.FOVslider.value + 10f);
                running = true;
                moveSpeed = runSpeed;
            }
        }
        else
        {
            if (running) {
                playerManager.ChangeFOV.Invoke(playerManager.settings.FOVslider.value);
                moveSpeed = walkSpeed;
                running = false;
            }
        }
    }

    public void RotatePlayer(Vector3 targetRotation, float rotSpeed)
    {
        // Rotation calculation - will look in the direction the input action
        Vector3 adjustedTarget = new Vector3(targetRotation.x, 0, targetRotation.z);

        Quaternion target = Quaternion.LookRotation(adjustedTarget);

        // Rotates the model over time
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, target, rotSpeed * Time.deltaTime);
        currentPlayerRotation = playerModel.transform.rotation.y;
    }

    private void HandlePlayerAnimations()
    {
        Vector3 playerVelocity = playerInput;

        animator.SetBool("isGrounded", groundedPlayer);

        // Falling Animation - start if they've been falling for a while
        if (!groundedPlayer && timeFalling > 0.2f)
        {
            animator.SetFloat("YVelocity", playerVelocity.y);
        }
        else
        {
            if(animator.GetFloat("YVelocity") < 1f) // Makes it so the SetFloat is only triggered when not falling anymore
            {
                animator.SetFloat("YVelocity", 1f); // If the player is on the ground, the falling animation won't trigger
            }
        }

        // Idle, Walking, Running

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

                // Stop people from grappling through walls (maybe costly?)

                if (Physics.Raycast(playerCamera.transform.position, closestGrapple, out hit, grappleMaxDistance, obstacleLayerMask))
                {
                    Debug.DrawLine(playerCamera.transform.position, hit.transform.position, Color.cyan);
                    closestGrapple = Vector3.zero;
                    DisableGrappleUI();
                    return false;
                }


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


    // Grapple Projectile pseudocode
    /*
     * Launch projectile
     * 
     * Move projectile each frame until destination is met 
     * or time waiting for grapple is exceeded
     * 
     * Start grapple to target
     * 
     */

    private void StartGrapple(GrappleableObject grappleTarget)
    {
        // Set grapple location
        grapplePoint = grappleTarget.anchorPoint.transform.position;
        grappleLockoutTimer = grappleLockoutTime;

        // Turn off physics
        //gravityOn = false;
        grappling = true;

        grappleArmCopy = Instantiate(grappleArmPrefab, grappleShootPoint.position, Quaternion.identity);
        grappleArmAtPoint = false;

        grappleArmOnModel.SetActive(false);

        // Reset player velocity
        playerInput = Vector3.zero;

        // Linerenderer
        grappleLine.SetPosition(0, grappleShootPoint.position);
        grappleLine.SetPosition(1, grappleArmCopy.transform.position);
        grappleLine.enabled = true;
    }

    public void HandleGrapple()
    { 
        RotatePlayer(grapplePoint - transform.position, rotationSpeed * 1.5f);

        Vector3 armPos = grappleArmCopy.transform.position;

        armPos = Vector3.MoveTowards(armPos, grapplePoint, Time.deltaTime * grappleArmSpeed);

        if(Vector3.Distance(armPos, grapplePoint) < 0.1f)
        {
            grappleArmAtPoint = true;
        }

        Vector3 direction =  grapplePoint - armPos;
        Vector3 newRot = Vector3.RotateTowards(grappleArmCopy.transform.forward, direction, 1f, 0.0f);

        grappleArmCopy.transform.position = armPos;
        grappleArmCopy.transform.rotation = Quaternion.LookRotation(newRot);

        grappleLine.SetPosition(0, grappleShootPoint.position);
        grappleLine.SetPosition(1, grappleArmCopy.transform.position);
    }

    public void GrappleToTarget()
    {
        if (grappling && grappleArmAtPoint)
        {
            // Find the distance between player and grapple point
            Vector3 grappleOffset = new Vector3(grapplePoint.x, grapplePoint.y - 1f, grapplePoint.z);

            Vector3 direction = grappleOffset - transform.position;

            // Normalize to translate to velocity
            direction.Normalize();
            RotatePlayer(direction, rotationSpeed);

            playerInput = direction * grappleSpeed;

            // LineRenderer
            grappleLine.SetPosition(0, grappleShootPoint.position);

            if (Vector3.Distance(transform.position, grappleOffset) < 1f)
            {
                transform.position = grappleOffset;
                playerInput.y = -0.1f;

                CancelGrapple();
            }
        }
    }
    public void CancelGrapple()
    {
        // Reset and clear everything
        gravityOn = true;
        grappling = false;
        playerInput = Vector3.zero;
        grappleArmOnModel.SetActive(true);

        if (grappleArmCopy != null)
        {
            Destroy(grappleArmCopy);
        }

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
}
