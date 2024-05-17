using UnityEngine;

public class CarMaterialChanger : MonoBehaviour
{
    [SerializeField] private int index;

    private void OnTriggerEnter(Collider other)
    {
        PlayerColor playCol = other.GetComponent<PlayerColor>();

        if (playCol != null )
        {
            playCol.Triggered(index);
        }
    }
}

public interface IStation {

    public int GetIndex();

}

