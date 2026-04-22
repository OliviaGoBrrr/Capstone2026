using UnityEngine;

public class LightPrism : MonoBehaviour
{
    public LayerMask InteractLayerMask;
    public float maxDistance, hasHit = 0;
    public Mirror_Prism Mirror;

    void Update()
    {
        Vector3 endPoint = (transform.forward.normalized * maxDistance) + transform.position;
        float interactDistance = Vector3.Distance(transform.position, endPoint);

        if ((Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactDistance, InteractLayerMask)) && hasHit == 0)
        {
            hasHit = 1;
            Mirror.PrismHit();
        }

        Debug.DrawLine(transform.position, endPoint, Color.green);
    }


}
