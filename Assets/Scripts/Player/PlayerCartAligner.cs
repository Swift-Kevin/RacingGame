using UnityEngine;

public class PlayerCartAligner : MonoBehaviour
{
    private void Update()
    {
        Vector3 v = Camera.main.transform.forward;
        v.y = 0;

        transform.forward = v;
    }
}
