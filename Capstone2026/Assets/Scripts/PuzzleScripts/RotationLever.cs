using UnityEngine;
using System.Collections;
public class RotationLever : MonoBehaviour, IInteractable
{
    Animator AnimController;
    public bool isMoving;
    public PlayerManager playerM;

    public void Start()
    {
        AnimController = GetComponent<Animator>();
    }

    //void IInteractable.OnInteract()
    //{
    //    AnimController.Play("rotatePrism");
    //}s

    public void OnInteract()
    {
        if (!isMoving && playerM.isPushPulling != true) AnimController.Play("rotatePrism");
    }
}

//if rotating, do not allow input - OnStateEnter
//if not rotating, allow input - OnStateExit