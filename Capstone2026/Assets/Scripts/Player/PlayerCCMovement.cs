using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCCMovement : MonoBehaviour
{
    // Movement Values
    [Header("Player Movement Values")]
    public float moveSpeed = 10f;
    [Range(0f, 180f)] public float rotationSpeed = 180f;
    public float jumpHeight = 0.5f;
    public float jumpHorizontalDampening = 0.7f;
    public float acceleration = 10f;
    public float decceleration = 10f;
    public float gravityValue = -9.81f;
    public bool playerJumpLockout = false;


    float turnSpeedVelocity;
    float turnSmoothTime;

    public float yAxisVelocity;
    private float currentSpeed;
    public Vector3 playerVelocity;
    private Vector3 playerRotation;
    public bool groundedPlayer;

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
    }

    private void Update()
    {
        groundedPlayer = playerController.isGrounded;

        playerVelocity.y += gravityValue * Time.deltaTime;

        PlayerJump();

        PlayerMove();
        playerController.Move(playerVelocity * Time.deltaTime);

        //Mathf.Clamp(playerVelocity.y, -0.1f, jumpHeight);
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
