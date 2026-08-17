using UnityEngine;

public class IlluminatedEvent : MonoBehaviour
{
    private LightCrystal lightCrystal;

    public bool illuminated = false;
    private bool prevIlluminated;

    [SerializeField] private float eventFireDelay;
    private float eventFireTimer;

    private bool eventFired;

    private void Awake()
    {
        lightCrystal = GetComponent<LightCrystal>();
        prevIlluminated = false;
    }

    private void Update()
    {
        illuminated = lightCrystal.illuminated;

        if(!prevIlluminated && illuminated)
        {
            prevIlluminated = true;
            eventFireTimer = eventFireDelay;
        }

        if(eventFireTimer > 0)
        { 
            eventFireTimer -= Time.deltaTime;
        }

        if (!eventFired)
        {
            if (illuminated && eventFireTimer < 0)
            {
                eventFired = true;
                Debug.Log("Event Fired!", this);
            }
        }
    }
}
