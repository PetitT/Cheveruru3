using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : BaseMovement
{
    private InputActions actions;
    private Camera cam;

    private float maximumSpeed => movementData.maximumSpeed;
    private float acceleration => movementData.acceleration;
    private float directionChangeAcceleration => movementData.directionChangeAcceleration;
    private float wallCheckDistance => movementData.wallCheckDistance;
    private float wallCheckWidth => movementData.sphereCastWidth;
    private LayerMask wallLayer => movementManager.WallLayer;

    private float targetSpeed;
    private float currentSpeed;

    private Vector2 currentDirection;
    public Vector2 targetDirection { get; private set; }

    public override void Initialize()
    {
        actions = new InputActions();
        actions.Enable();
        actions.Character.Movement.performed += Movement_performed;
        cam = Camera.main;
    }

    private void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        targetDirection = obj.ReadValue<Vector2>();
        targetSpeed = targetDirection == Vector2.zero ? 0 : maximumSpeed;
    }

    public override void Update()
    {
        Accelerate();
        UpdateDirection();
        Move();
    }

    private void Accelerate()
    {
        if (!movementManager.DashMovement.IsDashing)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;
        }
    }

    private void UpdateDirection()
    {
        currentDirection = Vector2.MoveTowards(currentDirection, targetDirection, directionChangeAcceleration * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 movement = camForward * currentDirection.y + camRight * currentDirection.x;

        CheckForWalls(movement);
        Movement = movement * currentSpeed;
    }

    private void CheckForWalls(Vector3 movement)
    {
        if (Physics.SphereCast(movementManager.Character.transform.position, wallCheckWidth, movement, out RaycastHit hit, wallCheckDistance, wallLayer))
        {
            currentSpeed = 0;
        }
    }
}
