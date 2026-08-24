using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject door;
    [SerializeField] private Vector3 changeToDoorPosition;

    [SerializeField] private bool oneShotDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (oneShotDoor)
        {
            Destroy(door);
            Destroy(this);

            print("AAA");
        }
        door.transform.position += changeToDoorPosition;
    }

    private void OnTriggerExit(Collider other)
    {
        door.transform.position -= changeToDoorPosition;
    }
}
