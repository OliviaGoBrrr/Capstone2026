using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference debugMenuAction;
    [SerializeField] private GameObject debugMenuGameObject;
    [SerializeField] private GameObject locationUIElement;
    [SerializeField] private GameObject teleportContainer;
    private Transform[] teleportLocations;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        if(debugMenuGameObject != null)
        {
            debugMenuGameObject.SetActive(false);
            teleportLocations = teleportContainer.GetComponentsInChildren<Transform>();
            PopulateDebugButtons();
        }
    }

    private void Update()
    {
        if(debugMenuGameObject != null)
        {
            if (debugMenuAction.action.WasPressedThisFrame())
            {
                DebugMenuToggle();
            }
        }
    }

    private void DebugMenuToggle()
    {
        debugMenuGameObject.SetActive(!debugMenuGameObject.activeSelf);
    }

    private void PopulateDebugButtons()
    {
        // Skips the first entry, as the the parent is considered in .GetComponentsInChildren<>();
        if(teleportLocations.Length > 1)
        {
            for(int i = 1; i < teleportLocations.Length; i++)
            {
                Button newButton = Instantiate(buttonPrefab, locationUIElement.transform); ;
                Transform teleportData = teleportLocations[i];

                newButton.GetComponentInChildren<TMP_Text>().SetText($"{teleportData.name}");
                newButton.onClick.AddListener(() => playerManager.HandleTeleport(teleportData.transform.position));
            }
        }
    }
}
