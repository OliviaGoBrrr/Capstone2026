using UnityEngine;
using DG.Tweening;

public class YRotateObject : MonoBehaviour, IInteractable
{
    [SerializeField] private int originalRotation;
    [SerializeField] private int rotationStep;
    [SerializeField] private int totalNumberOfRotations; // number of rotations before loop back to start
    private int currentNumberOfRotations = 0;

    // Update is called once per frame
    void Start()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, originalRotation, transform.eulerAngles.z);
    }

    void IInteractable.OnInteract()
    {
        print("AAA");
        transform.Rotate(0, rotationStep, 0, Space.Self);

        if (totalNumberOfRotations == currentNumberOfRotations)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, originalRotation, transform.eulerAngles.z);
        }

        currentNumberOfRotations += 1;
    }
}
