using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
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
    private Vector2 inp;

    public void Move()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    public void ForceStop()
    {
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
    }

    private void GetInput()
    {
        isBreaking = InputManager.Instance.IsBreaking;
        inp = InputManager.Instance.MoveVec;
    }

    private void HandleMotor()
    {
        // Update Colliders torque
        frontLeftCollider.motorTorque = inp.y * motorForce;
        frontRightCollider.motorTorque = inp.y * motorForce;

        // Need to have a consistent check back to is braking
        // since braking is can be on or off meaning we want the car to "slow" down
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBraking();
    }

    private void ApplyBraking()
    {
        // Update Front Brakes
        frontRightCollider.brakeTorque = currentbreakForce;
        frontLeftCollider.brakeTorque = currentbreakForce;

        // Update BackBrakes
        backRightCollider.brakeTorque = currentbreakForce;
        backLeftCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currSteerAng = maxSteerAng * inp.x;

        // Update steering angle
        frontLeftCollider.steerAngle = currSteerAng;
        frontRightCollider.steerAngle = currSteerAng;
    }

    private void UpdateWheels()
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
