using Unity.VisualScripting;
using UnityEngine;

public class QuestItem : MonoBehaviour, IInteractable
{
    [SerializeField] public int invArrayNum; 
    public GameObject cabbage;
    public PlayerManager playerM;
    public NPC Phinn;

    public void OnInteract()
    {
        PlayerManager.invArray[invArrayNum] = true; // remove
        NPC.Phinn.QuestAchieved = true; // should work
        Destroy(gameObject);
    }

    public void ActivateOutline() 
    {
        cabbage.layer = 30;
    }

    public void DeactivateOutline()
    {
        cabbage.layer = 0;
    }
}
