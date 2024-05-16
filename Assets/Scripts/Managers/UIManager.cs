using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private NetworkingUI networkingScript;
    [SerializeField] private Button btn_Quit;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject multiplayerMenu;
    [SerializeField] private GameObject playerUIObj;
    [SerializeField] private PlayerUI playerUIScript;

    public PlayerUI PlayerUIScript => playerUIScript;

    public bool isInGame = false;
    public bool isPauseOpened = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        btn_Quit.onClick.AddListener(QuitButtonPressed);
    }


    private void QuitButtonPressed()
    {
        GameManager.Instance.QuitApp();
    }

    public void HideAllMenus()
    {
        pauseMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
        playerUIObj.SetActive(false);
    }

    public void SetIsInGame(bool _status)
    {
        isInGame = _status;
        DisplayPlayerUI();
    }

    public void DisplayMultiplayerMenu()
    {
        HideAllMenus();
        multiplayerMenu?.SetActive(true);
    }

    private void DisplayPauseMenu()
    {
        HideAllMenus();
        pauseMenu?.SetActive(true);
    }

    private void DisplayPlayerUI()
    {
        HideAllMenus();
        playerUIObj?.SetActive(true);
    }

    public void TogglePauseMenu()
    {
        if (!isInGame)
            return;

        if (pauseMenu.activeSelf)
        {
            DisplayPlayerUI();
            GameManager.Instance.MouseLockHide();
            isPauseOpened = false;
        }
        else
        {
            DisplayPauseMenu();
            GameManager.Instance.MouseUnlockShow();
            isPauseOpened = true;
        }
    }
}
