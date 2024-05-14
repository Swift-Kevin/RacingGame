using UnityEngine;
using UnityEngine.UI;

public class NetworkingUI : MonoBehaviour
{
    // LAN
    [SerializeField] private Button btn_JoinLAN;
    [SerializeField] private Button btn_HostLAN;
    // WAN
    [SerializeField] private Button btn_JoinWAN;
    [SerializeField] private Button btn_HostWAN;

    private void Start()
    {
        // Quick Lobby
        btn_HostWAN.onClick.AddListener(QuickHostButtonFunctionality);
        btn_JoinWAN.onClick.AddListener(QuickJoinButtonFunctionality);

        // LAN Buttons
        btn_HostLAN.onClick.AddListener(LANHostButtonFunctionality);
        btn_JoinLAN.onClick.AddListener(LANJoinButtonFunctionality);
    }

    private async void LANHostButtonFunctionality()
    {
        await ConnectionManager.Instance.LANHostLobby();

        InGameHideUI();
    }

    private async void LANJoinButtonFunctionality()
    {
        await ConnectionManager.Instance.LANJoinLobby();

        InGameHideUI();
    }

    private async void QuickHostButtonFunctionality()
    {
        await ConnectionManager.Instance.CreateLobby("MyLobby", false);

        InGameHideUI();
    }

    private async void QuickJoinButtonFunctionality()
    {
        await ConnectionManager.Instance.QuickJoinLobby();
        InGameHideUI();
    }

    private void InGameHideUI()
    {
        UIManager.Instance.HideAllMenus();
        UIManager.Instance.SetIsInGame(true);
    }

    public void DisableButtons()
    {
        btn_JoinLAN.interactable = false;
        btn_HostLAN.interactable = false;
        btn_JoinWAN.interactable = false;
        btn_HostWAN.interactable = false;
    }

    public void EnableButtons()
    {
        btn_JoinLAN.interactable = true;
        btn_HostLAN.interactable = true;
        btn_JoinWAN.interactable = true;
        btn_HostWAN.interactable = true;
    }



}
