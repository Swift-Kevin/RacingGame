using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    private readonly NetworkVariable<int> _netMatIdx = new();
    
    [SerializeField] private List<Material> _materials;
    [SerializeField] private MeshRenderer _renderer;
    
    private int _index;

    private void Awake()
    {
        // Subscribing to a change event. This is how the owner will change its color.
        // Could also be used for future color changes
        _netMatIdx.OnValueChanged += OnValueChanged;
    }

    public override void OnDestroy()
    {
        _netMatIdx.OnValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(int prev, int next)
    {
        _renderer.material = _materials[next];
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _index = 0;
            CommitNetworkColorRpc(_index);
        }
        else
        {
            _renderer.material = _materials[_netMatIdx.Value];
        }
    }

    [Rpc(SendTo.Server)]
    private void CommitNetworkColorRpc(int color)
    {
        _netMatIdx.Value = color;
    }

    public void Triggered(int matIdx)
    {
        if (!IsOwner)
        {
            return;
        }

        CommitNetworkColorRpc(matIdx);
    }
}
