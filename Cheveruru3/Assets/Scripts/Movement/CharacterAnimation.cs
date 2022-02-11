using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public GameObject model;

    private void Update()
    {
        Vector3 forward = MovementManager.Instance.WalkMovement.Movement.normalized;
        if (forward != Vector3.zero)
        {
            model.transform.forward = forward;
        }
    }
}
