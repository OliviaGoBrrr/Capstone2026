using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCCMovement : MonoBehaviour
{
    // Movement Values
    [Header("Player Movement Values")]
    public float moveSpeed = 10f;
    public float jumpHeight = 0.5f;
    public float jumpHorizontalDampening = 0.7f;

    // Physics
    //[HideInInspector]
    public Vector3 playerVelocity;
    [HideInInspector]
    public bool groundedPlayer;
    public float acceleration = 10f;
    //public float decceleration = 10f;
    public float gravityValue = -9.81f;
    public bool gravityOn = true;

    // Rotation
    [Range(0f, 180f)] public float rotationSpeed = 180f;
    private Vector3 playerRotation;
    float turnSpeedVelocity;
    float turnSmoothTime;

    [Header("Grapple Action Values")]
    public bool grappling;
    public float grappleRange;
    public float grappleSpeed;
    public float grappleLockoutTime;
    private float grappleLockoutTimer = 0.0f;
    public LayerMask grappleTargetLayer;
    public Vector3 grapplePoint;
    [SerializeField]
    private LineRenderer grappleLine;
    [SerializeField]
    private Transform grappleHand;

    [Header("Player Camera Values")]
    public CharacterController playerController;
    public Camera playerCamera;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference grappleAction;
    public InputActionReference runAction;

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
    }

    private void Start()
    {
        // Cursor is invisible and is confined to screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        groundedPlayer = playerController.isGrounded;

        if (gravityOn)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }

        if(grappling == false)
        {
            PlayerJump();
            PlayerMove();
            FindValidGrappleTarget();
        }
        else if (grappling)
        {
            GrappleToTarget();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CancelGrapple();
                playerVelocity.y = jumpHeight;
            }
        }

        playerController.Move(playerVelocity * Time.deltaTime);

        // Timers

        if (grappleLockoutTimer > -1.0f)
        {
            grappleLockoutTimer -= Time.deltaTime;
        }
    }

    private void PlayerMove()
    {
        // Rotate the player with the direction they're walking towards
        //this.playerRotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime, 0);

        // Get the x,z direction the player is inputting
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 camF = playerCamera.transform.forward;
        Vector3 camR = playerCamera.transform.right;

        camF.y = 0f;
        camR.y = 0f;

        Vector3 desiredMove = (camF * input.y + camR * input.x).normalized;

        if (!groundedPlayer)
        {
            desiredMove *= jumpHorizontalDampening;
        }

        Vector3 targetVelcoity = desiredMove * moveSpeed;

        playerVelocity = Vector3.MoveTowards(playerVelocity, new Vector3(targetVelcoity.x, playerVelocity.y, targetVelcoity.z), acceleration * Time.deltaTime);
    }

    private void PlayerJump()
    {
        if (groundedPlayer)
        {
            if (jumpAction.action.WasPressedThisFrame())
            {
                playerVelocity.y = jumpHeight;
            }
            else if (playerVelocity.y < 0f)
            {
                playerVelocity.y = -3f;
            }
        }
    }

    private void FindValidGrappleTarget()
    {
        RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grappleRange, grappleTargetLayer))
        {
            GrappleableObject target = hit.transform.GetComponent<GrappleableObject>();

            Debug.DrawLine(target.anchorPoint.position, transform.position, Color.magenta);

            if (target != null)
            {
                if (Input.GetKeyDown(KeyCode.E) && grappleLockoutTimer <= 0)
                {
                    StartGrapple(target);
                }
            }
        }
    }

    private void StartGrapple(GrappleableObject grappleTarget)
    {
        // Turn off physics
        gravityOn = false;
        grappling = true;

        // Reset player velocity
        playerVelocity = Vector3.zero;

        // Set grapple location
        grapplePoint = grappleTarget.anchorPoint.transform.position;
        grappleLockoutTimer = grappleLockoutTime;

        // Linerenderer
        grappleLine.SetPosition(1, grapplePoint);
        grappleLine.enabled = true;
    }

    private void GrappleToTarget()
    {
        // Find the distance between player and grapple point
        Vector3 direction = grapplePoint - transform.position;

        // Normalize to translate to velocity
        direction.Normalize();
        playerVelocity = direction * grappleSpeed;

        // LineRenderer
        grappleLine.SetPosition(0, grappleHand.position);

        if (Vector3.Distance(transform.position, grapplePoint) < 1.0f)
        {
            transform.position = grapplePoint;
            playerVelocity = Vector3.zero;
            CancelGrapple();
        }
    }

    private void CancelGrapple()
    {
        // Reset and clear everything
        gravityOn = true;
        grappling = false;

        grapplePoint = Vector3.zero;

        // Linerenderer
        grappleLine.enabled = false;

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
