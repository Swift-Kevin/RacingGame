using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button button_Continue;
    [SerializeField] Button button_Exit;

    void Start()
    {
        button_Continue.onClick.AddListener(ContinueButtonFunctionality);
        button_Exit.onClick.AddListener(ExitButtonFunctionality);
    }

    private void ContinueButtonFunctionality()
    {
        UIManager.Instance.TogglePauseMenu();
    }

    private void ExitButtonFunctionality()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
