using System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerCamera), typeof(PlayerMovement))]
public class PlayerBase : NetworkBehaviour, ICheckpoint
{
    [SerializeField] private PlayerCamera cameraScript;
    [SerializeField] private PlayerMovement movementScript;
    [SerializeField] private Rigidbody rb;

    private Vector3 lastCheckpointPos;
    private Checkpoint lastCheckpoint;

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            lastCheckpointPos = GameManager.Instance.StartPoint;
            return;
        }

        cameraScript.Cam.SetActive(false);
    }

    void Update()
    {
        if (!IsOwner)
            return;

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
        if (!IsOwner)
            return;

        movementScript.Move();
    }

    public void UpdateCheckpoint(Checkpoint _check, Transform _transform)
    {
        lastCheckpoint?.TurnOnCheckpoint();
        lastCheckpointPos = _transform.position;
        lastCheckpoint = _check;
    }

    public void SendToCheckpoint()
    {
        TeleportToLastCheckpoint();
    }
}
