using UnityEngine;

public class ReplicateLaser : MonoBehaviour
{
    public Camera cam;
    private PlayerCCMovement player;
    RaycastHit hit;

    Ray ray;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerCCMovement>();
    }

    private void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(player.transform.position, ray.direction, Color.yellow);
    }
}
