using UnityEngine;
using System.Collections;
public class RotationLever : /*MonoBehaviour, IInteractable,*/ Interactable
{
    Animator AnimController;
    public bool isMoving;

    public void Start()
    {
        AnimController = GetComponent<Animator>();
    }

    //void IInteractable.OnInteract()
    //{
    //    AnimController.Play("rotatePrism");
    //}

    public override void onInteract()
    {
        if (!isMoving) AnimController.Play("rotatePrism");
    }
}

//if rotating, do not allow input - OnStateEnter
//if not rotating, allow input - OnStateExit