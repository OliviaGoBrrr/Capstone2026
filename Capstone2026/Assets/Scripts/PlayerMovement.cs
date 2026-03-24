using UnityEngine;
using UnityEngine.InputSystem;
using System;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;

[RequireComponent(typeof(PlayerInput),typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    // Movement Values
    public float moveSpeed = 10f;
    public float jumpHeight = 2f;

    private Vector3 move;
    public Vector3 velocity;
    private float gravity = 9.82f;
    private float jumpTimeOffset = 0.2f;
    CapsuleCollider groundCheckCollider;
    float groundedTimer;
    float jumpTimer;
    float startHeight;
    public bool isGrounded;
    bool isJumping;


    // Components
    private Rigidbody rb;
    private PlayerInput playerInput;

    // Player Camera
    private GameObject playerCamera;
    private Transform lookPoint;
    private Quaternion cameraRotation;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        playerInput = GetComponent<PlayerInput>();
        groundCheckCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    private void Update()
    {
        isGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerJump();
    }

    public void OnJump()
    {
        if (IsGrounded() && jumpTimer <= 0.0f)
        {
            groundedTimer = 0.1f;
            velocity.y += jumpHeight;
            jumpTimer = 0.2f;
        }
    }

    bool IsGrounded()
    {
        if (groundedTimer <= 0.0f)
        {
            Vector3 groundCheck = transform.position;
            float distanceToGround = groundCheckCollider.bounds.extents.y;
            bool blah = Physics.Raycast(groundCheck, -Vector3.up, distanceToGround + 0.01f);
            Debug.DrawRay(groundCheck, -Vector3.up, Color.blue);
            return blah;
        }
        else
        {
           return false;
        }
    }


    private void PlayerJump()
    {
        jumpTimer -= Time.fixedDeltaTime;
        groundedTimer -= Time.fixedDeltaTime;
    }

    // Player Horizontal Movement
    public void OnMove(InputValue value)
    {
        var moveValue = value.Get<Vector2>();
        velocity.x = moveValue.x;
        velocity.z = moveValue.y;
    }
    private void PlayerMove()
    {
        Vector3 newPos = transform.position;

        if (!IsGrounded())
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            velocity.y = 0.0f;
        }

        velocity.y = Mathf.Clamp(velocity.y, -3f, 10f);

        newPos += velocity * moveSpeed * Time.fixedDeltaTime;
        
        rb.MovePosition(newPos);
    }

    private void FollowPlayerCamera()
    {
        cameraRotation = playerCamera.transform.rotation;
        lookPoint.transform.rotation = cameraRotation;
    }

}
