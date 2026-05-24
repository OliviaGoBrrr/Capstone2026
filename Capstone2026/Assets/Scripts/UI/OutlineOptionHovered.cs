using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class OutlineOptionHovered : MonoBehaviour
{
    [SerializeField] private Outline backgroundOutline = null;

    [SerializeField] private InputActionReference pauseAction;


    void Start()
    {
        backgroundOutline.enabled = false;
    }

    void OnEnable()
    {
        backgroundOutline.enabled = false;
    }

    public void MouseEntered()
    {
        backgroundOutline.enabled = true;
    }

    public void MouseExited()
    {
        backgroundOutline.enabled = false;
    }

}
