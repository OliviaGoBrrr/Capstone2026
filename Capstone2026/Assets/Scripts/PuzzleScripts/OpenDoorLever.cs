using UnityEngine;
using DG.Tweening;
using UnityEngine.ProBuilder.Shapes;
using Unity.VisualScripting;

public class OpenDoorLever : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 changeToDoorPosition;
    private Vector3 originalPosition;

    [SerializeField] private float timeToMove;

    [SerializeField] private GameObject door;

    [SerializeField] private GameObject leverHandle;
    [SerializeField] private GameObject leverBase;

    [SerializeField] private bool isOneTime = true;

    private bool spent = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = door.transform.position;
    }

    public void OnInteract()
    {
        if (spent) return;

        door.transform.DOMove(originalPosition + changeToDoorPosition, timeToMove);

        leverHandle.transform.DOLocalRotate(new Vector3(-120, 0, 0), 0.25f);

        if (isOneTime)
        {
            spent = true;

            leverHandle.layer = 0;
            leverBase.layer = 0;
        }
    }

    public void ActivateOutline()
    {
        if (spent) return;
        leverHandle.layer = 30;
        leverBase.layer = 30;
    }

    public void DeactivateOutline()
    {
        leverHandle.layer = 0;
        leverBase.layer = 0;
    }
}
