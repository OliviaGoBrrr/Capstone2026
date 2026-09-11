using UnityEngine;
using DG.Tweening;

public class YRotateObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject crystalHead;
    [SerializeField] private GameObject crystalBase;

    [SerializeField] private GameObject objectToBeRotated;
    [SerializeField] private int originalRotation;
    [SerializeField] private int rotationStep;
    [SerializeField] private int totalNumberOfRotations; // number of rotations before loop back to start
    private int currentNumberOfRotations = 0;

    // Update is called once per frame
    void Start()
    {
        objectToBeRotated.transform.eulerAngles = new Vector3(objectToBeRotated.transform.eulerAngles.x, originalRotation, objectToBeRotated.transform.eulerAngles.z);
    }

    public void OnInteract()
    {
        if (totalNumberOfRotations == currentNumberOfRotations)
        {
            objectToBeRotated.transform.DORotate(new Vector3(0, originalRotation, 0), 0.5f).SetEase(Ease.OutCubic);

            //objectToBeRotated.transform.eulerAngles = new Vector3(objectToBeRotated.transform.eulerAngles.x, originalRotation, objectToBeRotated.transform.eulerAngles.z);
            currentNumberOfRotations = 0;
            return;
        }

        currentNumberOfRotations += 1;

        objectToBeRotated.transform.DORotate(new Vector3(0, originalRotation + rotationStep * currentNumberOfRotations, 0), 0.5f).SetEase(Ease.OutCubic);

        //objectToBeRotated.transform.Rotate(0, rotationStep, 0, Space.World);
    }

    public void ActivateOutline()
    {
        if (totalNumberOfRotations == 0) return;
        crystalHead.layer = 29; // Outline100Scale
        crystalBase.layer = 29; // Outline100Scale
    }

    public void DeactivateOutline()
    {
        crystalHead.layer = 0; // default
        crystalBase.layer = 0; // default
    }
}
