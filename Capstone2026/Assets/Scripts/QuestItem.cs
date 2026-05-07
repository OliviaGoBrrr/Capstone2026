using Unity.VisualScripting;
using UnityEngine;

public class QuestItem : MonoBehaviour, IInteractable
{
    public NPC AssignedNPC;

    public void OnInteract()
    {
        // add thing in here where u can't interact if quest hasn't been assigned
        AssignedNPC.QuestAchieved = true;
        Destroy(gameObject);
    }
}
