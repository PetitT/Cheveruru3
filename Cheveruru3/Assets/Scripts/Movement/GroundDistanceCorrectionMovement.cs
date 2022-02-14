using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDistanceCorrectionMovement : BaseMovement
{
    private float movementCorrectionSpeed => movementData.movementCorrectionSpeed;
    private float minDistanceFromGround => movementData.minDistanceFromGround;
    private bool isGrounded => movementManager.JumpMovement.IsGrounded;
    private LayerMask groundMask => movementManager.GroundLayer;

    //private float movementDeceleration = 5;

    public override void Update()
    {
        Decelerate();
        if (isGrounded)
        {
            CheckDistanceFromGround();
        }

    }

    private void Decelerate()
    {
        Movement = Vector3.zero;


        //Movement = Vector3.MoveTowards(Movement, Vector3.zero, movementDeceleration * Time.deltaTime);
    }

    private void CheckDistanceFromGround()
    {
        if (Physics.Raycast(movementManager.Character.transform.position, Vector3.down, out RaycastHit hit, minDistanceFromGround, groundMask))
        {
            if (Vector3.Distance(movementManager.Character.transform.position, hit.point) < minDistanceFromGround)
            {
                Movement = movementCorrectionSpeed * Vector3.up;
            }
        }
    }

    public override void Initialize()
    {
    }
}
