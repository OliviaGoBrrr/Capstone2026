using UnityEngine;
using UnityEngine.UIElements;

public class LightPrism : MonoBehaviour
{
    [SerializeField] LayerMask raycastMask;
    [SerializeField] LayerMask interactMask;
    public float maxDistance;
    public Mirror_Prism Mirror_Prism;

    void Update()
    {
        Vector3 endPoint = (transform.forward.normalized * maxDistance) + transform.position;
        float interactDistance = Vector3.Distance(transform.position, endPoint);

        if ((Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactDistance, raycastMask)))
        {
            //if is part of the interact layer, and is that the layer the ray has just hit
            if ((interactMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Mirror_Prism.PrismHit();
                Vector3 incomingVec = hit.point - transform.position;
                Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
                Debug.DrawLine(hit.point, reflectVec, Color.red);
            }
        }
        Debug.DrawLine(transform.position, endPoint, Color.green);
    }
}