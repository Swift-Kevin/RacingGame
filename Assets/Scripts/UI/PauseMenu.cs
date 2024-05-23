using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button button_Continue;
    [SerializeField] Button button_Options;
    [SerializeField] Button button_Exit;

    void Start()
    {
        button_Continue.onClick.AddListener(ContinueButtonFunctionality);
        button_Options.onClick.AddListener(OptionsButtonFunctionality);
        button_Exit.onClick.AddListener(ExitButtonFunctionality);
    }

    private void ContinueButtonFunctionality()
    {
        UIManager.Instance.TogglePauseMenu();
    }

    private void OptionsButtonFunctionality()
    {
        UIManager.Instance.DisplayOptionsMenu();
    }


    private void ExitButtonFunctionality()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
