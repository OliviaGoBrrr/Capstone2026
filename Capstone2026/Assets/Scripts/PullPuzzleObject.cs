using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Hoping to eventually implement script with grappling mechanics (only visually) 
/// </summary>
public class PullPuzzleObject : MonoBehaviour, IInteractable
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public bool hasBeenMoved = false;
    
    void Start()
    {
        if (startPosition == null)
        {
            startPosition = transform.localPosition;
        }
        Debug.Log(startPosition);
        Debug.Log(endPosition);
       
    }
    public void OnInteract()
    {
        if(!hasBeenMoved)
        {
            StartCoroutine(MoveOverTime());
            hasBeenMoved = true;
        }
    }

    public IEnumerator MoveOverTime() 
    {
        float elapsed = 0;
        while (elapsed < 1f) 
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPosition;
    }

}
