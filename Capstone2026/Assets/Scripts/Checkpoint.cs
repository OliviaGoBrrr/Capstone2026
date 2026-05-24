using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public PlayerManager playerM;
    public int checkpointNumber;
    public bool activated = false;

    public void Awake()
    {
        playerM = FindFirstObjectByType<PlayerManager>().GetComponent<PlayerManager>(); 
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            PostGameDataLog.updateCheckpointTimes($"Checkpoint {checkpointNumber}: " + $"{Time.timeSinceLevelLoad}");
        }

        activated = true;
        
        if(playerM.solarPanel.isInLight) // make sure that the checkpoint is acutally in light
        {
            playerM.lastCheckpoint = this.transform.position;
        }
    }
}
