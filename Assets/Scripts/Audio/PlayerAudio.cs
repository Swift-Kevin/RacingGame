using Unity.Netcode;
using UnityEngine;

public class PlayerAudio : NetworkBehaviour
{
    [SerializeField] private AudioSource carHonkSFX;
    [SerializeField] private AudioSource moveSFX;

    [Rpc(SendTo.Everyone)]
    public void PlayCarHonkSFXRpc()
    {
        if (!carHonkSFX.isPlaying)
        {
            carHonkSFX.Play();
        }
    }

    [Rpc(SendTo.Everyone)]
    public void StopCarHonkSFXRpc()
    {
        if (carHonkSFX.isPlaying)
        {
            carHonkSFX.Stop();
        }
    }

    [Rpc(SendTo.Everyone)]
    public void PlayMoveSFXRpc()
    {
        if (!moveSFX.isPlaying)
        {
            moveSFX.Play();
        }
    }

    [Rpc(SendTo.Everyone)]
    public void StopMoveSFXRpc()
    {
        if (moveSFX.isPlaying)
        {
            moveSFX.Stop();
        }
    }
}
