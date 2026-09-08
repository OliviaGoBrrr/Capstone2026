using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CameraSequencer : MonoBehaviour
{
    #region Sequence Setting Variables
    [Header("Sequence Variables")]
    [Tooltip("Does sequence play when scene loads in")]
    [SerializeField]
    private bool seqPlayOnStart = false;

    [Tooltip("If true, will move to next camera in sequence once it reaches the end of the spline, without player input")]
    [SerializeField]
    private bool seqPlayAuto = false;

    [Tooltip("If true, player input will go to next camera in sequence")]
    [SerializeField]
    private bool seqSkippable = false;

    [Tooltip("If true, player input will stop the entire sequence")]
    [SerializeField]
    private bool seqCancellable = false;
    #endregion

    [Header("Cameras")]
    [Tooltip("Player's Camera - Necessary to reset after intro sequence concludes")]
    [SerializeField]
    private CinemachineCamera playerCamera;

    /// <summary>
    /// Scene's Cinemachine Brain
    /// </summary>
    [Tooltip("Scene's Cinemachine Brain (Will try to allocate on start, if possible)")]
    [SerializeField]
    private CinemachineBrain camBrain;

    /// <summary>
    /// Cameras are chosen in array order. Ensure cameras are placed in order
    /// </summary>
    [Tooltip("Cameras will play in array order (first to last element)")]
    [SerializeField]
    private CinemachineCamera[] cameras;

    [Header("Spline Values")]
    [Range(0f, 10f)]
    public float DefaultCamSpeed = 1.0f;
    [Range(0f, 10f)]
    public float[] CamSpeeds;

    [Range(0.01f, 0.5f)]
    public float CameraOffsetBeforeSwitching = 0.1f;

    // Private Values
    private CinemachineCamera currentCam;
    private CinemachineSplineDolly currentSpline;

    // Check to see if whether or not we can do intro sequence

    // true for skippable or false for cancellable
    bool skipCheck = false;
    bool playSequence = false;
    int currentCamIndex = 0;
    float splineMaxDistance = 0f;

    #region Sequence Events

    [HideInInspector]
    public UnityEvent StartSequenceEvent = new();
    [HideInInspector]
    public UnityEvent NextCamSequenceEvent = new();
    [HideInInspector]
    public UnityEvent EndSequenceEvent = new();

    #endregion

    private void Start()
    {
        if (seqPlayOnStart)
        {
            StartSequence();
        }
        else
        {
            for (int i = 0; i < cameras.Length; i++)
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

    private void OnValidate()
    {
        // Probs not the best code
        // Makes it so designers can't have both cancelling and skipping on the camera sequence
        if (skipCheck)
        {
            if (seqCancellable)
            {
                seqSkippable = false;
                skipCheck = false;
            }
        }
        else if (!skipCheck)
        {
            if (seqSkippable)
            {
                seqCancellable = false;
                skipCheck = true;
            }

        }
    }

    #region Coroutines

    /*
    private IEnumerator LetterboxFadeIn
    {
        // Fade in bars on top of bottom

        // 

    }


    private IEnumerator LetterboxFadeOut
    {

    }
    */
    #endregion



    #region Camera Sequence Methods
    public virtual void StartSequence()
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

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        playerCamera.gameObject.SetActive(false);

        // Allocate current camera and reset position
        currentCam = cameras[0];
        currentSpline = currentCam.GetComponent<CinemachineSplineDolly>();
        splineMaxDistance = currentSpline.Spline.Spline.GetLength();
        currentSpline.CameraPosition = 0f;

        // Turn on current cam
        currentCam.gameObject.SetActive(true);

        //StartSequenceEvent.Invoke();

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
        if (Input.anyKeyDown)
        {
            if (seqCancellable)
            {
                CancelCameraSequence();
                return;
            }
            else if (seqSkippable)
            {
                NextCameraInSequence();
                return;
            }
        }

        float currSplinePos = currentSpline.CameraPosition;

        if (currentSpline.CameraPosition < splineMaxDistance - (splineMaxDistance * CameraOffsetBeforeSwitching))
        {
            // Set the camera's speed
            float camSpeed;

            if (currentCamIndex > CamSpeeds.Length - 1) { camSpeed = DefaultCamSpeed; }

            else { camSpeed = CamSpeeds[currentCamIndex]; }

            // Lerp Camera Position
            float newSplinePos = currSplinePos + ((splineMaxDistance - currSplinePos) * Time.deltaTime * camSpeed);
            currentSpline.CameraPosition = newSplinePos;
        }
        else
        {
            if (seqPlayAuto || Input.anyKeyDown)  // || player presses key to go to next sequence)
            {
                NextCameraInSequence();
                //dsdsdsd
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
        splineMaxDistance = currentSpline.Spline.Spline.GetLength();
        currentSpline.CameraPosition = 0f;

        // Teleport brain straight to camera and snap to location
        currentCam.ForceCameraPosition(currentCam.transform.position, currentCam.transform.rotation);
        currentCam.CancelDamping(false);
        currentCam.gameObject.SetActive(true);

        //NextCamSequenceEvent.Invoke();

        Debug.Log($"Switch Cameras for Intro Sequence - Cam:{currentCam.name}, Start Pos:{currentSpline.CameraPosition}, End Pos:{splineMaxDistance}");
    }

    /// <summary>
    /// Cancels the current sequence and returns the camera back to the player camera
    /// </summary>
    public virtual void CancelCameraSequence()
    {
        // Fade out

        // Once fade is done, make player cam live, and turn off all other cams\

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        playerCamera.gameObject.SetActive(true);

        playSequence = false;

        currentCam = null;
        currentSpline = null;

        Debug.Log("Cancelling Intro Sequence");

        //EndSequenceEvent.Invoke();
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
        if (playerCamera == null) { throw new ArgumentNullException(paramName: this.gameObject.name, message: "No Player Camera Allocated to LevelIntro"); }

        // If there is no CineMachine Brain allocated
        if (camBrain == null)
        {
            // Try to find one in the scene (should only be 1)
            camBrain = FindFirstObjectByType<CinemachineBrain>();

            // If it still can't find it
            if (camBrain == null)
            {
                throw new ArgumentNullException(paramName: this.gameObject.name, message: "No CineMachine Brain Found in Scene - Please Create a CineMachine Brain");
            }
        }
    }

    #endregion


}
