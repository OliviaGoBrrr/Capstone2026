using Unity.Cinemachine;
using UnityEngine;

public class Settings : MonoBehaviour
{

    [Header("Objects")]
    public CinemachineCamera cam;

    [Header("UI Objects")]
    [SerializeField] public UnityEngine.UI.Slider FOVslider;

    public void updateFOV()
    {
        cam.Lens.FieldOfView = FOVslider.value;
    }

}
