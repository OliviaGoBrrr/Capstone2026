using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject door;
    [SerializeField] private Vector3 changeToDoorPosition;

    private void OnTriggerEnter(Collider other)
    {
        door.transform.position += changeToDoorPosition;
    }

    private void OnTriggerExit(Collider other)
    {
        door.transform.position -= changeToDoorPosition;
    }
}
