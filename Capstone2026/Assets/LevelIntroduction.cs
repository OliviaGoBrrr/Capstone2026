using System;
using Unity.Cinemachine;
using UnityEngine;


public class LevelIntroduction : MonoBehaviour
{
    /// <summary>
    /// Scene's Cinemachine Brain
    /// </summary>
    public CinemachineBrain camBrain;

    /// <summary>
    /// Cameras are chosen in array order. Ensure cameras are placed in order
    /// </summary>
    public CinemachineCamera playerCamera;
    public CinemachineCamera[] cameras;


    private void Start()
    {
        StartIntro();
    }

    public void StartIntro()
    {
        // Check for errors
        try
        {
            IntroErrorCheck();
        }
        catch (Exception e)
        {

            Debug.LogError($"Intro Failed - Error: {e}");
            return;
        }

        Debug.Log("Level Introduction Started");

        // PSEUDO CODE

        // Make first camera in queue the Live Camera

        // Once camera is Live, fade and wipe into first camera
        
        // Start moving along dolly over time allocated

        // Once camera has gotten to last point (or within certain range of last point), make next camera live and start moving along dolly

        // Repeat until last camera

        // If camera is last in queue and reaches final destination, or the player presses a button to cancel intro, fade and wipe back, and make player camera live

    }

    /// <summary>
    /// Checks for errors before running level intro camera fly-through
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public void IntroErrorCheck()
    {
        // If there is no fly through introduction set up, stop function
        if (cameras.Length == 0) { throw new ArgumentException(message: "No Cameras Allocated to Level Intro"); }

        // If there is no playerCamera allocated
        if (playerCamera == null) { throw new ArgumentNullException(paramName:playerCamera.Name,message: "No Player Camera Allocated to LevelIntro"); }
    }
}
