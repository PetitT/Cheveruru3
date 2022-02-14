using LowTeeGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoSingleton<MovementManager>
{
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    public GameObject Character => character;
    public GameObject GroundCheck => groundCheck;
    public LayerMask GroundLayer => groundLayer;
    public LayerMask WallLayer => wallLayer;
    public GameObject CameraTarget => cameraTarget;

    private MovementData movementData;
    public MovementData Data => movementData ??= MovementData.GetMovementData();

    public WalkMovement WalkMovement { get; private set; } = new WalkMovement();
    public JumpMovement JumpMovement { get; private set; } = new JumpMovement();
    public DashMovement DashMovement { get; private set; } = new DashMovement();
    public GroundDistanceCorrectionMovement GroundDistanceCorrectionMovement { get; private set; } = new GroundDistanceCorrectionMovement();
    public BumpMovement BumpMovement { get; private set; } = new BumpMovement();

    private List<BaseMovement> movements = new List<BaseMovement>();

    private void Awake()
    {
        movements.Add(WalkMovement);
        movements.Add(JumpMovement);
        movements.Add(DashMovement);
        movements.Add(GroundDistanceCorrectionMovement);
        movements.Add(BumpMovement);

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

        Transform camera = Camera.main.transform;
        Vector3 camPos = camera.position;
        Vector3 dir = camera.forward;
        dir.y = 0;
        dir.Normalize();

        Gizmos.DrawLine(camPos, camPos + dir);
    }
}
