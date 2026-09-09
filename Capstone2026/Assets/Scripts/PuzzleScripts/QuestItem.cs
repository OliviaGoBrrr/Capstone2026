using Unity.VisualScripting;
using UnityEngine;

public class QuestItem : MonoBehaviour, IInteractable
{
    [SerializeField] public int invArrayNum; 
    public PlayerManager playerM;

    public void OnInteract()
    {
        playerM.invArray[invArrayNum] = true;
        Destroy(gameObject);
    }
}
