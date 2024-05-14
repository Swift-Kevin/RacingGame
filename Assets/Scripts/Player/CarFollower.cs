using System;
using UnityEngine;

public class CarFollower : MonoBehaviour
{
    public void UpdateCam(Vector3 _pos)
    {
        transform.position = _pos;
    }
}
