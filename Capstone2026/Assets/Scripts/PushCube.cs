 using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PushCube : Interactable
{
    public GameObject Player;
    private Vector3 startPosition;
    private bool CubeHeld = false;

    void Update()
    {
        if(CubeHeld)
        {
            transform.position = new Vector3(Player.transform.position.x + 2, Player.transform.position.y, Player.transform.position.z);
        } 
    }

    public override void onInteract()
    {
        if(CubeHeld)
        {
            CubeHeld = false;
            return;
        }

        CubeHeld = true;
        Debug.Log("grabbed cube!");
    }

    public void MoveCube()
    {
        
    }
}
