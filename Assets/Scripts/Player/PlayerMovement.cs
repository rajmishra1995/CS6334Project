using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputAction playerInputAction;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private AnimationStateController animationStateController;

    private InputAction move;

    private Vector2 moveDirection;
    private Vector3 forceDirection;
    private Vector3 horizontalVelocity;

    public static AnimationStateController.animationState animationState;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Player.Jump.started += OnJump;
        move = playerInputAction.Player.Move;
        playerInputAction.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Jump.started -= OnJump;
        playerInputAction.Player.Disable();
    }

    void Start()
    {
        moveDirection = Vector2.zero;
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        forceDirection = forceDirection + moveDirection.x * GetCameraRight(playerCamera) * moveSpeed;
        forceDirection = forceDirection + moveDirection.y * GetCameraForward(playerCamera) * moveSpeed;

        playerRigidbody.AddForce(forceDirection, ForceMode.Impulse);

        DetermineAnimationState();

        forceDirection = Vector3.zero;

        // Incrementing the down force when the player is falling down
        if (playerRigidbody.velocity.y < 0f)
            playerRigidbody.velocity = playerRigidbody.velocity + Vector3.down * 50f * Time.deltaTime;

        // Limiting the speed of the player
        horizontalVelocity = playerRigidbody.velocity;
        horizontalVelocity.y = 0;
        
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            playerRigidbody.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * playerRigidbody.velocity.y;

        //ControlRotation();
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (groundCheck.isGrounded)
        {
            forceDirection = forceDirection + Vector3.up * jumpForce;
            groundCheck.isGrounded = false;
        }
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;

        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;

        return forward.normalized;
    }

    private void DetermineAnimationState()
    {
        Debug.Log(forceDirection.magnitude);

        // Idle
        if (forceDirection.magnitude < 0.1f)
            animationState = AnimationStateController.animationState.idle;

        // WalkingForward
        if (forceDirection.z > 0.1f && Mathf.Abs(forceDirection.x) < 0.25f)
            animationState = AnimationStateController.animationState.walkingForward;

        // WalkingBackwards
        if (forceDirection.z < -0.1f && Mathf.Abs(forceDirection.x) < 0.25f)
            animationState = AnimationStateController.animationState.walkingBackwards;
    }

    private void ControlRotation()
    {
        Vector3 direction = playerRigidbody.velocity;
        direction.y = 0f;

        // If the character is moving and the player is pressing the move buttons,
        // control the rotation of the character
        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.playerRigidbody.rotation = Quaternion.LookRotation(direction, Vector3.up);

        else
            playerRigidbody.angularVelocity = Vector3.zero;
    }
}
