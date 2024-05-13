using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Collider coll;
    [SerializeField] private GameObject mesh;

    private void OnTriggerEnter(Collider other)
    {
        ICheckpoint check = other.GetComponent<ICheckpoint>();

        if (check != null)
        {
            check.UpdateCheckpoint(this, transform);
            coll.enabled = false;
            mesh.SetActive(false);
        }
    }

    public void TurnOnCheckpoint()
    {
        coll.enabled = true;
        mesh.SetActive(true);
    }
}
