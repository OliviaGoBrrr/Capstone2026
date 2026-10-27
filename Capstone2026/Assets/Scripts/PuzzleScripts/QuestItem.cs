using Unity.VisualScripting;
using UnityEngine;

public class QuestItem : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isMotor = false;

    public GameObject questItem;
    public GameManager gameManager;

    public void OnInteract()
    {
        print("AAAAA");
        gameManager.pickedUpCabbages = true;
        gameObject.SetActive(false);
    }

    public void ActivateOutline() 
    {
        if (isMotor) questItem.layer = 29;
        else questItem.layer = 30;
    }

    public void DeactivateOutline()
    {
        questItem.layer = 0;
    }
}
