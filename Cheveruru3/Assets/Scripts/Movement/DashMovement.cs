using LowTeeGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DashMovement : BaseMovement
{
    private InputActions actions;
    private Camera cam;
    private float dashForce => movementData.DashForce;
    private AnimationCurve dashForceCurve => movementData.DashForceCurve;
    private float speedFalloff => movementData.DashForceFalloff;
    private float dashCoolDown => movementData.DashCooldown;
    private float dashBufferTime => movementData.DashBufferTime;
    private float wallCheckDistance => movementData.wallCheckDistance;
    private float wallCheckWidth => movementData.sphereCastWidth;
    private LayerMask wallLayer => movementManager.WallLayer;
    private float dashTime => dashForceCurve.keys.Last().time;

    private float remainingDashTime;
    private float remainingDashCooldown;
    private float remainingDashBufferTime;

    public Vector3 dashDirection { get; private set; }

    public bool IsDashing => remainingDashTime > 0;
    private bool CanDash => remainingDashCooldown < 0;


    public event Action onDashBegin;
    public override void Initialize()
    {
        actions = new InputActions();
        actions.Enable();

        cam = Camera.main;

        actions.Character.Dash.performed += Dash_performed;
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        remainingDashBufferTime = dashBufferTime;
    }

    public override void Update()
    {
        CheckForDash();
        DoDash();
    }

    private void CheckForDash()
    {
        if (remainingDashBufferTime > 0 && CanDash)
        {
            remainingDashTime = dashTime;
            remainingDashCooldown = dashCoolDown;
            remainingDashBufferTime = 0;

            Vector2 moveDirection = movementManager.WalkMovement.targetDirection;

            //Should find a way to dash forward when not moving
            if (moveDirection == Vector2.zero)
            {
                moveDirection = movementManager.Character.transform.forward;
            }

            Vector3 camForward = cam.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = cam.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            dashDirection = camForward * moveDirection.y + camRight * moveDirection.x;

            onDashBegin?.Invoke();
        }

        remainingDashBufferTime -= Time.deltaTime;
    }

    private void DoDash()
    {
        remainingDashCooldown -= Time.deltaTime;

        if (IsDashing)
        {
            if (CheckForWalls(dashDirection))
            {
                remainingDashTime = 0;
                Movement = Vector3.zero;
            }
            else
            {
                remainingDashTime -= Time.deltaTime;
                float speedMultiplicator = dashForceCurve.Evaluate(dashTime - remainingDashTime) * dashForce;
                Movement = dashDirection * speedMultiplicator;
            }
        }
        else
        {
            if (CheckForWalls(Movement))
            {
                Movement = Vector3.zero;
            }

            else
            {
                Movement = Vector3.MoveTowards(Movement, Vector3.zero, speedFalloff * Time.deltaTime);
            }
        }
    }


    private bool CheckForWalls(Vector3 dashDirection)
    {
        return Physics.SphereCast(movementManager.Character.transform.position, wallCheckWidth, dashDirection, out RaycastHit hit, wallCheckDistance, wallLayer);
    }
}
