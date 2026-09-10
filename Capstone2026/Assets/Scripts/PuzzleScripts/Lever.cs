using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public Animator DoorAnim;
    public Animator LeverAnim;

    public void OnInteract()
    {
        DoorAnim.SetBool("LeverTrigger", true);
        LeverAnim.SetBool("doorOpen", true);
    }

    public void ActivateOutline() { }

    public void DeactivateOutline() { }
}
