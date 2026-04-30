using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Hoping to eventually implement script with grappling mechanics (only visually) 
/// </summary>
public class PullPuzzleObject : Interactable
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public bool hasBeenMoved = false;
    
    void Start()
    {
        startPosition = transform.position;
    }
    public override void onInteract()
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
        while (elapsed < .5f) 
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / .5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
    }

}
