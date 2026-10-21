using Unity.VisualScripting;
using UnityEngine;

public class QuestItem : MonoBehaviour, IInteractable
{
    public GameObject cabbage;
    public GameManager gameManager;

    public void OnInteract()
    {
        gameManager.pickedUpCabbages = true;
        gameObject.SetActive(false);
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
