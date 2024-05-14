using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Transform startingCheckpoint;
    public Vector3 StartPoint => startingCheckpoint.position;

    public void QuitApp()
    {
        Application.Quit();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void MouseUnlockShow()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void MouseLockHide()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReturnToMainMenu()
    {
        DisconnectPlayer(); // disconnect from the relay
        MouseUnlockShow(); // show cursor and unlock it from center
        UIManager.Instance.SetIsInGame(false); // disable turning on pause menu
        UIManager.Instance.DisplayMultiplayerMenu(); 
    }

    public void DisconnectPlayer()
    {
        if (!NetworkManager.Singleton.ShutdownInProgress)
        {
            ConnectionManager.Instance.LeaveLobby();
        }
        NetworkManager.Singleton.Shutdown();
    }
}
