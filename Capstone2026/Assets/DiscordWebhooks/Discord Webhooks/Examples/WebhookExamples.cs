using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class shows some example implementations for various Discord Webhook functionality.
/// 
/// For these functions to work, you first need to assign a valid webhook URL in the inspector
/// so that these functions know where to upload the content.
/// </summary>
public class WebhookExamples : MonoBehaviour
{
    // YOU MUST ASSIGN A VALID VALID WEBHOOK URL IN THE INSPECTOR FOR THESE EXAMPLES TO WORK.
    //public string webhookURL;

    /// <summary>
    /// Example 1: Write some text (max 2000 characters).
    /// </summary>
    public void Example1()
    {
        if (!WebhookAssigned()) return;

        string message = "```" +
            $"Playtester Name: Joel" + Environment.NewLine +
            $"Time Spent Playing: {Time.realtimeSinceStartup}s" + Environment.NewLine +
            $"Deaths: 5" + Environment.NewLine +
            $"Level Reached: 7" + Environment.NewLine +
            $"Items Collected: 13" + Environment.NewLine +
            $"Enemies Killed: 106" + Environment.NewLine +
            $"Player Feedback: Game seems cool. I would play it again. The ranged enemy is way too strong. Level 3 is repetitive." +
            "```";

        //DiscordWebhooks.SendMessage($"Hello! This is a some basic text.");
        DiscordWebhooks.SendMessage(message);
    }


    public void ExampleMethod ()
    {
        DiscordWebhooks.SendMessage("This is an example message");
    }

    /// <summary>
    /// Example 2: Write some text, but put it in a codeblock.
    /// </summary>
    public void Example2()
    {
        if (!WebhookAssigned()) return;
        DiscordWebhooks.SendMessage("```Hello! This is some text in a code block!```");
    }

    /// <summary>
    /// Example 3: Write some text, but give the bot a custom username.
    /// </summary>
    public void Example3()
    {
        if (!WebhookAssigned()) return;
        WWWForm form = new WWWForm();
        form.AddField("content", $"Hello! This is a some basic text with a custom bot name.");
        form.AddField("username", $"Botty McBottson");
        UnityEngine.Networking.UnityWebRequest.Post(DiscordWebhooks.instance.WEBHOOK_URL, form).SendWebRequest();
    }

    /// <summary>
    /// Example 4: Write some text, but give the bot a custom username and avatar URL.
    /// </summary>
    public void Example4()
    {
        if (!WebhookAssigned()) return;
        WWWForm form = new WWWForm();
        form.AddField("content", $"Hello! This is a some basic text with a custom bot name and avatar.");
        form.AddField("username", $"Botty McBottson");
        form.AddField("avatar_url", "https://preview.redd.it/8n6x4gk2pnr71.png?auto=webp&s=c2e0b2084c4046ce9091c585e0f85752f767c2ed"); // TODO
        UnityEngine.Networking.UnityWebRequest.Post(DiscordWebhooks.instance.WEBHOOK_URL, form).SendWebRequest();
    }

    /// <summary>
    /// Example 5: Write some text with some custom markdown.
    /// </summary>
    public void Example5()
    {
        if (!WebhookAssigned()) return;
        DiscordWebhooks.SendMessage($"This _is_ **a** ***custom*** __message__ __*containing*__ __**custom**__ __***markdown***__ ~~text~~.");
    }

    /// <summary>
    /// Example 6: Write some text, and have it converted to speech (TTS enabled).
    /// </summary>
    public void Example6()
    {
        if (!WebhookAssigned()) return;
        WWWForm form = new WWWForm();
        form.AddField("username", $"Admin");
        form.AddField("content", $"This is a text to speech message.");
        form.AddField("tts", "true");
        UnityEngine.Networking.UnityWebRequest.Post(DiscordWebhooks.instance.WEBHOOK_URL, form).SendWebRequest();
    }

    /// <summary>
    /// Example 7: Write some text and embed an image via URL.
    /// </summary>
    public void Example7()
    {
        if (!WebhookAssigned()) return;
        DiscordWebhooks.SendImageFromURL("https://preview.redd.it/8n6x4gk2pnr71.png?auto=webp&s=c2e0b2084c4046ce9091c585e0f85752f767c2ed", "This is the Unity logo.");
    }

    /// <summary>
    /// Example 8: Upload an image from file.
    /// </summary>
    public void Example8()
    {
        if (!WebhookAssigned()) return;
        string path = "Assets/IGB388/Week 8/Base/DiscordWebHooks/Images/Example.jpg";
        WWWForm form = new WWWForm();
        form.headers["Content-Type"] = "multipart/form-data";
        form.AddBinaryData("file1", File.ReadAllBytes(path), "Image.jpg");
        UnityEngine.Networking.UnityWebRequest.Post(DiscordWebhooks.instance.WEBHOOK_URL, form).SendWebRequest();
    }

    /// <summary>
    /// Example 9: Take screenshot of game and upload it.
    /// </summary>
    public void Example9()
    {
        if (!WebhookAssigned()) return;
        DiscordWebhooks.SendScreenshot();
    }

    /// <summary>
    /// Example 10: Upload a screenshot of the game with included text.
    /// </summary>
    public void Example10()
    {
        if (!WebhookAssigned()) return;
        DiscordWebhooks.SendScreenshot("This is a screenshot from the game.");
    }

    // Check that a Webhook has been assigned in the inspector, otherwise tell the user.
    public bool WebhookAssigned()
    {
        if (DiscordWebhooks.instance.WEBHOOK_URL == string.Empty)
            Debug.LogError("No Webhook URL Assigned! Assign one to the DiscordWebhookExample object.");
        return (DiscordWebhooks.instance.WEBHOOK_URL != string.Empty);
    }
}
