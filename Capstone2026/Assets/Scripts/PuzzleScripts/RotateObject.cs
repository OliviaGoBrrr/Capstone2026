using UnityEngine;
using DG.Tweening;

public class YRotateObject : MonoBehaviour, IInteractable
{
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
        print("AAA");
        objectToBeRotated.transform.Rotate(0, rotationStep, 0, Space.World);

        if (totalNumberOfRotations == currentNumberOfRotations)
        {
            objectToBeRotated.transform.eulerAngles = new Vector3(objectToBeRotated.transform.eulerAngles.x, originalRotation, objectToBeRotated.transform.eulerAngles.z);
        }

        currentNumberOfRotations += 1;
    }
}
