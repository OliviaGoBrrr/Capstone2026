using UnityEngine;
using DG.Tweening;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject door;
    [SerializeField] private Vector3 changeToDoorPosition;

    [SerializeField] private bool oneShotDoor;

    [SerializeField] private float timeToMove = 1;

    private Vector3 originalPosition;


    private void Start()
    {
        originalPosition = door.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (oneShotDoor)
        {
            Destroy(door);
            Destroy(this);
        }
        else
        {
            DOTween.Kill("moving door");
            door.transform.DOMove(originalPosition + changeToDoorPosition, timeToMove).SetId("moving door");

            //door.transform.position += changeToDoorPosition;
        }

            
    }

    private void OnTriggerExit(Collider other)
    {
        DOTween.Kill("moving door");
        door.transform.DOMove(originalPosition, timeToMove).SetId("moving door");
    }
}
