using UnityEngine;

[CreateAssetMenu(fileName = "Settings Object", menuName = "Settings")]
public class SettingsSO : ScriptableObject
{
    // Audios
    public float masterVol;
    public float musicVol;
    public float playerVol;
    public float sfxVol;

    [Seperator]
    // Other
    //public float fieldOfView;
    public float mouseSensitivity;
}
