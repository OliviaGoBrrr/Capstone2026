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
    [HideInInspector]
    public Vector3 playerVelocity;
    [HideInInspector]
    public bool groundedPlayer;
    public float acceleration = 10f;
    //public float decceleration = 10f;
    public float gravityValue = -9.81f;

    // Rotation
    [Range(0f, 180f)] public float rotationSpeed = 180f;
    private Vector3 playerRotation;
    float turnSpeedVelocity;
    float turnSmoothTime;

    [Header("Grapple Action Values")]
    public float grappleRange;
    public LayerMask grappleTargetLayer;
    bool grappling = false;

    [Header("Player Camera Values")]
    public CharacterController playerController;
    public Camera playerCamera;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference grappleAction;
    public InputActionReference runAction;

    private void Start()
    {
        // Cursor is invisible and is confined to screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        grappling = false;
    }

    private void Update()
    {
        groundedPlayer = playerController.isGrounded;

        if (!grappling)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CancelGrapple();
        }

        PlayerJump();
        PlayerMove();
        playerController.Move(playerVelocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        FindValidGrappleTarget();
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
            else if (playerVelocity.y < 0f && !grappling)
            {
                playerVelocity.y = -3f;
            }
        }
        CancelGrapple();
    }

    private void FindValidGrappleTarget()
    {
        RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grappleRange, grappleTargetLayer))
        {
            GrappleableObject target = hit.transform.GetComponent<GrappleableObject>();

            Debug.DrawLine(target.anchorPoint.position, transform.position, Color.magenta);

            if (target != null && Input.GetKeyDown(KeyCode.E))
            {
                GrappleToTarget(target);
            }
        }
    }

    private void GrappleToTarget(GrappleableObject grappleTarget)
    {
        Debug.Log("Player grappled to: " + grappleTarget.name + " at: " + grappleTarget.transform.position);
        // Move player to target

        // Player can "cancel" grapple by jumping

        // When the player has reached target, unassign grapple target
    }

    private void CancelGrapple()
    {
        grappling = false;
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
