using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTimer : MonoBehaviour
{

    [SerializeField] private string TimerName;

    //Events
    public event System.Action OnEnd;
    public event System.Action OnStart;
    public event System.Action OnRestart;
    public event System.Action OnTick;

    //Values
    [SerializeField] float duration;
    private float currTime;
    public bool RunTimer = false;

    //Properties
    public float CurrentTime => currTime;
    public float DurationTime => duration;
    public float Percentage => Mathf.Clamp01(currTime / duration);
    public float ReversePercentage => Mathf.Clamp01((duration - currTime) / duration);
    public float RemainingTime => duration * Percentage;

    public void SetToMax()
    {
        currTime = duration;
        RunTimer = true;
    }

    public void RestartTimer()
    {
        SetToMax();
        OnRestart?.Invoke();
    }

    public void StartTimer(float _duration = 0f, bool _override = false)
    {
        if (_override)
        {
            SetToMax();
            OnStart?.Invoke();
            return;
        }

        if (_duration != 0)
            SetDuration(_duration);

        if (!RunTimer)
        {
            SetToMax();
            OnStart?.Invoke();
        }
    }

    public void SetDuration(float _duration)
    {
        duration = _duration;
    }

    public void StopTimer()
    {
        RunTimer = false;
        currTime = 0;
    }

    void Update()
    {
        if (RunTimer)
        {
            if (currTime > 0)
            {
                currTime -= Time.deltaTime;
                OnTick?.Invoke();
            }
            else
            {
                currTime = 0;
                OnEnd?.Invoke();
                RunTimer = false;
            }
        }
    }
}
