using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DiscordWebhooks : MonoBehaviour
{
    public static string WEBHOOK_URL = "https://discord.com/api/webhooks/1498946439571832862/pP2N2jLfWmpnhSLmBiAeY1v4pj-JLSYGLbLbsSfOCa6mHLjsTNlqySgwXwqP431mNyxC";

    public static void SendMessage(string message, string username = "", string avatar_url = "")
    {
        SendMessageFromWebhook(WEBHOOK_URL, message, username, avatar_url);
    }

    public static void SendScreenshot(string optionalMessage = "", string username = "", string avatar_url = "")
    {
        SendScreenshotFromWebhook(WEBHOOK_URL, optionalMessage, username, avatar_url);
    }

    public static void SendImage(Texture2D texture, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        SendImageFromWebhook(WEBHOOK_URL, texture, optionalMessage, username, avatar_url);
    }

    public static void SendImageFromURL(string imageURL, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        SendImageURLFromWebhook(WEBHOOK_URL, imageURL, optionalMessage, username, avatar_url);
    }

    public static void SendData(string filename, byte[] data, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        SendDataFromWebhook(WEBHOOK_URL, filename, data, optionalMessage, username, avatar_url);
    }

    public static void SendFile(string path, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        SendFileFromWebhook(WEBHOOK_URL, path, optionalMessage, username, avatar_url);
    }

    public static void SendMessageFromWebhook(string webhookURL, string message, string username = "", string avatar_url = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("content", message);
        if (username.Length > 0) form.AddField("username", username);
        if (avatar_url.Length > 0) form.AddField("avatar_url", avatar_url);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static async void SendScreenshotFromWebhook(string webhookURL, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        const string fileName = "Screenshot.png";
        await ScreenshotTools.TakeScreenshotAsync(fileName);
        SendImageFromWebhook(webhookURL, fileName, optionalMessage, username, avatar_url);
    }

    public static void SendImageFromWebhook(string webhookURL, Texture2D texture, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        byte[] bytes = texture.EncodeToPNG();
        WWWForm form = new WWWForm();
        form.headers["Content-Type"] = "multipart/form-data";
        form.AddBinaryData("file1", bytes, "Image.png");
        if (username.Length > 0) form.AddField("username", username);
        if (avatar_url.Length > 0) form.AddField("avatar_url", avatar_url);
        if (optionalMessage.Length > 0) form.AddField("content", optionalMessage);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static void SendImageFromWebhook(string webhookURL, string imagePath, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        byte[] bytes = File.ReadAllBytes(imagePath);
        WWWForm form = new WWWForm();
        form.headers["Content-Type"] = "multipart/form-data";
        form.AddBinaryData("file1", bytes, "Image.png");
        if (username.Length > 0) form.AddField("username", username);
        if (avatar_url.Length > 0) form.AddField("avatar_url", avatar_url);
        if (optionalMessage.Length > 0) form.AddField("content", optionalMessage);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static void SendImageURLFromWebhook(string webhookURL, string imageURL, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        WWWForm form = new WWWForm();
        string start = optionalMessage.Length > 0 ? optionalMessage + "\n" : "";
        if (username.Length > 0) form.AddField("username", username);
        if (avatar_url.Length > 0) form.AddField("avatar_url", avatar_url);
        form.AddField("content", optionalMessage + " " + imageURL);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static void SendDataFromWebhook(string webhookURL, string filename, byte[] data, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        
        WWWForm form = new WWWForm();
        form.headers["Content-Type"] = "multipart/form-data";
        form.AddBinaryData("file1", data, filename);
        if (username.Length > 0) form.AddField("username", username);
        if (avatar_url.Length > 0) form.AddField("avatar_url", avatar_url);
        if (optionalMessage.Length > 0) form.AddField("content", optionalMessage);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static void SendFileFromWebhook(string webhookURL, string path, string optionalMessage = "", string username = "", string avatar_url = "")
    {
        SendDataFromWebhook(webhookURL, Path.GetFileName(path), File.ReadAllBytes(path), optionalMessage, username, avatar_url);
    }
}
