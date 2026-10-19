using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class demoEnd : MonoBehaviour
{
    [Header("DEMO")]
    [SerializeField] public GameObject EndOfDemoCanvas;
    [SerializeField] public PlayerManager playerM;
    [SerializeField] public InputActionAsset playerInputMap;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EndOfDemoCanvas.SetActive(true);

            Time.timeScale = 0;
            playerInputMap.FindActionMap("Player").Disable();
            ShowCursor();
        }
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
