using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : BaseMovement
{
    private InputActions actions;

    private float maximumSpeed => movementData.maximumSpeed;
    private float acceleration => movementData.acceleration;
    private float directionChangeAcceleration => movementData.directionChangeAcceleration;

    private float targetSpeed;
    private float currentSpeed;

    private Vector2 currentDirection;
    private Vector2 targetDirection;

    public override void Initialize()
    {
        actions = new InputActions();
        actions.Enable();
        actions.Character.Movement.performed += Movement_performed;
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
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

    private void UpdateDirection()
    {
        currentDirection = Vector2.MoveTowards(currentDirection, targetDirection, directionChangeAcceleration * Time.deltaTime);
    }

    private void Move()
    {
        Movement = new Vector3(currentDirection.x, 0, currentDirection.y) * currentSpeed;
    }
}
