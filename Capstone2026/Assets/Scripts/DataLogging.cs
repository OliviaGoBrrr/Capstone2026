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
    public TMP_InputField nameField;
    public Button startPlaying;

    public void SaveDeviceInfo()
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
        startPlaying.gameObject.SetActive(true);
    }

    public void ActuallyStartTheGameNow()
    {
        SceneManager.LoadScene("SceneSelect");
    }
}
