using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void UpdateScoreUI(float _score)
    {
        scoreText.text = _score.ToString();
    }

}
