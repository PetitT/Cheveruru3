using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovement 
{
    public Vector3 Movement { get; protected set; }

    protected MovementManager movementManager => MovementManager.Instance;
    protected MovementData movementData => movementManager.Data;

    public abstract void Initialize();
    public abstract void Update();
}
