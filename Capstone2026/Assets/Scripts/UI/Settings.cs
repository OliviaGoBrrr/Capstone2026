using DG.Tweening.Core.Easing;
using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//i need to actually set up this script so that changes stay across scenes and stuff but i havent yet so treat this as placeholder
public class Settings : MonoBehaviour
{

    [Header("Objects")]
    public CinemachineCamera cam;

    [Header("UI Objects")]
    [SerializeField] public UnityEngine.UI.Slider FOVslider;
    [SerializeField] public GameObject WASD, joystick, grapple1, grapple2, grapple3, grapple4;
    [SerializeField] public GameObject space, buttonSouth;
    [SerializeField] public GameObject E1, E2, E3, E4, E5, E6, E7, rightShoulder0, rightShoulder1, leftShoulder;

    public void updateFOV()
    {
        cam.Lens.FieldOfView = FOVslider.value;
    }
    public void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "_MVPFarmTutorial" && Gamepad.current != null)
        {
            space.gameObject.SetActive(true);
            E1.gameObject.SetActive(true);
            E2.gameObject.SetActive(true);
            E3.gameObject.SetActive(true); 
            E4.gameObject.SetActive(true);
            E5.gameObject.SetActive(true);
            E6.gameObject.SetActive(true);
            E7.gameObject.SetActive(true);
            WASD.gameObject.SetActive(true);

            rightShoulder0.gameObject.SetActive(false);
            rightShoulder1.gameObject.SetActive(false);
            leftShoulder.gameObject.SetActive(false);
            buttonSouth.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
            grapple1.gameObject.SetActive(false);
            grapple2.gameObject.SetActive(false);
            grapple3.gameObject.SetActive(false);
            grapple4.gameObject.SetActive(false);
        }
        else if (currentScene.name == "_MVPFarmTutorial" && Gamepad.current != null)
        {
            rightShoulder0.gameObject.SetActive(true);
            rightShoulder1.gameObject.SetActive(true);
            leftShoulder.gameObject.SetActive(true);
            buttonSouth.gameObject.SetActive(true);
            joystick.gameObject.SetActive(true);
            grapple1.gameObject.SetActive(true);
            grapple2.gameObject.SetActive(true);
            grapple3.gameObject.SetActive(true);
            grapple4.gameObject.SetActive(true);

            space.gameObject.SetActive(false);
            E1.gameObject.SetActive(false);
            E2.gameObject.SetActive(false);
            E3.gameObject.SetActive(false);
            E4.gameObject.SetActive(false);
            E5.gameObject.SetActive(false);
            E6.gameObject.SetActive(false);
            E7.gameObject.SetActive(false);
            WASD.gameObject.SetActive(false);
        }
    }
}
