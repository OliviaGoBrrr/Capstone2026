using Unity.VisualScripting;
using UnityEngine;

public class QuestItem : MonoBehaviour, IInteractable
{
    [SerializeField] public int invArrayNum; 
    public PlayerManager playerM;

    public void OnInteract()
    {
        Debug.Log("Hello!");
        PlayerManager.invArray[invArrayNum] = true;
        Destroy(gameObject);
    }
}
