using System;
using UnityEngine;
using UnityEngine.UI;

public class RocketChargeUI : MonoBehaviour
{
    [SerializeField] private Image leftVisual;
    [SerializeField] private Image rightVisual;
    [SerializeField] private GameObject objVisual;

    public void Toggle(bool _status)
    {
        objVisual.SetActive(_status);
    }

    public void SetVal(float _percent)
    {
        leftVisual.fillAmount = _percent;
        rightVisual.fillAmount = _percent;
    }
}
