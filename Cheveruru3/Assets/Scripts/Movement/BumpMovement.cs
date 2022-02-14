using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpMovement : BaseMovement
{
    private Vector3 bumpDirection;
    private float remainingBumpForce;
    private float bumpForceFalloff => movementData.BumpFalloff;
    private float bumpForce => movementData.BumpForce;

    public void Bump(Vector3 direction, float force)
    {
        bumpDirection = direction;
        remainingBumpForce = force;
    }

    public override void Initialize()
    {
        CharacterCollision.Instance.onCollided += Instance_onCollided;
    }

    private void Instance_onCollided(Collider obj)
    {
        Vector3 direction = (movementManager.Character.transform.position - obj.gameObject.transform.position).normalized;
        direction.y = 0;
        Bump(direction, bumpForce);
    }

    public override void Update()
    {
        DoBump();
    }

    private void DoBump()
    {
        if (remainingBumpForce > 0)
        {
            if (CheckForWalls(bumpDirection))
            {
                remainingBumpForce = 0;
            }
            else
            {
                remainingBumpForce -= Time.deltaTime * bumpForceFalloff;
            }

            Movement = bumpDirection * remainingBumpForce;
        }
    }


    private bool CheckForWalls(Vector3 dashDirection)
    {
        return Physics.SphereCast(movementManager.Character.transform.position, movementData.sphereCastWidth, dashDirection, out RaycastHit hit, movementData.wallCheckDistance, movementManager.WallLayer);
    }
}
