using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform playerObj;
    [SerializeField] private float camLookSpeed = 10f;

    private float yRot;
    private float xRot;

    public void Look()
    {
        Vector2 lookVec = InputManager.Instance.LookVec;
        var yawDelt = lookVec.x * Time.deltaTime * camLookSpeed;
        var pitchDelt = lookVec.y * Time.deltaTime * camLookSpeed;

        yRot += yawDelt;
        xRot -= pitchDelt;

        yRot = yRot >= 360 ? yRot - 360 : yRot < 0 ? yRot + 360 : yRot;
        xRot = Mathf.Clamp(xRot, -85, 85);

        //cameraPivot.localRotation = Quaternion.Euler(xRot, 0, 0);
        cameraPivot.localRotation = Quaternion.Euler(0, yRot, 0);
    }
}
