using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
    [HideInInspector] public bool triggered = false;

    public void PressurePlateTriggered()
    {
        triggered = true;
    }
}
