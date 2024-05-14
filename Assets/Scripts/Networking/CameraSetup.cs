using Unity.Netcode;
using UnityEngine;

public class CameraSetup : NetworkBehaviour
{
    

    public override void OnNetworkSpawn()
    {
        //base.OnNetworkSpawn();

        if (!IsOwner)
            return;

        Camera.main.transform.SetParent(transform);
        // Zero out
        Camera.main.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
