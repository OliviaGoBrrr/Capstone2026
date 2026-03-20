using Unity.VisualScripting;
using UnityEngine;

public class SolarDetector : MonoBehaviour
{
    // Script purpose is for a solar panel to detect direct contact with a light source
    // 4 raycasts on the solar panel are sent toward the light source and if 2 do not detect any collisions the object is in light


    // raycast only detects objects in this layermask
    private LayerMask layerMask;

    [SerializeField] private GameObject lightSource;
    [SerializeField] private Transform centreSolarPanel;
    [SerializeField] private float lengthSolarPanel;

    [SerializeField] private GameObject[] rayCastPositions = new GameObject[4];

    [SerializeField] private int sizeOfRayCastBox = 8;

    private MeshRenderer MeshRenderer;

    [HideInInspector] public bool isInLight = false;


    void Awake()
    {
        layerMask = LayerMask.GetMask("Obstacle");

        MeshRenderer = GetComponent<MeshRenderer>();


        // setting positions for the 4 raycasts
        Vector3 solarPanelPos = centreSolarPanel.position;

        rayCastPositions[0].transform.position = new Vector3(solarPanelPos.x - lengthSolarPanel / sizeOfRayCastBox, solarPanelPos.y, solarPanelPos.z - lengthSolarPanel / sizeOfRayCastBox);
        rayCastPositions[1].transform.position = new Vector3(solarPanelPos.x + lengthSolarPanel / sizeOfRayCastBox, solarPanelPos.y, solarPanelPos.z - lengthSolarPanel / sizeOfRayCastBox);
        rayCastPositions[2].transform.position = new Vector3(solarPanelPos.x - lengthSolarPanel / sizeOfRayCastBox, solarPanelPos.y, solarPanelPos.z + lengthSolarPanel / sizeOfRayCastBox);
        rayCastPositions[3].transform.position = new Vector3(solarPanelPos.x + lengthSolarPanel / sizeOfRayCastBox, solarPanelPos.y, solarPanelPos.z + lengthSolarPanel / sizeOfRayCastBox);

    }

    void FixedUpdate()
    {
        if (CheckIfInLight() <= 2)
        {
            isInLight = true;
            MeshRenderer.material.color = Color.white;
        }
        else
        {
            isInLight = false;
            MeshRenderer.material.color = Color.red;
        }
    }


    int CheckIfInLight()
    {
        Vector3 distance = lightSource.transform.position - transform.position;

        

        var numberOfCollisions = 0;

        print(numberOfCollisions);

        for (int i = 0; i < rayCastPositions.Length; i++)
        {
            if (Physics.Raycast(rayCastPositions[i].transform.position, Vector3.Normalize(distance), Vector3.Magnitude(distance), layerMask))
            {
                numberOfCollisions += 1;
            }
            Debug.DrawRay(rayCastPositions[i].transform.position, transform.TransformDirection(distance), Color.blue);
        }
        print(numberOfCollisions);
        return (numberOfCollisions);
        
    }
}
