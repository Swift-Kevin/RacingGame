using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerBase : NetworkBehaviour, ICheckpoint
{
    [Seperator]
    [SerializeField] private CustomTimer flippedOverTimer;
    [SerializeField] private PlayerMovement movementScript;

    [Seperator]
    [SerializeField] private PlayerCamera cameraScript;
    [SerializeField] private CarFollower carFollowScript;
    [SerializeField] private GameObject camPivot;

    [Seperator]
    [SerializeField] private PlayerFlipOver flipUIScript;
    [SerializeField] private PlayerAudio playerAudioScript;


    private NetworkVariable<int> currentCoins = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private Vector3 lastCheckpointPos;
    private Checkpoint lastCheckpoint;

    private bool isMoving;
    private bool isHonking;

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            lastCheckpointPos = GameManager.Instance.StartPoint;
            camPivot.transform.parent = null;

            flippedOverTimer.OnStart += () => { flipUIScript.ToggleFlipTextObj(true); };
            flippedOverTimer.OnTick += UpdateFlipUI;
            flippedOverTimer.OnEnd += FlipCar;

            return;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        flippedOverTimer.OnStart -= () => { flipUIScript.ToggleFlipTextObj(true); };
        flippedOverTimer.OnTick -= UpdateFlipUI;
        flippedOverTimer.OnEnd -= FlipCar;
    }

    void Update()
    {
        if (IsOwner)
        {
            cameraScript.Look();
            carFollowScript.UpdateCam(transform.position);

            CheckIsFlipped();

            if (transform.position.y < -100f)
            {
                TeleportToLastCheckpointRpc();
                movementScript.ForceStopRpc();
                flipUIScript.ToggleFlipTextObj(false);
            }

            isMoving = Mathf.Abs(InputManager.Instance.MoveVec.x) > 0 || Mathf.Abs(InputManager.Instance.MoveVec.y) > 0;
            isHonking = InputManager.Instance.IsHonking;

            UXCompsRpc(isMoving, isHonking);
        }
    }

    [Rpc(SendTo.Server)]
    private void UXCompsRpc(bool isMoving, bool isHonking)
    {
        if (isMoving)
        {
            playerAudioScript.PlayMoveSFXRpc();
        }
        else
        {
            playerAudioScript.StopMoveSFXRpc();
        }

        if (isHonking)
        {
            playerAudioScript.PlayCarHonkSFXRpc();
        }
        else
        {
            playerAudioScript.StopCarHonkSFXRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void TeleportToLastCheckpointRpc()
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
        TeleportToLastCheckpointRpc();
    }

    public void AddPoint()
    {
        if (IsServer)
        {
            currentCoins.Value = currentCoins.Value + 1;
        }
        UIManager.Instance.PlayerUIScript.UpdateScoreUI(currentCoins.Value);
    }

    [Rpc(SendTo.Server)]
    private void FlipPlayerCarRpc()
    {
        Vector3 flipPos = transform.position;
        flipPos.y += 2;
        transform.position = flipPos;

        var oldRot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(oldRot.x, oldRot.y, 0);
    }

    private void CheckIsFlipped()
    {
        bool isPlayerFlipped = Vector3.Dot(transform.up, Vector3.up) < 0.05;

        // If the up directions are facing away from each other
        if (isPlayerFlipped && !flippedOverTimer.RunTimer)
        {
            flippedOverTimer.StartTimer();
        }
        else if (!isPlayerFlipped && flippedOverTimer.RunTimer)
        {
            flippedOverTimer.StopTimer();
            flipUIScript.UpdateFlipTimerText(0);
            flipUIScript.ToggleFlipTextObj(false);
        }
    }

    private void FlipCar()
    {
        FlipPlayerCarRpc();
        movementScript.ForceStopRpc();
        flipUIScript.ToggleFlipTextObj(false);
    }

    private void UpdateFlipUI()
    {
        flipUIScript.UpdateFlipTimerText(flippedOverTimer.Percentage);
    }
}
