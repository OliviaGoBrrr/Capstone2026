//using Mono.Cecil.Cil;
using UnityEngine;

public class Temp_LightMaze : MonoBehaviour
{
    //Good Day Programmers who are reading this message, as you can see, I named this "TEMP". This is because
    //it is some rough code I have jumbled together that is essentially just an on/off switch, as I did not
    //want to mess with the pre-existing lever switch without permission.

    //The intention, as can be seen at the end of the 4/12 designer meeting notes, is to have a rope attached to a tree,
    //which can be interacted with, and thus, pulled down. The tree is blocking sunlight in one position, and allowing
    //sunlight in the other. So when it's in the default state, the first two *blue* platforms are in the light,
    //and then when it is toggled, those two go in the shade, and the other two get lit up. 

    //This puzzle *may* be reworked, as the first state of the puzzle, where the top left and bottom right blue platforms
    //are active, is likely going to be hard to have the tree light up, while not lighting up the middle two blue platforms.

    public GameObject Switch_A, Switch_B, RedPlatform, RedPlatform_Blocker, BluePlatform_A, BluePlatform_B, BluePlatform_B_Blocker;
    public Transform Player;
    public bool has_already_entered = false;

    void Start()
    {
        RedPlatform.SetActive(false);
        BluePlatform_B.SetActive(false);
        BluePlatform_A.SetActive(true);
    }

    void Update()
    {
        if (Vector3.Distance(Switch_A.transform.position, Player.transform.position) < 5f && !has_already_entered) flipState_Blue(true);
        else if (Vector3.Distance(Switch_A.transform.position, Player.transform.position) < 2f && has_already_entered) flipState_Blue(false);
        if (Vector3.Distance(Switch_B.transform.position, Player.transform.position) < 5f) flipState_Red(true);
    }

    void flipState_Blue(bool state)
    {
        BluePlatform_A.SetActive(!state);
        BluePlatform_B.SetActive(state);
        if (!RedPlatform.activeSelf) BluePlatform_B_Blocker.SetActive(!state);
        has_already_entered = true;
    }

    void flipState_Red(bool state)
    {
        RedPlatform.SetActive(state);
        RedPlatform_Blocker.SetActive(false);
        BluePlatform_B_Blocker.SetActive(false);
    }
}
