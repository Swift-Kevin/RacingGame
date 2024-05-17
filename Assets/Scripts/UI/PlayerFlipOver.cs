using UnityEngine;
using UnityEngine.UI;

public class PlayerFlipOver : MonoBehaviour
{
    [SerializeField] private Image flipImg;
    [SerializeField] private GameObject flipObj;

    public void UpdateFlipTimerText(float _percent)
    {
        flipImg.fillAmount = _percent;

        if (_percent <= 0.01)
        {
            flipImg.gameObject.SetActive(false);
        }
        else if (_percent >= 0.99)
        {
            flipImg.gameObject.SetActive(true);
        }
    }

    public void ToggleFlipTextObj(bool _status)
    {
        flipObj.SetActive(_status);
    }
}
