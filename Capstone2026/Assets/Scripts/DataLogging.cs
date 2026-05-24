using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DataLogging : MonoBehaviour
{
    string playerName;
    bool playerIDExists;
    List<string> deviceInfo;
    Color defaultColour;
    public TMP_InputField nameField;
    public Button startPlaying;

    /// <summary>
    /// Set the startPlaying Button to be inactive if empty. Set input text to "". 
    /// </summary>
    private void Start()
    {
        nameField.textComponent.text = "";
        if (nameField.textComponent.text.Length > 1)
        {
            startPlaying.interactable = true;
            if (!startPlaying.IsActive()) SaveDeviceInfo();
        }
        else
        {
            startPlaying.interactable = false;
        }
    }
    /// <summary>
    /// Set placeholder text, Check if input is > 1, and < 32, if so, turn the start button on and and take device info
    /// </summary>
    public void checkIfNameInputted()
    {
        TextMeshProUGUI placeholder = (TextMeshProUGUI)nameField.placeholder;
        placeholder.text = "Enter name...";

        if (nameField.textComponent.text.Length > 1 && nameField.textComponent.text.Length < 32)
        {
            startPlaying.interactable = true;
            SaveDeviceInfo();
        }
        else if (nameField.textComponent.text.Length > 32)
        {
            Debug.Log(nameField.textComponent.text.Length);
            nameField.text = "";
            placeholder.text = "Exceeded Char Limit";
        }
        else
        {
            startPlaying.interactable = false;
            placeholder.text = "Enter name...";
        }
    }

    /// <summary>
    /// Save all device info and send it to Discord
    /// </summary>
    void SaveDeviceInfo()
    {
        playerName = nameField.text;

        //Retrieve Device Specs
        List<string> deviceInfo = new List<string>
        {
            $"SystemSpecs",
            $"DeviceName: {SystemInfo.deviceUniqueIdentifier}",
            $"OS: {SystemInfo.operatingSystem}",
            $"Processor: {SystemInfo.processorModel}",
            $"Processor Cores: {SystemInfo.processorCount}",
            $"System Memory: {SystemInfo.systemMemorySize}MB",
            $"GPU: {SystemInfo.graphicsDeviceName}",
            $"GPU Memory: {SystemInfo.graphicsMemorySize}MB",
            $"Graphics Driver Version: {SystemInfo.graphicsDeviceVersion}",
            $"Screen Resolution {Screen.width} x {Screen.height}",
            "IsEditor: " + (Application.isEditor ? "Yes" : "No")
        };

        string messageToSend = string.Join("\n", deviceInfo);

        /*if (!Application.isEditor) */
        DiscordWebhooks.SendMessage(messageToSend, playerName);
    }

    void ActuallyStartTheGameNow()
    {
        SceneManager.LoadScene("SceneSelect");
    }
}
