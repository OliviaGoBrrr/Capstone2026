using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
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

    public LineRenderer lineRenderer;

    private void Awake()
    {
        if(beamStartPoint == null) // Throws an error if the beam's start point isn't attached
        {
            beamStartPoint = transform;
            //Debug.LogError($"{this.gameObject.name}{this.GetInstanceID()} does not have the BeamStart object attached in the inspector", this);
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, beamStartPoint.position);
        lineRenderer.enabled = false;
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
            lineRenderer.enabled = false;
            if(crystalHitting != null)
            {
                StopLightBeam();
            }
        }
    }

    public void ShootLightBeam()
    {
        Vector3 direction = transform.forward;
        Vector3 rayStart = beamStartPoint.position;

        if (lineRenderer.enabled == false) { lineRenderer.enabled = true; }
        lineRenderer.SetPosition(0, beamStartPoint.position);

        // Visual of the Line
        Debug.DrawLine(rayStart, rayStart + (direction * beamMaxDistance), Color.red);

        // Checks to see if the ray hits anything
        if (Physics.Raycast(rayStart, direction, out RaycastHit hit, beamMaxDistance, beamLayerMask))
        {
            lineRenderer.SetPosition(1, new Vector3(hit.transform.position.x, beamStartPoint.position.y, hit.transform.position.z));

            if (hit.transform.TryGetComponent<LightCrystal>(out LightCrystal crystal))
            {
                if (crystal.beamsHitting.Count >= crystal.lightsNeededToIlluminate)
                {
                    return;
                }

                if (crystal.beamsHitting.Contains(this) == false)
                {
                    crystal.beamsHitting.Add(this);
                    crystal.illuminated = true;
                }

                if (crystalHitting == null)
                {
                    crystalHitting = crystal;

                }

                return;
            }

            else if (hit.transform.TryGetComponent<PlayerManager>(out PlayerManager player))
            {
                if (player.TryGetComponent<PlayerSolarDetector>(out PlayerSolarDetector solarDetector))
                {
                    Debug.Log("Recharging Player");
                    solarDetector.ChangeBatteryPercent(player.batteryPercent, player.batteryLightRateOfChangePerSecond);
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, beamStartPoint.position + (transform.forward * beamMaxDistance));
            StopLightBeam();
        }
    }

    public void StopLightBeam()
    {
        if(crystalHitting != null)
        {
            crystalHitting.beamsHitting.Remove(this); // causing a stack overflow if two beams hit eachother
            crystalHitting.StopLightBeam();
            crystalHitting = null;
        }
    }
}
