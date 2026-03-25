using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCCMovement : MonoBehaviour
{
    // Movement Values
    [Header("Player Movement Values")]
    public float moveSpeed = 10f;
    [Range(0f, 180f)] public float rotationSpeed = 180f;
    public float jumpHeight = 2f;

    public CharacterController playerController;
    private Vector3 playerVelocity;
    private Vector3 playerRotation;
    private bool groundedPlayer;

    [Header("Player Camera Values")]
    public Camera playerCamera;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    // Constants
    private const float gravityValue = -9.81f;

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
        this.playerRotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime, 0);

        // Get the x,z, direction the player is moving
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Transforms the direction in regards to direction of the camera
        move = playerCamera.transform.TransformDirection(move);
        Vector3 camDirection = playerCamera.transform.forward;
        camDirection = Vector3.ProjectOnPlane(camDirection, Vector3.up);

        // Stops the player from moving faster than they should (fixes the diagonal "boost")
        move = Vector3.ClampMagnitude(move, 1f);

        // If they're inputting a direction, move in relation to the camera direction
        if (move != Vector3.zero)
        {
            transform.forward = camDirection;
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
        this.transform.Rotate(this.playerRotation);
    }


    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }
}
