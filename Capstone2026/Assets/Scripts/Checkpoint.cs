using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    public PlayerManager playerM;
    public GameObject SavingIcon;
    public int checkpointNumber;
    public bool activated = false;

    public void Awake()
    {
        playerM = FindFirstObjectByType<PlayerManager>().GetComponent<PlayerManager>(); 
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!activated && other.tag == "Player")
        {
            PostGameDataLog.updateCheckpointTimes($"Checkpoint {checkpointNumber}: " + $"{Time.timeSinceLevelLoad}");
            SaveIconSpin();
        }

        activated = true;

        if (playerM.solarPanel.isInLight) // make sure that the checkpoint is acutally in light
        {
            playerM.lastCheckpoint = this.transform.position;
        }
    }

    public void SaveIconSpin()
    {
        SavingIcon.SetActive(true);

        Image gameIcon = SavingIcon.GetComponentInChildren<Image>();
        TextMeshProUGUI savingText = SavingIcon.GetComponentInChildren<TextMeshProUGUI>();

        //Error Handling
        if (gameIcon == null || savingText == null)
        {
            Debug.LogError($"Missing component: Image={gameIcon}, Text={savingText}");
            return;
        }

        //setting the icon to be transparent
        gameIcon.color = new Color(gameIcon.color.r, gameIcon.color.g, gameIcon.color.b, 0f);


        //Save Logo Fade in + Fade Out
        gameIcon.DOKill(); // stop any previous tween on this image

        Sequence sequence = DOTween.Sequence();
        sequence.Append(gameIcon.DOFade(1f, 1f)); // fade in icon
        sequence.Append(savingText.DOFade(1f, 1f)); // fade in icon
        sequence.AppendInterval(2f);           // stay visible
        sequence.Append(gameIcon.DOFade(0f, 1f)); // fade out icon
        sequence.Append(savingText.DOFade(0f, 1f)); // fade out text
        sequence.OnComplete(() => SavingIcon.SetActive(false));
    }
}
