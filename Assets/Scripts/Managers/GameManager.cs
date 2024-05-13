using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Transform startingCheckpoint;
    public Vector3 StartPoint => startingCheckpoint.position;

    private void Awake()
    {
        Instance = this;
    }
}
