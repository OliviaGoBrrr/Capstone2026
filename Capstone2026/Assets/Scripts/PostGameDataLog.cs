using System.Collections.Generic;
using UnityEngine;

public static class PostGameDataLog
{
    [Header("During Game Info")]
    static public List<string> timeInEachScene = new List<string>();
    static public List<string> timeToReachEachCheckpoint = new List<string>();

    [Header("amount of 'blank' counts")]
    static public int pushpullInteractInt = 0;
    static public int prismInteractInt = 0;
    static public int lightMazeInteractInt = 0;
    static public int grappleAmountInt = 0;
    static public int diedInt = 0;
    static public void listInits()
    {
        timeInEachScene.Add("Time in each scene:");
        timeToReachEachCheckpoint.Add("Time to reach each checkpoint:");
    }
    static public void updateSceneTime(string timeSpent)
    {
        timeInEachScene.Add(timeSpent);
        Debug.Log(string.Join("\n", timeInEachScene));
    }

    static public void updateCheckpointTimes(string timeSpent)
    {
        timeToReachEachCheckpoint.Add(timeSpent);
        Debug.Log(string.Join("\n", timeToReachEachCheckpoint));
    }

    static public void sendInfoToDiscord()
    {
        string allCountInts = string.Join("\n", "Counts:",
            $"Interactions with PushPull objects: {pushpullInteractInt}",
            $"Interactions with prism/rotation objects: {prismInteractInt}",
            $"Interactions with light maze switches {lightMazeInteractInt}",
            $"Times grappled: {grappleAmountInt}",
            $"Times died: {diedInt}");

        string timeSceneStr = string.Join("\n", timeInEachScene);
        string checkpointTimeStr = string.Join("\n", timeToReachEachCheckpoint);

        string messageToSend = string.Join("\n", timeSceneStr, checkpointTimeStr, allCountInts);
        DiscordWebhooks.SendMessage(messageToSend, "Current player game stats:");
    }
}
