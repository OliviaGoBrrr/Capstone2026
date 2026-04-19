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
    [SerializeField] protected GameObject exclamationMark;
    [Header("Dialogue")]
    [SerializeField] string[] startDialogue;
    protected string[] currentDialogue;
    protected int index = -1; // Current Line being displayed

    /* to be changed when systems are set up
    protected Player player;
    */

    private TextMeshProUGUI textBox;
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
        /* to be changed when systems are set up
        textBox = GameObject.FindGameObjectWithTag("UI Manager").GetComponent<UIManager>().dialogueText;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        */
        currentDialogue = startDialogue;
        if (exclamationMark != null)
        {
            exclamationMark = Instantiate(exclamationMark, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
        }
    }

    public virtual void Update()
    {
        // If dialogue is active and typing is not currently happening, then when e is pressed go to next line, else finish the line
        if (index == -1) { return; }
        // if (!Input.GetKeyDown(player.GetComponent<Interaction>().interactKey)) { return; } needs to be changed for state machine
        if (isTyping) finishTyping = true;
        else NextLine();
        
    }

    protected virtual void StartDialogue()
    {
        DialogueBoxState(true);
        /* to be changed to proper system + state machine

        PlayerCamera cam = player.GetComponentInChildren<PlayerCamera>();
        if (cam == null)
        {
            Debug.LogError("Could not find PlayerCamera in Player children!");
            return;
        }
        cam.DialogueActive = true;
        player.CanMove = false;
        StartCoroutine(TurnToPlayer());
        NextLine();
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        */
    }

    private IEnumerator TypeLine()
    {
        if (index < 0 || index >= currentDialogue.Length) yield break;

        // Set Text Speed
        //textSpeed = GameManager.instance.textSpeed;
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
        /* to be changed to proper system + state machine
        DialogueBoxState(false);
        player.GetComponentInChildren<PlayerCamera>().DialogueActive = false;
        player.CanMove = true;
        OnDialogueFinish();
        StartCoroutine(DialogueCooldown());
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        */
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

    /*
    private IEnumerator TurnToPlayer()
    {
        
        Transform target = player.transform;
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float rotationSpeed = 5f;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.05f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        
        
    }
    */
}
