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
    public float gravityValue = -9.81f;

    float turnSpeedVelocity;
    float turnSmoothTime;

    [HideInInspector]
    public Vector3 playerVelocity;
    private Vector3 playerRotation;
    private bool groundedPlayer;

    [Header("Player Camera Values")]
    public CharacterController playerController;
    public Camera playerCamera;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference runAction;
    public InputActionReference grappleAction;

    

    

    

    private void Start()
    {
        // Cursor is invisible and is confined to screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        groundedPlayer = playerController.isGrounded;

        if (groundedPlayer)
        {
            if(playerVelocity.y < -2f)
            {
                playerVelocity.y = -2f;
            }
        }

        PlayerMove();
    }

    private void PlayerMove()
    {
        // Rotate the player with the direction they're walking towards
        //this.playerRotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime, 0);

        // Get the x,z direction the player is inputting
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Debug.Log(input);

        Vector3 move = new Vector3(input.x, 0, input.y);

        Vector3 camF = playerCamera.transform.forward;
        Vector3 camR = playerCamera.transform.right;

        camF.y = 0f;
        camR.y = 0f;

        camF.Normalize();
        camR.Normalize();

        // Stops the player from moving faster than they should (fixes the diagonal "boost")
        move = Vector3.ClampMagnitude(move, 1f);

        // If they're inputting a direction, move in relation to the camera direction
        if (move != Vector3.zero)
        {
            Vector3 desiredMove = (camF * input.y + camR * input.x);
            move = desiredMove;
            transform.forward = move;
        }

        // Jump handling
        if(groundedPlayer && jumpAction.action.WasPressedThisFrame())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // Gravity enacting on the player
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Calculate where the player is going, then move and rotate them
        Vector3 finalMove = move * moveSpeed + Vector3.up * playerVelocity.y;
        playerController.Move(finalMove * Time.deltaTime);
        //this.transform.Rotate(this.playerRotation);
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        runAction.action.Enable();
        grappleAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        runAction.action.Disable();
        grappleAction.action.Disable();
    }
}
