using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// this entire thing is pretty much copy pasted from my IGB200 project.
/// everything that's commented out basically just needs to be swapped over to our current systems, i.e. state machine.
/// </summary>
public abstract class AbstractNPC : Interactable
{
    //[SerializeField] protected GameObject exclamationMark;
    [Header("Dialogue")]
    [SerializeField] string[] startDialogue;
    protected string[] currentDialogue;
    protected int index = -1; // Current Line being displayed

    public PlayerInteract playerI;
    public PlayerManager playerM;


    public TextMeshProUGUI textBox;
    private float textSpeed;
    private bool isTyping = false;
    private bool finishTyping = false;

    private Coroutine currentTyping;

    

    public override void onInteract()
    {
        // If no line has been displayed, start dialogue
        if (index == -1) StartDialogue();
    }

    public virtual void Start()
    {
        //should probably find a way to set the player scripts and text box without the inspector cuz the inspector is annoyinnngggg

        currentDialogue = startDialogue;

        /*if (exclamationMark != null)
        {
            exclamationMark = Instantiate(exclamationMark, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
        }*/
    }

    public virtual void Update()
    {
        // If dialogue is active and typing is not currently happening, then when e is pressed go to next line, else finish the line
        if (index == -1) { return; }
        if (!playerI.interactAction.action.WasPressedThisFrame()) { return; } 
        if (isTyping) finishTyping = true;
        else NextLine();
        
    }

    protected virtual void StartDialogue()
    {
        DialogueBoxState(true);
        
        playerM.DialogueState.EnterState();
        //StartCoroutine(TurnToPlayer());
        NextLine();
        
    }

    private IEnumerator TypeLine()
    {
        if (index < 0 || index >= currentDialogue.Length) yield break;

        // Set Text Speed
        // textSpeed = GameManager.instance.textSpeed;
        // Empty text box then type line character by character (makes it look pretty)
        isTyping = true;
        textBox.text = string.Empty;
        foreach(char c in currentDialogue[index].ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(0.1f - (textSpeed) / 100f);
            if (finishTyping) { textBox.text = currentDialogue[index]; finishTyping = false; break; }
        }
        isTyping = false;
    }

    public void NextLine()
    {
        // If typing is still happening or dialogue is finished, exit early
        if (isTyping || index >= currentDialogue.Length - 1)
        {
            EndDialogue();
            return;
        }

        index++;
        if (currentTyping != null) StopCoroutine(currentTyping);
        currentTyping = StartCoroutine(TypeLine());
    }

    private void EndDialogue()
    {
        DialogueBoxState(false);
        playerM.DialogueState.ExitState();
        OnDialogueFinish();
        StartCoroutine(DialogueCooldown());
    }

    protected abstract void OnDialogueFinish(); // Abstract Function for when Dialogue Finishes (For stuff like getting/giving items)

    private IEnumerator DialogueCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        index = -1;
    }

    public void DialogueBoxState(bool state)
    {
        textBox.transform.parent.gameObject.SetActive(state); // Set Parent of Text (Dialogue Box) to state
    }

    private IEnumerator TurnToPlayer()
    {
        
        Transform target = playerM.transform;
        Vector3 direction = playerM.transform.position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float rotationSpeed = 5f;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.05f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        
        
    }
}
