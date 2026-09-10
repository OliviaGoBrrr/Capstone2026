using UnityEngine;
using System.Collections;
public class RotationLever : MonoBehaviour, IInteractable
{
    Animator AnimController;
    public bool isMoving;
    public PlayerManager playerM;
    public AudioClip clip;

    private void Awake()
    {
        playerM = FindFirstObjectByType<PlayerManager>().GetComponent<PlayerManager>(); 
    }

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
        PostGameDataLog.prismInteractInt++;
        if (!isMoving && playerM.isPushPulling != true) 
        {
            AudioManager.Instance.PlaySFX(clip, transform, 0.25f);
            AnimController.Play("rotatePrism");
        }
    }
    public void ActivateOutline() { }

    public void DeactivateOutline() { }
}

//if rotating, do not allow input - OnStateEnter
//if not rotating, allow input - OnStateExit