using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovementRocketing : NetworkBehaviour
{
    [Seperator]
    [SerializeField] private GameObject rocketParticles;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rocketSpeed;
    [SerializeField] private RocketFuelPool rocketFuel;

    [Seperator]
    [SerializeField] private CustomTimer rechargeWaitTimer;
    [SerializeField] private CustomTimer rechargeTimer;

    public float RocketBoostSpeed => rocketSpeed;
    public bool CanRocket => rocketFuel.IsValid;

    private bool isRocketing;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rocketFuel.SetMax();

        rechargeWaitTimer.OnEnd += RechargeWaitTimer_OnEnd;
        rechargeTimer.OnStart += RechargeTimer_OnStart;
        rechargeTimer.OnTick += RechargeTimer_OnTick;
        rechargeTimer.OnEnd += RechargeTimer_OnEnd;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        rechargeWaitTimer.OnEnd -= RechargeWaitTimer_OnEnd;
        rechargeTimer.OnStart -= RechargeTimer_OnStart;
        rechargeTimer.OnTick -= RechargeTimer_OnTick;
        rechargeTimer.OnEnd -= RechargeTimer_OnEnd;
    }

    public void RocketForce()
    {
        rocketFuel.Decrease(Time.deltaTime * 5);
        UIManager.Instance.RocketUIScript.SetVal(rocketFuel.Percent);
        rb.AddForce(transform.forward * rocketSpeed, ForceMode.Impulse);
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateRocketVisualsRpc(bool _isRocketing)
    {
        rocketParticles.gameObject.SetActive(_isRocketing);
    }

    public bool RocketCheck()
    {
        isRocketing = InputManager.Instance.IsRocketing;

        if (!isRocketing || !rocketFuel.IsMaxed || !rechargeWaitTimer.RunTimer)
        {
            rechargeWaitTimer.StartTimer();
        }
        else if (isRocketing)
        {
            rechargeWaitTimer.StopTimer();
            rechargeTimer.StopTimer();
        }

        return isRocketing;
    }

    #region Timers

    private void RechargeWaitTimer_OnEnd()
    {
        rechargeTimer.StartTimer();
    }

    private void RechargeTimer_OnStart()
    {
        UIManager.Instance.RocketUIScript.Toggle(true);
    }

    private void RechargeTimer_OnTick()
    {
        if (isRocketing)
        {
            rechargeTimer.StopTimer();
            return;
        }

        if (!rocketFuel.IsMaxed)
        {
            rocketFuel.Increase(Time.deltaTime);
        }
        else
        {
            rechargeTimer.StopTimer();
        }

        UIManager.Instance.RocketUIScript.SetVal(rocketFuel.Percent);
    }

    private void RechargeTimer_OnEnd()
    {
        rocketFuel.SetMax();
        UIManager.Instance.RocketUIScript.Toggle(false);
    }

    #endregion
}
