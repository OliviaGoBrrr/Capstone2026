using Unity.VisualScripting;
using UnityEngine;

public class PlayerSolarDetector : MonoBehaviour
{
    // Script purpose is for a solar panel to detect direct contact with a light source
    // 4 raycasts on the solar panel are sent toward the light source and if 2 do not detect any collisions the object is in light


    // raycast only detects objects in this layermask
    private int layerMask;
    private int interactLayerMask;
    private int crystalLayerMask;

    [SerializeField] private GameObject lightSource;
    [SerializeField] private Transform centreSolarPanel;
    [SerializeField] private float lengthSolarPanel;

    [SerializeField] private GameObject[] rayCastPositions = new GameObject[4];

    [SerializeField] private int sizeOfRayCastBox = 8;

    [HideInInspector] public bool isInLight = false;
    [HideInInspector] public Transform lastCheckpoint;

    MeshRenderer r;

    void Awake()
    {
        layerMask = 6;
        interactLayerMask = 8;
        crystalLayerMask = 12;

        r = GetComponent<MeshRenderer>();

        // setting positions for the 4 raycasts
        Vector3 solarPanelPos = centreSolarPanel.position;

        rayCastPositions[0].transform.position = new Vector3(solarPanelPos.x - lengthSolarPanel / sizeOfRayCastBox, solarPanelPos.y, solarPanelPos.z - lengthSolarPanel / sizeOfRayCastBox);
        rayCastPositions[1].transform.position = new Vector3(solarPanelPos.x + lengthSolarPanel / sizeOfRayCastBox, solarPanelPos.y, solarPanelPos.z - lengthSolarPanel / sizeOfRayCastBox);
        rayCastPositions[2].transform.position = new Vector3(solarPanelPos.x - lengthSolarPanel / sizeOfRayCastBox, solarPanelPos.y, solarPanelPos.z + lengthSolarPanel / sizeOfRayCastBox);
        rayCastPositions[3].transform.position = new Vector3(solarPanelPos.x + lengthSolarPanel / sizeOfRayCastBox, solarPanelPos.y, solarPanelPos.z + lengthSolarPanel / sizeOfRayCastBox);

    }

    // shifts current battery in either pos+ or neg- depending on if direction is 1 or -1
    public float ChangeBatteryPercent(float batteryPercent, float ROC)
    {
        //Debug.Log(batteryPercent);
        return Mathf.Clamp(batteryPercent + ROC * Time.deltaTime, 0f, 100f);
    }

    public int CheckIfInLight()
    {

        float deg2rad = Mathf.PI / 180;

        float x = lightSource.transform.rotation.x * deg2rad;
        float y = (lightSource.transform.rotation.y + 180) * deg2rad;
        //float z = lightSource.transform.rotation.z * deg2rad;

        Vector3 lightDirection = new Vector3(Mathf.Cos(x) * Mathf.Sin(y), Mathf.Sin(x), Mathf.Cos(x) * Mathf.Cos(y));

        Color rayColor = Color.blue;

        var numberOfCollisions = 0;

        //print(numberOfCollisions);
        
        for (int i = 0; i < rayCastPositions.Length; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(rayCastPositions[i].transform.position, lightSource.transform.TransformDirection(lightDirection), out hit, Vector3.Magnitude(lightDirection) * 100) && hit.transform.gameObject.layer != layerMask && hit.transform.gameObject.layer != interactLayerMask && hit.transform.gameObject.layer != 7 && hit.transform.gameObject.layer != crystalLayerMask)
            {
                numberOfCollisions += 1;
                rayColor = Color.red;
                //Debug.Log(hit.transform.gameObject.name);
            }
            Debug.DrawRay(rayCastPositions[i].transform.position, lightSource.transform.TransformDirection(lightDirection) * 100, rayColor);

            rayColor = Color.blue;
        }
        //print(numberOfCollisions);
        return (numberOfCollisions);
        
    }
}
