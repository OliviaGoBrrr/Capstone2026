using Unity.Cinemachine;
using UnityEngine;

//i need to actually set up this script so that changes stay across scenes and stuff but i havent yet so treat this as placeholder
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
