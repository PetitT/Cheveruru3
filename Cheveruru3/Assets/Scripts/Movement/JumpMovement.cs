using LowTeeGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : BaseMovement
{
    private InputActions actions;

    private float groundCheckRadius = 0.4f;
    private float gravity = -50f;
    private float initialJumpForce = 15f;
    private float timeBeforeGroundCheck = 0.5f;
    private float jumpRequestBufferTime = 0.1f;
    private float coyoteTime = 0.5f;

    private float currentYForce;
    private bool isGrounded;
    private float remainingTimeToGroundCheck;
    private float remainingJumpRequestBufferTime;
    private float remainingCoyoteTime;

    public override void Initialize()
    {
        actions = new InputActions();
        actions.Enable();

        actions.Character.Jump.performed += Jump_performed;
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
            currentYForce += gravity * Time.deltaTime;
        }
        else
        {
            currentYForce = 0;
        }
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
        remainingTimeToGroundCheck = timeBeforeGroundCheck;
        remainingCoyoteTime = 0;
        isGrounded = false;
    }
}
