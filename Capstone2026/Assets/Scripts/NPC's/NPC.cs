using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// same thing as AbstractNPC
/// </summary>
public class NPC : AbstractNPC
{
    [Header("Quest")]
    [SerializeField] private string[] repeatedDialogue; // Dialogue for after the original dialogue is finished
    [SerializeField] private string[] endDialogue; //dialogue when quest is complete
    [SerializeField] private string[] completedRepeatDialogue; //same thing but end of quest
    [HideInInspector] public bool PlayerHasItem = false;
    [HideInInspector] public bool QuestAchieved = false; // Has the player gotten the item needed
    private bool QuestComplete = false;
    private bool dialogueFinish = false;

    public override void Start()
    {
        base.Start(); //reflects start
    }

    public override void Update()
    {
        base.Update(); //reflects update

        if (QuestAchieved && !QuestComplete && currentDialogue == repeatedDialogue)
        {
            EndQuest();
        }
        if (exclamationMark == null) return;
        if (currentDialogue != repeatedDialogue && currentDialogue != completedRepeatDialogue && index == -1)
        {
            exclamationMark.SetActive(true);
        } else { exclamationMark.SetActive(false); }
    }

    protected override void OnDialogueFinish()
    {
        /* item collection set up
        
        // If it has not been interacted with before this, give the player the item
        if (!PlayerHasItem)
        {
            if (itemToGive != null)
            {
                Destroy(itemToGive);
            }

            PlayerHasItem = true;
        }
        */

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