using Unity.VisualScripting;
using UnityEngine;

public class QuestItem : MonoBehaviour, IInteractable
{
    [SerializeField] public int invArrayNum; 
    public PlayerManager playerM;
    public NPC Phinn;

    public void OnInteract()
    {
        Debug.Log("Hello!");
        PlayerManager.invArray[invArrayNum] = true; // remove
        NPC.Phinn.QuestAchieved = true; // should work
        Destroy(gameObject);
    }

    public void ActivateOutline() { }

    public void DeactivateOutline() { }
}
