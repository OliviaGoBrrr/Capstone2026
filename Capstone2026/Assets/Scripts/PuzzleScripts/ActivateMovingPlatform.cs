using Unity.VisualScripting;
using UnityEngine;

public class ActivateMovingPlatform : MonoBehaviour
{
    public bool isPlatformActive = false;

    [SerializeField] private GameObject platform;
    [SerializeField] private Vector3 activePosition;
    [SerializeField] private Vector3 deactivePosition;

    private MovingPlatform platformScript;

    public CharacterController player;

    private void Awake()
    {
        //platform.transform.position = deactivePosition;

        platformScript = platform.GetComponent<MovingPlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlatformActive)
        {
            if (Vector3.Distance(platform.transform.localPosition, activePosition) < 0.1f) return;

            platform.transform.position -= new Vector3(0, 0, 2 * Time.deltaTime);

            if (platformScript.isPlayerOn)
            {
                player.Move(new Vector3(0, 0, -2 * Time.deltaTime));
            }
        }
        else
        {
            if (Vector3.Distance(platform.transform.localPosition, deactivePosition) < 0.1f) return;

            platform.transform.position += new Vector3(0, 0, 2 * Time.deltaTime);

            if (platformScript.isPlayerOn)
            {
                player.Move(new Vector3(0, 0, 2 * Time.deltaTime));
            }
        }
    }
}
