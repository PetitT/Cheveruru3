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
    private float terminalVelocity => movementData.TerminalVelocity;
    private GameObject cameraTarget => movementManager.CameraTarget;

    private float currentGravity;
    private float currentYForce;
    private float remainingTimeToGroundCheck;
    private float remainingJumpRequestBufferTime;
    private float remainingCoyoteTime;
    private float remainingJumpApexGravityTime;
    private bool isGrounded = false;
    private bool hasReachedApex = false;
    private float targetCameraYPos;
    private float initialCameraYPos;

    public override void Initialize()
    {
        actions = new InputActions();
        actions.Enable();

        actions.Character.Jump.performed += Jump_performed;
        actions.Character.Jump.canceled += Jump_canceled;

        movementManager.DashMovement.onDashBegin += DashMovement_onDashBegin;

        initialCameraYPos = movementManager.CameraTarget.transform.localPosition.y;
        currentGravity = defaultGravity;
    }

    private void DashMovement_onDashBegin()
    {
        Jump(10);
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
        MoveCameraTarget();
        Movement = new Vector3(0, currentYForce, 0);
    }

    private void MoveCameraTarget()
    {
        float targetY = isGrounded ? initialCameraYPos : targetCameraYPos;
        float distance = Mathf.Abs(cameraTarget.transform.position.y - targetY);
        float newY = Mathf.MoveTowards(cameraTarget.transform.position.y, targetY, distance * Time.deltaTime);
        movementManager.CameraTarget.transform.position = new Vector3(movementManager.Character.transform.position.x, newY, movementManager.Character.transform.position.z);
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            currentYForce += currentGravity * Time.deltaTime;
            currentYForce = Mathf.Max(currentYForce, terminalVelocity);
            if (!hasReachedApex && (currentYForce < 0))
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
                initialCameraYPos = movementManager.Character.transform.position.y;
            }
        }
    }

    private void CheckForJump()
    {
        if (remainingJumpRequestBufferTime > 0)
        {
            if (isGrounded || remainingCoyoteTime > 0)
            {
                Jump(initialJumpForce);
            }
        }

        remainingJumpRequestBufferTime -= Time.deltaTime;
    }

    private void Jump(float force)
    {
        currentYForce = force;
        currentGravity = defaultGravity;
        remainingTimeToGroundCheck = timeBeforeGroundCheck;
        remainingCoyoteTime = 0;
        isGrounded = false;
        targetCameraYPos = movementManager.CameraTarget.transform.position.y;
    }
}
