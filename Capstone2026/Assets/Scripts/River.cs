using UnityEngine;

public class River : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().DeadState.EnterState();
        }
    }
}
