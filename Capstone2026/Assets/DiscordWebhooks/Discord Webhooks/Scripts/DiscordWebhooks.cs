using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

public class DiscordWebhooks : MonoBehaviour
{
    public string WEBHOOK_URL = "https://discord.com/api/webhooks/1498946439571832862/pP2N2jLfWmpnhSLmBiAeY1v4pj-JLSYGLbLbsSfOCa6mHLjsTNlqySgwXwqP431mNyxC";

    private static DiscordWebhooks _instance;
    public static DiscordWebhooks instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<DiscordWebhooks>();
            return _instance;
        }
    }

    public static new void SendMessage(string message)
    {
        SendMessage(instance.WEBHOOK_URL, message);
    }

    public static void SendScreenshot(string optionalMessage = "")
    {
        SendScreenshot(instance.WEBHOOK_URL, optionalMessage);
    }

    public static void SendImage(Texture2D texture, string optionalMessage = "")
    {
        SendImage(instance.WEBHOOK_URL, texture, optionalMessage);
    }

    public static void SendImageFromURL(string imageURL, string optionalMessage = "")
    {
        SendImageFromURL(instance.WEBHOOK_URL, imageURL, optionalMessage);
    }

    public static void SendData(string filename, byte[] data, string optionalMessage = "")
    {
        SendData(instance.WEBHOOK_URL, filename, data, optionalMessage);
    }

    public static void SendFile(string path, string optionalMessage = "")
    {
        SendFile(instance.WEBHOOK_URL, path, optionalMessage);
    }

    public static void SendMessage(string webhookURL, string message)
    {
        WWWForm form = new WWWForm();
        form.AddField("content", message);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static void SendScreenshot(string webhookURL, string optionalMessage = "")
    {
        byte[] bytes = ScreenCapture.CaptureScreenshotAsTexture().EncodeToPNG();
        WWWForm form = new WWWForm();
        form.headers["Content-Type"] = "multipart/form-data";
        form.AddBinaryData("file1", bytes, "Image.png");
        if (optionalMessage.Length > 0) form.AddField("content", optionalMessage);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static void SendImage(string webhookURL, Texture2D texture, string optionalMessage = "")
    {
        byte[] bytes = texture.EncodeToPNG();
        WWWForm form = new WWWForm();
        form.headers["Content-Type"] = "multipart/form-data";
        form.AddBinaryData("file1", bytes, "Image.png");
        if (optionalMessage.Length > 0) form.AddField("content", optionalMessage);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static void SendImageFromURL (string webhookURL, string imageURL, string optionalMessage = "")
    {
        WWWForm form = new WWWForm();
        string start = optionalMessage.Length > 0 ? optionalMessage + "\n" : "";
        form.AddField("content", optionalMessage + imageURL);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static void SendData(string webhookURL, string filename, byte[] data, string optionalMessage = "")
    {
        
        WWWForm form = new WWWForm();
        form.headers["Content-Type"] = "multipart/form-data";
        form.AddBinaryData("file1", data, filename);
        if (optionalMessage.Length > 0) form.AddField("content", optionalMessage);
        UnityEngine.Networking.UnityWebRequest.Post(webhookURL, form).SendWebRequest();
    }

    public static void SendFile(string webhookURL, string path, string optionalMessage = "")
    {
        SendData(webhookURL, Path.GetFileName(path), File.ReadAllBytes(path), optionalMessage);
    }
}
