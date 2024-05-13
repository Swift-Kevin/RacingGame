using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ICheckpoint check = other.GetComponent<ICheckpoint>();

        if (check != null)
        {
            check.SendToCheckpoint();
        }
    }
}
