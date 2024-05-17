using UnityEngine;

public class FaceMainCam : MonoBehaviour
{
    private void FixedUpdate()
    {
        Vector3 dir = transform.position - Camera.main.transform.position;
        dir.y = 0;
        dir.Normalize();

        transform.rotation = Quaternion.LookRotation(dir);
    }
}
