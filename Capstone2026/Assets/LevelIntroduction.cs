using System;
using Unity.Cinemachine;
using UnityEngine;


public class LevelIntroduction : MonoBehaviour
{
    [Header("Sequence Variables")]
    [Tooltip("Does sequence play when scene loads in")]
    public bool seqPlayOnStart = false;

    [Tooltip("Does the sequence play automatically")]
    public bool seqPlayAuto = false;

    [Tooltip("Can the player skip camera sequence")]
    public bool seqSkippable = false;

    [Header("Cameras")]
    [Tooltip("Player's Camera - Necessary to reset after intro sequence concludes")]
    public CinemachineCamera playerCamera;

    /// <summary>
    /// Scene's Cinemachine Brain
    /// </summary>
    [Tooltip("Scene's Cinemachine Brain (Will try to allocate on start, if possible)")]
    public CinemachineBrain camBrain;

    /// <summary>
    /// Cameras are chosen in array order. Ensure cameras are placed in order
    /// </summary>
    [Tooltip("Cameras will play in array order (first to last element)")]
    public CinemachineCamera[] cameras;

    [Header("Spline Values")]
    [Range(0f, 10f)]
    public float defaultCamSpeed = 1.0f;
    [Range(0f, 10f)]
    public float[] camSpeeds;

    [Range(0.01f, 0.5f)]
    public float cameraOffsetBeforeSwitching = 0.1f;

    // Private Values
    private CinemachineCamera currentCam;
    private CinemachineSplineDolly currentSpline;

    // Check to see if whether or not we can do intro sequence
    bool playSequence = false;
    int currentCamIndex = 0;
    float splineMaxKnots;



    private void Start()
    {
        if (seqPlayOnStart)
        {
            StartSequence();
        }
        else
        {
            for(int i = 0; i < cameras.Length; i++)
            {
                cameras[i].gameObject.SetActive(false);
            }
            playerCamera.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (playSequence)
        {
            PlayCameraSequence();
        }
    }

    public void StartSequence()
    {
        // Check for errors
        try
        {
            SequenceErrorCheck();
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

        playSequence = true;
    }

    /// <summary>
    /// Plays the camera sequence, moving between 
    /// </summary>
    public void PlayCameraSequence()
    {
        // If there is no current cam (for some reason), reset so the player's camera is on and that the intro sequence finishes
        if (currentCam == null) { playerCamera.gameObject.SetActive(true); playSequence = false; return; }

        // If the play can just skip to the next camera
        if (seqSkippable && Input.anyKeyDown)
        {
            NextCameraInSequence();
            return;
        }

        float currSplinePos = currentSpline.CameraPosition;

        if (currentSpline.CameraPosition < splineMaxKnots - cameraOffsetBeforeSwitching)
        {
            // Set the camera's speed
            float camSpeed;

            if (currentCamIndex > camSpeeds.Length - 1) { camSpeed = defaultCamSpeed; }

            else { camSpeed = camSpeeds[currentCamIndex]; }

            // Lerp Camera Position
            float newSplinePos = currSplinePos + ((splineMaxKnots - currSplinePos) * Time.deltaTime * camSpeed);
            currentSpline.CameraPosition = newSplinePos;
        }
        else
        {
            if (seqPlayAuto || Input.anyKeyDown)  // || player presses key to go to next sequence)
            {
                NextCameraInSequence();
            }
        }
    }

    public void NextCameraInSequence()
    {
        // Go to next camera in the list
        currentCamIndex++;

        currentCam.gameObject.SetActive(false);

        // If there are no more cameras left in the count, cancel the intro
        if (currentCamIndex > cameras.Length - 1)
        {
            CancelCameraSequence();
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

    public void CancelCameraSequence()
    {
        // Fade out

        // Once fade is done, make player cam live, and turn off all other cams\

        for( int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
            cameras[i] = null;
        }

        playerCamera.gameObject.SetActive(true);

        playSequence = false;

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
    public void SequenceErrorCheck()
    {
        // If there is no fly through introduction set up, stop function
        if (cameras.Length == 0) { throw new ArgumentException(message: "No Cameras Allocated to Level Intro"); }

        // If there is no playerCamera allocated
        if (playerCamera == null) { throw new ArgumentNullException(paramName:this.gameObject.name,message: "No Player Camera Allocated to LevelIntro"); }

        // If there is no CineMachine Brain allocated
        if(camBrain == null)
        {
            // Try to find one in the scene (should only be 1)
            camBrain = FindFirstObjectByType<CinemachineBrain>();

            // If it still can't find it
            if(camBrain == null)
            {
                throw new ArgumentNullException(paramName: this.gameObject.name, message: "No CineMachine Brain Found in Scene - Please Create a CineMachine Brain");
            }
        }
    }
}
