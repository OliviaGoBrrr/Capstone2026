using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// same thing as AbstractNPC
/// </summary>
public class NPC : AbstractNPC
{
    [Header("Dialogue")]
    [SerializeField] private string[] repeatedDialogue; // Dialogue for after the original dialogue is finished
    [SerializeField] private string[] endDialogue; //dialogue when quest is complete
    [SerializeField] private string[] completedRepeatDialogue; //same thing but end of quest
    [HideInInspector] public bool QuestAchieved = false; // Has the player gotten the 1st item needed
    public bool QuestComplete = false;
    public bool dialogueFinish = false;
    public RawImage progressPrompt;

    [Header("Game Manager")]
    public GameManager gameManager;
    

    public override void Start()
    {
        base.Start(); //reflects start
        progressPrompt.gameObject.SetActive(false);
    }

    public override void Update()
    {
        base.Update(); //reflects update

        //if (gameManager.pickedUpCabbages == true) { QuestAchieved = true; }
        if (gameManager.pickedUpMotor == true) { QuestAchieved = true; }

        if (QuestAchieved && !QuestComplete && currentDialogue != endDialogue)
        {
            EndQuest();
        }
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

    public override void NextLine()
    {
        progressPrompt.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(progressPrompt.DOFade(0f, 0.5f)); //fade out icon
        base.NextLine();
        sequence.AppendInterval(3f);
        sequence.Append(progressPrompt.DOFade(1f, 1f)); // fade in icon
        sequence.OnComplete(() => progressPrompt.gameObject.SetActive(true));
    }

    public void EndQuest()
    {
        currentDialogue = endDialogue;
        QuestComplete = true;
    }
}