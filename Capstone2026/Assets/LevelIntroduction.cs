using System;
using Unity.Cinemachine;
using UnityEngine;


public class LevelIntroduction : MonoBehaviour
{
    public CinemachineCamera playerCamera;
    /// <summary>
    /// Scene's Cinemachine Brain
    /// </summary>
    public CinemachineBrain camBrain;

    /// <summary>
    /// Cameras are chosen in array order. Ensure cameras are placed in order
    /// </summary>
    public CinemachineCamera[] cameras;

    [Range(0f, 2f)]
    public float defaultCamSpeed = 1.0f;

    [Range(0f, 5f)]
    public float[] camSpeeds;

    int currentCamIndex = 0;
    private CinemachineCamera currentCam;
    private CinemachineSplineDolly currentSpline;
    float splineMaxKnots;

    bool doIntroSequence = false;

    private void Start()
    {
        StartIntro();
    }

    private void Update()
    {
        if (doIntroSequence)
        {
            IntroSequence();
        }
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

        for(int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        playerCamera.gameObject.SetActive(false);

        // Allocate current camera and reset position
        currentCam = cameras[0];
        currentSpline = currentCam.GetComponent<CinemachineSplineDolly>();
        splineMaxKnots = currentSpline.Spline.Splines[0].Count - 1;
        currentSpline.CameraPosition = 0f;

        // Turn on current cam
        currentCam.gameObject.SetActive(true);

        doIntroSequence = true;
    }

    public void IntroSequence()
    {
        // If there is no current cam (for some reason), reset so the player's camera is on and that the intro sequence finishes
        if (currentCam == null) { playerCamera.gameObject.SetActive(true); doIntroSequence = false; return; }

        float currSplinePos = currentSpline.CameraPosition;

        if (currentSpline.CameraPosition < splineMaxKnots - 0.1f)
        {
            // Set the camera's speed
            float camSpeed;

            if(currentCamIndex > camSpeeds.Length - 1) { camSpeed = defaultCamSpeed; }
            
            else{ camSpeed = camSpeeds[currentCamIndex]; }

            // Lerp Camera Position
            float newSplinePos = currSplinePos + ((splineMaxKnots - currSplinePos) * Time.deltaTime * camSpeed);
            currentSpline.CameraPosition = newSplinePos;
        }
        else
        {
            // Go to next camera in the list
            currentCamIndex++;

            currentCam.gameObject.SetActive(false);

            // If there are no more cameras left in the count, cancel the intro
            if(currentCamIndex > cameras.Length - 1)
            {
                CancelIntroSequence();
                return;
            }

            // Ensure that the next camera is selected
            currentCam = cameras[currentCamIndex];
            currentCam.CancelDamping(true);

            // Find the spline and allocated 
            currentSpline = currentCam.GetComponent<CinemachineSplineDolly>();
            splineMaxKnots = currentSpline.Spline.Splines[0].Count - 1;
            currentSpline.CameraPosition = 0f;

            // Teleport brain straight to camera and snap to location
            currentCam.ForceCameraPosition(currentCam.transform.position, currentCam.transform.rotation);
            currentCam.CancelDamping(false);
            currentCam.gameObject.SetActive(true);

            Debug.Log($"Switch Cameras for Intro Sequence - Cam:{currentCam.name}, Start Pos:{currentSpline.CameraPosition}, End Pos:{splineMaxKnots}");
        }
    }

    public void CancelIntroSequence()
    {
        // Fade out

        // Once fade is done, make player cam live, and turn off all other cams\

        for( int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
            cameras[i] = null;
        }

        playerCamera.gameObject.SetActive(true);

        doIntroSequence = false;

        currentCam = null;
        currentSpline = null;

        Debug.Log("Cancelling Intro Sequence");

        // Fade back in

        // Give player movement
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
