using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CameraSequencer : MonoBehaviour
{
    #region Sequence Setting Variables
    [Header("Sequence Variables")]

    [Tooltip("Input key for next camera/skip")]
    [SerializeField]
    protected InputActionReference nextCameraAction;

    [Tooltip("Hide Player UI during Camera Sequence")]
    [SerializeField]
    protected bool seqHidePlayerUI = false;

    [Tooltip("Will require a box collider with the IsTrigger flag set to true and a rigidbody to work")]
    [SerializeField]
    protected bool seqPlayOnTrigger = false;
    
    [SerializeField]
    protected BoxCollider seqTriggerCollider;
    protected bool seqPlayed = false; 

    [Tooltip("Does sequence play when scene loads in")]
    [SerializeField]
    protected bool seqPlayOnStart = false;

    [Tooltip("If true, will move to next camera in sequence once it reaches the end of the spline, without player input")]
    [SerializeField]
    protected bool seqPlayAuto = false;

    [Tooltip("Allows the sequence to be played multiple times - pair with PlayOnTrigger to happen each time the player sets the trigger condiition")]
    [SerializeField]
    protected bool seqPlayMultipleTimes = false;

    [Tooltip("If true, player input will go to next camera in sequence * Note: Does not work with cameras that use the knot position unit type")]
    [SerializeField]
    protected bool seqSkippable = false;

    [Tooltip("If true, player input will stop the entire sequence")]
    [SerializeField]
    protected bool seqCancellable = false;

    [Tooltip("Determines how the camera will move to the next camera in the sequence (i.e. Cut = instantly)")]
    [SerializeField]
    protected CinemachineBlendDefinition seqTransitionToNextCamType;
    #endregion

    #region Private Variables
    [Header("Cameras")]
    [Tooltip("Player's Camera - Necessary to reset after intro sequence concludes")]
    [SerializeField]
    protected CinemachineCamera playerCamera;

    /// <summary>
    /// Scene's Cinemachine Brain
    /// </summary>
    [Tooltip("Scene's Cinemachine Brain (Will try to allocate on start, if possible)")]
    [SerializeField]
    protected CinemachineBrain camBrain;

    /// <summary>
    /// Cameras are chosen in array order. Ensure cameras are placed in order
    /// </summary>
    [Tooltip("Cameras will play in array order (first to last element)")]
    [SerializeField]
    protected CinemachineCamera[] cameras;
    protected CinemachineCamera currentCam;
    protected CinemachineSplineDolly currentSpline;
    protected CinemachineBlendDefinition camBrainDefaultBlend;
    protected UnityEngine.Splines.PathIndexUnit camPositionUnits;

    // Player Manager Reference

    protected PlayerManager playerManager;

    // Check to see if whether or not we can do intro sequence
    // true for skippable or false for cancellable
    bool skipCheck = false;
    protected bool playSequence = false;
    int currentCamIndex = 0;
    float splineMaxDistance = 0f;
    
    [SerializeField]
    private float splineMaxKnots = 0f;
    [SerializeField]
    private float splineTargetKnot;

    #endregion

    #region Public Variables
    [Header("Spline Values")]
    [Range(0f, 10f)]
    public float DefaultCamSpeed = 1.0f;
    [Range(0f, 10f)]
    public float[] CamSpeeds;

    [Range(0.01f, 0.5f)]
    public float CameraOffsetBeforeSwitching = 0.1f;
    #endregion

    #region Sequence Events

    [HideInInspector]
    public UnityEvent StartSequenceEvent = new();
    [HideInInspector]
    public UnityEvent NextCamSequenceEvent = new();
    [HideInInspector]
    public UnityEvent EndSequenceEvent = new();

    #endregion

    #region Unity Methods
    private void Start()
    {
        // Check for errors
        try
        {
            SequenceErrorCheck();
        }
        catch (Exception e)
        {
            // If theres any errors caught, will throw a message and cancel all other start functions for the camera sequence
            Debug.LogError($"({this.name}) Sequence Failed To Load - Error: {e}");
            return;
        }


        // Turn off all sequence cameras - ensures that sequences don't start/the wrong camera is live on start
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        // Store how the brain transitions between cameras by default
        camBrainDefaultBlend = camBrain.DefaultBlend;

        // Checks to see if we auto play on scene start
        if (seqPlayOnStart)
        {
            StartSequence();
        }
        else
        {
            playerCamera.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (playSequence)
        {
            PlayCameraSequence();

            // Depending on how the camera calculates its position, will change how the camera moves
            switch (currentSpline.PositionUnits)
            {
                case (UnityEngine.Splines.PathIndexUnit.Distance):
                    MoveCameraAlongSpline();
                    break;

                case (UnityEngine.Splines.PathIndexUnit.Knot):
                    MoveCameraToNextKnot();
                    break;

                case (UnityEngine.Splines.PathIndexUnit.Normalized):
                    MoveCameraAlongSplineNormalized();
                    break;

                default:
                    MoveCameraAlongSpline();
                    break;
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (seqPlayOnTrigger && !seqPlayed)
            {
                Debug.Log($"{this.name}: Sequence Triggered By Collider");
                StartSequence();
            }
        }
    }

    #endregion

    #region Camera Sequence Methods

    protected virtual void AllocateCameraVariables(CinemachineCamera camera)
    {
        // Find the spline and allocate it
        currentCam = camera;
        currentSpline = currentCam.GetComponent<CinemachineSplineDolly>();
        camPositionUnits = currentSpline.PositionUnits;

        if(camPositionUnits == UnityEngine.Splines.PathIndexUnit.Knot)
        {
            splineMaxKnots = currentSpline.Spline.Splines[0].Count - 1;
            splineTargetKnot = 1;
        }

        splineMaxDistance = currentSpline.Spline.Spline.GetLength();
        currentSpline.CameraPosition = 0f;
    }

    public virtual void StartSequence()
    {
        // Ensure that if a sequence is only meant to trigger once, it can not trigger again
        if(!seqPlayMultipleTimes) 
        {
            if (seqPlayed)
            {
                return;
            }
            else
            {
                seqPlayed = true;
            }
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }


        // Stop player from moving
        if (seqHidePlayerUI) { playerManager.batterUIContainer.SetActive(false); }
        playerManager.StateMachine.ChangeState(playerManager.DialogueState);
        playerCamera.gameObject.SetActive(false);

        // Set brain's transition style
        camBrain.DefaultBlend = seqTransitionToNextCamType;

        // Allocate current camera and reset position
        AllocateCameraVariables(cameras[0]);

        // Turn on current cam
        currentCam.gameObject.SetActive(true);

        playSequence = true;
    }

    /// <summary>
    /// Plays the camera sequence, moving between 
    /// </summary>
    public virtual void PlayCameraSequence()
    {
        // If there is no current cam (for some reason), reset so the player's camera is on and that the intro sequence finishes
        if (currentCam == null) { playerCamera.gameObject.SetActive(true); playSequence = false; return; }

        // If the play can just skip to the next camera
        if (nextCameraAction.action.WasPressedThisFrame())
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
    }

    /// <summary>
    /// Moves the camera along the specified spline
    /// </summary>
    protected virtual void MoveCameraAlongSpline()
    {
        float currSplinePos = currentSpline.CameraPosition;

        // Set the camera's speed
        float camSpeed;

        if (currentCamIndex > CamSpeeds.Length - 1) { camSpeed = DefaultCamSpeed; }
        else { camSpeed = CamSpeeds[currentCamIndex]; }

        float step = splineMaxDistance * (camSpeed * Time.deltaTime);

        // Lerp Camera Position
        float newSplinePos = currSplinePos + step;
        currentSpline.CameraPosition = newSplinePos;

        // If the camera is past the offset, move on to next camera (either automatically or when input is detected)
        if ((currentSpline.CameraPosition > (splineMaxDistance * (1 - CameraOffsetBeforeSwitching))) && (seqPlayAuto || nextCameraAction.action.WasPressedThisFrame()))
        {
            NextCameraInSequence();
        }
    }

    /// <summary>
    /// For moving a camera along a spline with knots - will go to each knot and wait for input
    /// </summary>
    private void MoveCameraToNextKnot()
    {
        float currSplinePos = currentSpline.CameraPosition;

        float camSpeed;

        if (splineTargetKnot > CamSpeeds.Length) { camSpeed = DefaultCamSpeed; }
        else { camSpeed = CamSpeeds[(int)splineTargetKnot - 1]; }


        float step = camSpeed * Time.deltaTime;

        float newSplinePos = currSplinePos + step;

        if (newSplinePos < splineTargetKnot)
        {
            currentSpline.CameraPosition = newSplinePos;
        }

        if(newSplinePos > (splineTargetKnot - CameraOffsetBeforeSwitching))
        {
            if (nextCameraAction.action.WasPressedThisFrame() || seqPlayAuto)
            {
                splineTargetKnot++;
                // will check to see if its the last knot and cancel the sequence if it is
                if (splineTargetKnot > splineMaxKnots)
                {
                    CancelCameraSequence();
                    return;
                }
            }
        }
    }

    private void MoveCameraAlongSplineNormalized()
    {
        // Need to make
    }


    /// <summary>
    /// Changes to next camera in the camera array. Note: Does not work with cameras that use the knot position unit type
    /// </summary>
    public virtual void NextCameraInSequence()
    {
        // Go to next camera in the list
        currentCamIndex++;

        // If there are no more cameras left in the count, cancel the intro
        if (currentCamIndex > cameras.Length - 1)
        {
            currentCamIndex--;
            seqPlayAuto = false;
            CancelCameraSequence();
            return;
        }

        currentCam.gameObject.SetActive(false);

        // Ensure that the next camera is selected
        AllocateCameraVariables(cameras[currentCamIndex]);

        // Teleport brain straight to camera and snap to location
        currentCam.ForceCameraPosition(currentCam.transform.position, currentCam.transform.rotation);
        //currentCam.CancelDamping(false);
        currentCam.gameObject.SetActive(true);

        Debug.Log($"Switch Cameras for Intro Sequence - Cam:{currentCam.name}, Start Pos:{currentSpline.CameraPosition}, End Pos:{splineMaxDistance}");
    }

    /// <summary>
    /// Cancels the current sequence and returns the camera back to the player camera
    /// </summary>
    public virtual void CancelCameraSequence()
    {
        // Ensure that the sequence has finished, and won't trigger again
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        Debug.Log("Cancelling Cam Sequence");

        // Give player movement again
        playerManager.batterUIContainer.SetActive(true);
        playerManager.StateMachine.ChangeState(playerManager.IdleSubState);
        playerCamera.gameObject.SetActive(true);

        camBrain.DefaultBlend = camBrainDefaultBlend;

        playSequence = false;

        currentCam = null;
        currentSpline = null;

        if (!seqPlayMultipleTimes) { seqPlayed = true; } //Destroy(this); }
    }

    /// <summary>
    /// Checks for errors before running level intro camera fly-through
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public void SequenceErrorCheck()
    {
        // Check for the player manager in the scene
        if(playerManager == null)
        {
            playerManager = FindFirstObjectByType<PlayerManager>();

            if(playerManager == null)
            {
                throw new ArgumentNullException(paramName: this.gameObject.name, message: "Sequence couldn't find the player manager");
            }
        }

        // An action needs to be assigned from the Input System
        if(nextCameraAction == null) { throw new ArgumentNullException(paramName: this.gameObject.name, message: "A next camera action needs to be assigned to this camera sequence"); }

        // If there is no fly through introduction set up, stop function
        if (cameras.Length == 0) { throw new ArgumentException(message: "No Cameras Allocated to Level Intro"); }

        // If there is no playerCamera allocated
        if (playerCamera == null) { throw new ArgumentNullException(paramName: this.gameObject.name, message: "No Player Camera Allocated to Camera Sequence"); }

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
