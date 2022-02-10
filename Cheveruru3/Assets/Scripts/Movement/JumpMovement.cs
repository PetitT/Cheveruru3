using LowTeeGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : BaseMovement
{
    private InputActions actions;

    private float groundCheckRadius => movementData.GroundCheckRadius;
    private float defaultGravity => movementData.DefaultGravity;
    private float jumpCancelGravity => movementData.JumpCancelGravity;
    private float jumpApexGravity => movementData.JumpApexGravity;
    private float initialJumpForce => movementData.InitialJumpForce;
    private float timeBeforeGroundCheck => movementData.TimeBeforeGroundCheck;
    private float jumpRequestBufferTime => movementData.JumpRequestBufferTime;
    private float coyoteTime => movementData.CoyoteTime;
    private float jumpApexGravityTime => movementData.JumpApexGravityTime;

    private float currentGravity;
    private float currentYForce;
    private float remainingTimeToGroundCheck;
    private float remainingJumpRequestBufferTime;
    private float remainingCoyoteTime;
    private float remainingJumpApexGravityTime;
    private bool isGrounded;
    private bool hasReachedApex;

    public override void Initialize()
    {
        actions = new InputActions();
        actions.Enable();

        actions.Character.Jump.performed += Jump_performed;
        actions.Character.Jump.canceled += Jump_canceled;
    }

    private void Jump_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        currentGravity = jumpCancelGravity;
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        remainingJumpRequestBufferTime = jumpRequestBufferTime;
    }

    public override void Update()
    {
        CheckForGround();
        ApplyGravity();
        CheckForJump();
        Movement = new Vector3(0, currentYForce, 0);
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            currentYForce += currentGravity * Time.deltaTime;
            if(!hasReachedApex && (currentYForce < 0))
            {
                currentGravity = jumpApexGravity;
                remainingJumpApexGravityTime = jumpApexGravityTime;
                hasReachedApex = true;
            }
        }
        else
        {
            currentYForce = 0;
        }

        Timer.CountDown(ref remainingJumpApexGravityTime, () => currentGravity = defaultGravity);
    }

    private void CheckForGround()
    {
        remainingTimeToGroundCheck -= Time.deltaTime;
        remainingCoyoteTime -= Time.deltaTime;

        if (remainingTimeToGroundCheck <= 0)
        {
            isGrounded = Physics.OverlapSphere(MovementManager.Instance.GroundCheck.transform.position, groundCheckRadius, MovementManager.Instance.GroundLayer).Length > 0;
            if (isGrounded)
            {
                remainingCoyoteTime = coyoteTime;
                hasReachedApex = false;
            }
        }
    }

    private void CheckForJump()
    {
        if (remainingJumpRequestBufferTime > 0)
        {
            if (isGrounded || remainingCoyoteTime > 0)
            {
                Jump();

            }
        }

        remainingJumpRequestBufferTime -= Time.deltaTime;
    }

    private void Jump()
    {
        currentYForce = initialJumpForce;
        currentGravity = defaultGravity;
        remainingTimeToGroundCheck = timeBeforeGroundCheck;
        remainingCoyoteTime = 0;
        isGrounded = false;
    }
}
