using System.Collections.Generic;
using UnityEngine;

public static class PostGameDataLog
{
    [Header("During Game Info")]
    static public List<string> timeInEachScene = new List<string>();
    static public void updateSceneTime(string timeSpent)
    {
        timeInEachScene.Add(timeSpent);
        Debug.Log(string.Join("\n", timeInEachScene));
    }

    static public void sendInfoToDiscord()
    {
        string messageToSend = string.Join("\n", timeInEachScene);
        DiscordWebhooks.SendMessage(messageToSend);
    }
}
