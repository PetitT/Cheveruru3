using LowTeeGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoSingleton<MovementManager>
{
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public GameObject GroundCheck => groundCheck;
    public LayerMask GroundLayer => groundLayer;

    private MovementData movementData;
    public MovementData Data => movementData ??= MovementData.GetMovementData();

    private WalkMovement walkMovement = new WalkMovement();
    private JumpMovement jumpMovement = new JumpMovement();

    private List<BaseMovement> movements = new List<BaseMovement>();

    private void Awake()
    {
        movements.Add(walkMovement);
        movements.Add(jumpMovement);

        movements.ForEach(t => t.Initialize());
    }

    private void Update()
    {
        movements.ForEach(t => t.Update());
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector3 baseMovement = Vector3.zero;

        for (int i = 0; i < movements.Count; i++)
        {
            baseMovement += movements[i].Movement;
        }

        character.transform.Translate(baseMovement * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.transform.position, 0.4f);
    }
}
