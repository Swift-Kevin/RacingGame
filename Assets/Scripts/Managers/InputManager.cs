using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private PlayerInputs playerInputs;

    public PlayerInputs.GeneralActions Actions => playerInputs.General;

    public Vector2 MoveVec => Actions.Movement.ReadValue<Vector2>();
    public Vector2 LookVec => Actions.Looking.ReadValue<Vector2>();
    public bool IsBreaking => Actions.Breaking.IsPressed();

    private void Awake()
    {
        Instance = this;
        playerInputs = new PlayerInputs();
        playerInputs.Enable();
    }

    private void Start()
    {
        playerInputs.UI.PauseToggle.performed += PauseToggle_performed;
    }

    private void PauseToggle_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (UIManager.Instance.isInGame)
        {
            UIManager.Instance.TogglePauseMenu();
            Debug.Log("toggled");
        }
    }
}
