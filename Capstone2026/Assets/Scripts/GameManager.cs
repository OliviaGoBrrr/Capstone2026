using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public PlayerManager PlayerManager;
    bool inCutscene = false;
    public static GameManager gameManager { get; private set; }

    [Header("Questing")]
    public bool pickedUpCabbages = false;

    private void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(gameObject);
            return;
        }

        gameManager = this;
        DontDestroyOnLoad(gameObject);
    }
}
