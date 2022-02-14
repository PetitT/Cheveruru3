using LowTeeGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoSingleton<CharacterCollision>
{
    public event Action<Collider> onCollided;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DamagingObject damagingObject))
        {
            onCollided?.Invoke(other);
        }
    }
}
