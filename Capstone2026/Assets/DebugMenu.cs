using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference debugMenuAction;
    [SerializeField] private GameObject debugMenuGameObject;
    [SerializeField] private GameObject locationUIElement;
    [SerializeField] private GameObject[] teleportLocations;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        PopulateDebugButtons();
        debugMenuGameObject.SetActive(false);
    }

    private void Update()
    {
        if (debugMenuAction.action.WasPressedThisFrame())
        {
            DebugMenuToggle();
        }
    }

    private void DebugMenuToggle()
    {
        debugMenuGameObject.SetActive(!debugMenuGameObject.activeSelf);
    }

    private void PopulateDebugButtons()
    {
        if(teleportLocations.Length > 0)
        {
            for(int i = 0; i < teleportLocations.Length; i++)
            {
                Button newButton = Instantiate(buttonPrefab, locationUIElement.transform); ;
                GameObject teleportData = teleportLocations[i];

                newButton.GetComponentInChildren<TMP_Text>().SetText($"{teleportData.name}");
                newButton.onClick.AddListener(() => playerManager.HandleTeleport(teleportData.transform.position));
            }
        }
    }
}
