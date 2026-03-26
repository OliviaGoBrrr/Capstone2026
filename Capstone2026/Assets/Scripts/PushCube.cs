using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PushCube : Interactable
{
    public AnimationCurve speedCurve;
    public float duration = 1f; 
    private float timeElapsed = 0f;
    private Vector3 startPosition;
    private bool CubeMoving = false;

    void Update()
    {
        if(CubeMoving)
        {
            MoveCube();
        }
    }

    public override void onInteract()
    {
        if(!CubeMoving) // only allow the cube to be moved again if it isn't currently moving
        {
            startPosition = transform.position;
            timeElapsed = 0;
            CubeMoving = true;
        }
    }

    public void MoveCube()
    {
        if (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveValue = speedCurve.Evaluate(t); // speed of the cube at current frame. determined by an animation curve

            Vector3 targetPosition = new Vector3(startPosition.x + 3, startPosition.y, startPosition.z); // right now it just goes in one direction but with further implementation would go in the directions that the puzzle calls for. if needed could also be adjusted for pulling

            transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue); // move the cube

            timeElapsed += Time.deltaTime;       
        }

        else
            CubeMoving = false;
    }
}
