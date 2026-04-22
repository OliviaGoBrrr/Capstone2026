using UnityEngine;
using System.Collections;
public class RotationLever : Interactable
{
    public GameObject rotatedObj;
    public int rotateCount;
    Animator AnimController;

    public void Start()
    {
        AnimController = rotatedObj.GetComponent<Animator>();
    }

    public override void onInteract()
    {
        AnimController.Play("rotatePrism");
        rotateCount++;
    }
}
