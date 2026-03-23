using UnityEngine;
using UnityEngine.InputSystem;
using System;
using NUnit.Framework.Constraints;

[RequireComponent(typeof(PlayerInput),typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    // Movement Values
    public float moveSpeed = 10f;
    public float jumpHeight = 2f;

    private Vector3 move;
    private Vector3 velocity;
    private float gravity = 9.82f;
    private float jumpTimeOffset = 0.2f;
    float jumpTimer;
    float startHeight;
    bool isGrounded;
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
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerMove();
    }

    public void OnJump()
    {
        if (IsGrounded())
        {
            velocity.y += jumpHeight;
            isJumping = true;
        }
        Debug.Log("Player pressed the jump key");
    }

    bool IsGrounded() { return Physics.Raycast(transform.position, -Vector3.up, 0.1f); }

    private void PlayerJump()
    {
        if (isJumping)
        {
            Vector3 newPos = transform.position;
            move.y += gravity * Time.deltaTime;
            rb.MovePosition(newPos);
        }
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
        newPos.x += velocity.x * moveSpeed * Time.deltaTime;
        newPos.z += velocity.z * moveSpeed * Time.deltaTime;

        rb.MovePosition(newPos);
    }

    private void FollowPlayerCamera()
    {
        cameraRotation = playerCamera.transform.rotation;
        lookPoint.transform.rotation = cameraRotation;
    }

}
