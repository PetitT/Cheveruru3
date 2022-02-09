using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovement 
{
    public Vector3 Movement { get; protected set; }

    public abstract void Initialize();
    public abstract void Update();
}
