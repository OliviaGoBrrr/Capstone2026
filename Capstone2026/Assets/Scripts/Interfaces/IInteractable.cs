using UnityEngine;

/// <summary>
/// Interface for an interactable.
/// </summary>
public interface IInteractable
{
    public void OnInteract();

    public void ActivateOutline();

    public void DeactivateOutline();
}