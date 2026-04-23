using Unity.VisualScripting;
using UnityEngine;

public class QuestItem : Interactable
{
    public NPC AssignedNPC;

    public override void onInteract()
    {
        // add thing in here where u can't interact if quest hasn't been assigned
        AssignedNPC.QuestAchieved = true;
        Destroy(gameObject);
    }
}
