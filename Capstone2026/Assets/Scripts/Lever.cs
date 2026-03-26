using UnityEngine;

public class Lever : Interactable
{
    public Animator DoorAnim;
    public Animator LeverAnim;

    public override void onInteract()
    {
        DoorAnim.SetBool("LeverTrigger", true);
        LeverAnim.SetBool("doorOpen", true);
    }
}
