using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class OptionHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Outline backgroundOutline;

    [SerializeField] private InputActionReference pauseAction;

    void Start()
    {
        backgroundOutline.enabled = false;
    }

    void OnEnable()
    {
        backgroundOutline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print(eventData.hovered);
        backgroundOutline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundOutline.enabled = false;
    }

}
