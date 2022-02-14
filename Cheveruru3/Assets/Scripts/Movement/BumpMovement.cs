using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpMovement : BaseMovement
{
    private Vector3 bumpDirection;
    private float remainingBumpForce;

    public void Bump(Vector3 direction, float force)
    {
        bumpDirection = direction;
        remainingBumpForce = force;
    }

    public override void Initialize()
    {
        
    }

    public override void Update()
    {
       
    }
}
