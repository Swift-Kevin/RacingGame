using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [Seperator]
    [SerializeField] private TextMeshProUGUI flipCountText;
    [SerializeField] private GameObject flipTimerObj;

    public void UpdateFlipTimerText(float _remainingTime)
    {
        

        flipCountText.text = _remainingTime.ToString("#.00");
    }

    public void ToggleFlipTextObj(bool _status)
    {
        flipTimerObj.SetActive(_status);
    }

    public void UpdateScoreUI(float _score)
    {
        scoreText.text = _score.ToString();
    }

}
