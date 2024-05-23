using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Button btn_Quit;
    
    [Seperator]
    [SerializeField] private GameObject objPauseUI;
    [SerializeField] private GameObject objNetworkUI;
    [SerializeField] private GameObject objPlayerUI;
    [SerializeField] private GameObject objOptionsUI;
    
    [Seperator]
    [SerializeField] private PlayerUI playerUIScript;
    [SerializeField] private RocketChargeUI rocketUIScript;
    [SerializeField] private NetworkingUI networkingScript;

    public PlayerUI PlayerUIScript => playerUIScript;
    public RocketChargeUI RocketUIScript => rocketUIScript;

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
        objPauseUI.SetActive(false);
        objNetworkUI.SetActive(false);
        objPlayerUI.SetActive(false);
        objOptionsUI.SetActive(false);
    }

    public void SetIsInGame(bool _status)
    {
        isInGame = _status;
        DisplayPlayerUI();
    }

    public void DisplayMultiplayerMenu()
    {
        HideAllMenus();
        objNetworkUI?.SetActive(true);
    }

    private void DisplayPauseMenu()
    {
        HideAllMenus();
        objPauseUI?.SetActive(true);
        GameManager.Instance.MouseUnlockShow();
        isPauseOpened = true;
    }

    private void DisplayPlayerUI()
    {
        HideAllMenus();
        objPlayerUI?.SetActive(true);
        GameManager.Instance.MouseLockHide();
        isPauseOpened = false;
    }

    public void TogglePauseMenu()
    {
        if (!isInGame)
            return;

        if (objPauseUI.activeSelf)
        {
            DisplayPlayerUI();
        }
        else
        {
            DisplayPauseMenu();
        }
    }

    public void DisplayOptionsMenu()
    {
        HideAllMenus();
        objOptionsUI.SetActive(true);
    }
}
