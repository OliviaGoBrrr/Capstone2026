using UnityEngine;

public class LightPrism : MonoBehaviour
{
    public LayerMask InteractLayerMask;
    public float maxDistance, hasHit = 0;
    public Mirror_Prism Mirror_Prism;
    public Transform mirrorPos;

    void Update()
    {
        Vector3 endPoint = (transform.forward.normalized * maxDistance) + transform.position;
        float interactDistance = Vector3.Distance(transform.position, endPoint);

        if ((Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactDistance, InteractLayerMask)) && hasHit == 0)
        {
            //if (Vector3.Distance(hit.point, mirrorPos.transform.position) < 2f)
            //{
            //    if ((Physics.Raycast(transform.position, transform.forward, out RaycastHit hit_reflect, interactDistance, InteractLayerMask)) && hasHit == 1)
            //    {
            //        hasHit = 2;
            //        Debug.Log("reflected ray has hit");
            //    }
            //}
            //else
            //{
                hasHit = 1;
                Mirror_Prism.PrismHit();
            //}

        }

        Debug.DrawLine(transform.position, endPoint, Color.green);
    }
}
