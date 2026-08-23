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
    public DisableGrapplePoint grappleHitting;

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
            if(grappleHitting != null)
            {
                StopGrapple();
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
            Vector3 hitPos = hit.transform.position;

            if (hit.transform.TryGetComponent<LightCrystal>(out LightCrystal crystal))
            {
                if (crystal.beamsHitting.Count >= crystal.lightsNeededToIlluminate)
                {
                    return;
                }

                if (crystal.beamsHitting.Contains(this) == false)
                {
                    crystal.beamsHitting.Add(this);
                }

                if (crystalHitting == null)
                {
                    crystalHitting = crystal;

                }

                lineRenderer.SetPosition(1, hitPos);

                return;
            }

            else if (hit.transform.TryGetComponent<PlayerManager>(out PlayerManager player))
            {
                if (player.TryGetComponent<PlayerSolarDetector>(out PlayerSolarDetector solarDetector))
                {
                    solarDetector.ChangeBatteryPercent(player.batteryPercent, player.batteryLightRateOfChangePerSecond);
                }

                lineRenderer.SetPosition(1, rayStart + (direction * (Vector3.Distance(rayStart, player.transform.position))));
            }

            else if (hit.transform.TryGetComponent<DisableGrapplePoint>(out DisableGrapplePoint grapple))
            {
                if (grappleHitting == null)
                {
                    grappleHitting = grapple;

                    grappleHitting.isGrappleActive = true;
                }
                lineRenderer.SetPosition(1, hitPos);
            }

            //else if (hit.transform.TryGetComponent<>(out ))
        }
        else
        {
            lineRenderer.SetPosition(1, beamStartPoint.position + (transform.forward * beamMaxDistance));
            StopLightBeam();
            StopGrapple();
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

    public void StopGrapple() // cheese fix, if multiple point at grapple bad stuff happens (i think??)
    {
        if (grappleHitting != null)
        {
            grappleHitting.isGrappleActive = false;
            grappleHitting = null;
        }
    }
}
