using UnityEngine;

public class GrappleableObject : MonoBehaviour
{
    private Collider grappleCollider;

    public bool hovered;

    private void OnCollisionEnter(Collision collision)
    {
       if(collision != null)
        {
            Debug.Log($"{collision.gameObject.name} is hovering {this.name}");
            if (collision.gameObject.name == "GrappleTrigger")
            {
                hovered = true;
            }
        } 
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision != null)
        {
            Debug.Log($"{collision.gameObject.name} is no longer hovering {this.name}");
            if (collision.gameObject.name == "GrappleTrigger")
            {
                hovered = false;
            }
        }
    }

}
