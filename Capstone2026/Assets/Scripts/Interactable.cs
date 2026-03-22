using UnityEngine;

/// <summary>
/// Abstract class for an interactable. Further development could allow for individualised interact text, etc.
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    public abstract void onInteract();
}

