using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// same thing as AbstractNPC
/// </summary>
public class NPC : AbstractNPC
{
    [Header("Dialogue 2")]
    [SerializeField] private string[] repeatedDialogue; // Dialogue for after the original dialogue is finished
    [SerializeField] private string[] endDialogue; //dialogue when quest is complete
    [SerializeField] private string[] completedRepeatDialogue; //same thing but end of quest
    [HideInInspector] public bool QuestAchieved = false; // Has the player gotten the item needed
    public bool QuestComplete = false;
    public bool dialogueFinish = false;

    [Header("Questing")]
    [SerializeField] public int questArrayItem;

    

    public override void Start()
    {
        base.Start(); //reflects start
    }

    public override void Update()
    {
        base.Update(); //reflects update
        //Debug.Log(PlayerManager.invArray[questArrayItem]);
        if (PlayerManager.invArray[questArrayItem] == true) { QuestAchieved = true; }

        if (QuestAchieved && !QuestComplete && currentDialogue == repeatedDialogue)
        {
            EndQuest();
        }
        //if (exclamationMark == null) return;
        if (currentDialogue != repeatedDialogue && currentDialogue != completedRepeatDialogue && index == -1)
        {
            //exclamationMark.SetActive(true);
        } else { /*exclamationMark.SetActive(false);*/ }
    }

    protected override void OnDialogueFinish()
    {  
        // Set future dialogue to the second/ending dialogue and then reset the line number
        if (!QuestComplete) currentDialogue = repeatedDialogue;

        if (QuestComplete == true && dialogueFinish == false)
        {
            currentDialogue = completedRepeatDialogue;

            dialogueFinish = true; 
        }
    }

    

    public void EndQuest()
    {
        currentDialogue = endDialogue;
        QuestComplete = true;
    }
}