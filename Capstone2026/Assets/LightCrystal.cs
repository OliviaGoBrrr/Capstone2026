using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LightCrystal : MonoBehaviour
{
    public Transform beamStartPoint;
    public float beamMaxDistance = 10f;

    public int lightsNeededToIlluminate = 1;
    public LightCrystal crystalHitting;

    public List<LightCrystal> beamsHitting = new List<LightCrystal>();

    public bool illuminated;
    public LayerMask beamLayerMask;

    private void Awake()
    {
        if(beamStartPoint == null) // Throws an error if the beam's start point isn't attached
        {
            Debug.LogError($"{this.gameObject.name}{this.GetInstanceID()} does not have the BeamStart object attached in the inspector", this);
        }
    }

    private void Update()
    {
        if (illuminated && beamStartPoint != null)
        {
            ShootLightBeam();
        }
        IlluminateCrystal();
    }

    public void IlluminateCrystal()
    {
        if (!illuminated && beamsHitting.Count >= lightsNeededToIlluminate)
        {
            illuminated = true;
        } 
        
        else if(illuminated && lightsNeededToIlluminate > beamsHitting.Count)
        {
            illuminated = false;
            if(crystalHitting != null)
            {
                StopLightBeam();
            }
        }
    }

    public void ShootLightBeam()
    {
        Ray ray = new();

        ray.direction = transform.forward;

        // Visual of the Line
        Debug.DrawLine(beamStartPoint.position, beamStartPoint.position + (ray.direction * beamMaxDistance), Color.red);

        if(Physics.Raycast(beamStartPoint.position, transform.forward, out RaycastHit hit, beamMaxDistance, beamLayerMask))
        {
            LightCrystal crystal = hit.transform.GetComponent<LightCrystal>();

            if (crystal != null)
            {
                if (crystal.beamsHitting.Contains(this) == false)
                {
                    crystal.beamsHitting.Add(this);
                }

                if (crystalHitting == null)
                {
                    crystalHitting = crystal;
                }
            }
        }
        else
        {
            StopLightBeam();
        }


    }

    public void StopLightBeam()
    {
        if(crystalHitting != null)
        {
            crystalHitting.beamsHitting.Remove(this);
            crystalHitting.StopLightBeam();
            crystalHitting = null;
        }
    }
}
