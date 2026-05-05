using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public PlayerManager playerM;

    public void OnTriggerEnter(Collider other)
    {
        if(playerM.solarPanel.isInLight) // make sure that the checkpoint is acutally in light
        {
            playerM.lastCheckpoint = this.transform.position;
        }
    }
}
