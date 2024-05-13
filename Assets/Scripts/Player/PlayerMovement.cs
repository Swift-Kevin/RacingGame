using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] private Rigidbody rb;
    //[SerializeField] private float maxSpeed = 10;
    //[SerializeField] private float speed = 1;

    //public void Move()
    //{
    //    // Get/Adjust camera's forward axis
    //    Vector3 camFWD = Camera.main.transform.forward;
    //    camFWD.y = 0;
    //    camFWD.Normalize();

    //    // Get/Adjust camera's right axis
    //    Vector3 camRht = Camera.main.transform.right;
    //    camRht.y = 0;
    //    camRht.Normalize();

    //    // Store inp values
    //    Vector2 inp = InputManager.Instance.MoveVec;

    //    // Calculate plannar input
    //    Vector3 planInp = Vector3.ClampMagnitude((camFWD * inp.y) + (camRht * inp.x), 1.0f);
    //    // Adds a force to the cart
    //    rb.AddForce(planInp * speed * Time.deltaTime, ForceMode.Impulse);

    //    // Store/Clamp Speed/Velocity
    //    float capY = rb.linearVelocity.y;
    //    Vector3 vel = rb.linearVelocity;
    //    vel = Vector3.ClampMagnitude(vel, maxSpeed);
    //    vel.y = capY;

    //    // actually sets the velocity to the clamped (adjusted) velocity, so we don't over accelerate
    //    rb.linearVelocity = vel;
    //}

    [Seperator]
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAng;
    [SerializeField] private float maxSpeed;

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
    private float currentSpeed;
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

        if (inp.y > 0 && currentSpeed < maxSpeed)
        {
            currentSpeed += Time.deltaTime;
        }
        else if (inp.y <= 0)
        {
            currentSpeed -= Time.deltaTime;
        }
    }

    private void HandleMotor()
    {
        // Update Colliders torque
        frontLeftCollider.motorTorque = inp.y * motorForce;
        frontRightCollider.motorTorque = inp.y * motorForce;

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
