using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementData")]
public class MovementData : ScriptableObject
{
    [Header("Run")]
    public float maximumSpeed = 10;
    public float acceleration = 10;
    public float directionChangeAcceleration = 7;

    [Header("Jump")]
    public float InitialJumpForce = 15f;
    public float DefaultGravity = -50f;
    public float JumpCancelGravity = -100f;
    public float JumpApexGravity = -10f;
    public float TerminalVelocity = -10f;
    public float GroundCheckRadius = 0.4f;
    public float TimeBeforeGroundCheck = 0.1f;
    public float JumpRequestBufferTime = 0.1f;
    public float CoyoteTime = 0.5f;
    public float JumpApexGravityTime = 0.05f;

    [Header("Dash")]
    public float DashForce;
    public float DashForceFalloff;
    public float DashCooldown;
    public float DashBufferTime;
    public AnimationCurve DashForceCurve;

    [Header("Bump")]
    public float BumpForce;
    public float BumpFalloff;

    [Header("Collision")]
    public float wallCheckDistance = 1;
    public float minDistanceFromGround;
    public float movementCorrectionSpeed = 1f;
    public float sphereCastWidth;

    public static MovementData GetMovementData()
    { 
        return Resources.Load<MovementData>("MovementData");
    }
}
