using UnityEngine;

public class DisableGrapplePoint : MonoBehaviour
{
    [SerializeField] private GameObject grapplePoint;
    public bool isGrappleActive = false;

    [SerializeField] Material flowerMat;
    private Color originalFlowerColour = new Color(1, 0.4470588f, 0.8470588f);

    // Update is called once per frame
    void Update()
    {
        if (isGrappleActive)
        {
            flowerMat.SetColor("_Bottom_Color", originalFlowerColour);
            grapplePoint.SetActive(true);
        }
        else
        {
            flowerMat.SetColor("_Bottom_Color", Color.white);
            grapplePoint.SetActive(false);
        }
    }
}
