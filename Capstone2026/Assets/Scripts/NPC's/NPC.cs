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
    [SerializeField] private string[] quest1RepeatedDialogue; // Dialogue for after the original dialogue is finished
    [SerializeField] private string[] quest2RepeatedDialogue; // Dialogue for after the second quests dialogue is finished
    [SerializeField] private string[] endQuest1Dialogue; //dialogue when quest 1 is complete
    [SerializeField] private string[] endQuest2Dialogue; //dialogue when quest 2 is complete
    [SerializeField] private string[] startQuest2Dialogue; //dialogue when quest 2 is starting
    [SerializeField] private string[] completedRepeatDialogue; //same thing but end of quest
    [HideInInspector] public bool Quest1Achieved = false; // Has the player gotten the 1st item needed
    [HideInInspector] public bool Quest2Achieved = false; // Has the player gotten the 2nd item needed
    public bool Quest1Complete = false;
    public bool Quest2Complete = false;
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

        if (gameManager.pickedUpCabbages == true) { Quest1Achieved = true; }
        if (gameManager.pickedUpMotor == true) { Quest2Achieved = true; }

        if (Quest1Achieved && !Quest1Complete && currentDialogue != endQuest1Dialogue)
        {
            EndQuest1();
        }
        if (Quest2Achieved && !Quest2Complete && currentDialogue != endQuest2Dialogue)
        {
            EndQuest2();
        }
    }

    protected override void OnDialogueFinish()
    {  
        // Set future dialogue to the second/ending dialogue and then reset the line number
        if (!Quest1Complete) currentDialogue = quest1RepeatedDialogue;
        if (Quest1Complete && !Quest2Complete) currentDialogue = quest2RepeatedDialogue;

        if (Quest1Complete == true && dialogueFinish == false)
        {
            currentDialogue = completedRepeatDialogue;
            dialogueFinish = true;
        }
        if (Quest2Complete == true && dialogueFinish == false)
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

    public void EndQuest1()
    {
        currentDialogue = endQuest1Dialogue;
        Quest1Complete = true;
    }

    public void EndQuest2()
    {
        currentDialogue = endQuest2Dialogue;
        Quest2Complete = true;
    }
}