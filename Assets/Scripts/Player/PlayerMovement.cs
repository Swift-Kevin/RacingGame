using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerMovementRocketing rocketScript;

    [Seperator]
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAng;

    [Seperator]
    [Header("Colliders")]
    [SerializeField] private WheelCollider frontLeftCollider;
    [SerializeField] private WheelCollider frontRightCollider;
    [SerializeField] private WheelCollider backLeftCollider;
    [SerializeField] private WheelCollider backRightCollider;

    [Seperator]
    [Header("Transforms")]
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform backLeftTransform;
    [SerializeField] private Transform backRightTransform;

    private float currSteerAng;
    private float currentbreakForce;
    private bool isBreaking;
    private bool isRocketing;
    private Vector2 inp;

    public void Move()
    {
        GetInput();
        HandleMotorRpc(inp, isBreaking, isRocketing);
        HandleRocketing();
        HandleSteeringRpc(inp);
        UpdateWheelsRpc();
    }

    private void HandleRocketing()
    {
        if (isRocketing && rocketScript.CanRocket)
        {
            rocketScript.RocketForceRpc();
        }
    }

    [Rpc(SendTo.Server)]
    public void ForceStopRpc()
    {
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }

    private void GetInput()
    {
        isBreaking = InputManager.Instance.IsBreaking;
        inp = InputManager.Instance.MoveVec;
        isRocketing = rocketScript.RocketCheck();
    }

    [Rpc(SendTo.Server)]
    private void HandleMotorRpc(Vector2 _inp, bool _breaking, bool _rocketing)
    {
        // Update Colliders torque
        frontLeftCollider.motorTorque = _inp.y * motorForce;
        frontRightCollider.motorTorque = _inp.y * motorForce;

        if (Mathf.Abs(inp.y) == 0f)
        {
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.deltaTime / 2);
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.deltaTime / 2);
        }

        rocketScript.UpdateRocketVisualsRpc(_rocketing);

        // Need to have a consistent check back to is braking
        // since braking is can be on or off meaning we want the car to "slow" down
        currentbreakForce = _breaking ? breakForce : 0f;
        ApplyBrakingRpc(currentbreakForce);
    }

    [Rpc(SendTo.Server)]
    private void ApplyBrakingRpc(float _cbf)
    {
        // Update Front Brakes
        frontRightCollider.brakeTorque = _cbf;
        frontLeftCollider.brakeTorque = _cbf;

        // Update BackBrakes
        backRightCollider.brakeTorque = _cbf;
        backLeftCollider.brakeTorque = _cbf;
    }

    [Rpc(SendTo.Server)]
    private void HandleSteeringRpc(Vector2 _inp)
    {
        currSteerAng = maxSteerAng * _inp.x;

        // Update steering angle
        frontLeftCollider.steerAngle = currSteerAng;
        frontRightCollider.steerAngle = currSteerAng;
    }

    [Rpc(SendTo.Server)]
    private void UpdateWheelsRpc()
    {
        // Front wheels
        UpdateWheel(frontRightCollider, frontRightTransform);
        UpdateWheel(frontLeftCollider, frontLeftTransform);

        // Back wheels
        UpdateWheel(backRightCollider, backRightTransform);
        UpdateWheel(backLeftCollider, backLeftTransform);
    }

    private void UpdateWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;

        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
