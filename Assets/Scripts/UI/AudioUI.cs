using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] private SliderUI masterSlider;

    private void Start()
    {
        masterSlider.TurnOn();
        masterSlider.Slider_UI.onValueChanged.AddListener(UpdateMasterSlider);
    }

    private void UpdateMasterSlider(float value)
    {
        AudioManager.Instance.UpdateMasterVolume(Mathf.Clamp(value, masterSlider.Min, masterSlider.Max));
    }

}

