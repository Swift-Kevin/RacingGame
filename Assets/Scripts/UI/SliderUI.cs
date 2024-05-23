using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    [SerializeField] private string sliderName;
    [SerializeField] private float minVal;
    [SerializeField] private float maxVal;
    [Seperator]
    [SerializeField] private Slider sliderUI;
    [SerializeField] private TextMeshProUGUI displayText;

    public Slider Slider_UI => sliderUI;

    public float Min => minVal;
    public float Max => maxVal;

    public void TurnOn()
    {
        sliderUI.minValue = minVal;
        sliderUI.maxValue = maxVal;
        displayText.text = sliderName;
    }
}
