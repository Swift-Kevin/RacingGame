using System;
using UnityEngine;

[RequireComponent(typeof(PlayerCamera), typeof(PlayerMovement))]
public class PlayerBase : MonoBehaviour, ICheckpoint
{
    [SerializeField] private PlayerCamera cameraScript;
    [SerializeField] private PlayerMovement movementScript;
    [SerializeField] private Rigidbody rb;

    private Vector3 lastCheckpointPos;
    private Checkpoint lastCheckpoint;

    private void Start()
    {
        lastCheckpointPos = GameManager.Instance.StartPoint;
    }

    void Update()
    {
        cameraScript.Look();

        // If the up directions are facing away from each other
        if (Vector3.Dot(transform.up, Vector3.up) < 0)
        {
            TeleportToLastCheckpoint();
            movementScript.ForceStop();
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void TeleportToLastCheckpoint()
    {
        transform.position = lastCheckpointPos;

        var oldRot = transform.rotation;
        transform.rotation = Quaternion.Euler(oldRot.x, oldRot.y, 0);
    }

    private void FixedUpdate()
    {
        movementScript.Move();
    }

    public void UpdateCheckpoint(Checkpoint _check, Transform _transform)
    {
        lastCheckpoint?.TurnOnCheckpoint();
        lastCheckpointPos = _transform.position;
        lastCheckpoint = _check;
    }
}
