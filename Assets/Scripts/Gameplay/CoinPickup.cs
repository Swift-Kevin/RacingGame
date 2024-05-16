using Unity.Netcode;
using UnityEngine;

public class CoinPickup : NetworkBehaviour
{
    [SerializeField] private NetworkObject netObj;
    [SerializeField] private Collider coll;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBase>().AddPoint();
            // avoid double collision
            coll.enabled = false;
            TellToDestroyRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void TellToDestroyRpc()
    {
        netObj.Despawn(true);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }
}
