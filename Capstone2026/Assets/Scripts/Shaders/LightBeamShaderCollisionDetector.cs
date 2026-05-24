using UnityEngine;

public class LightBeamShaderCollisionDetector : MonoBehaviour
{
    RaycastHit hit;

    [SerializeField] private Renderer r;

    private float percentDistanceFromStartToHit = 0;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * r.bounds.size.z, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, r.bounds.size.z))
        {
            percentDistanceFromStartToHit = 1 - hit.distance / r.bounds.size.z;

            r.material.SetFloat("_LightFallOffPoint", percentDistanceFromStartToHit); // give shader distance to collision
        }
        else
        {
            var currentLightFallOffPoint = r.material.GetFloat("_LightFallOffPoint");

            r.material.SetFloat("_LightFallOffPoint", Mathf.Clamp(currentLightFallOffPoint - 0.05f, 0, 1)); // default to max distance
        }

    }
}
