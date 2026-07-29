using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class ReportBug : MonoBehaviour
{
    public class FeedbackData
    {
        [Header("Form Data")]
        public string bugCategoryData;
        public string bugDescriptionData;
        public string computerSpecsData = "None";
    }

    public class ComSpecsData
    {
        private static string processorModel = SystemInfo.processorModel;
        private static string gpuModelName = SystemInfo.graphicsDeviceName;
        private static string memorySize = SystemInfo.systemMemorySize.ToString();


        public string systemInfo = new string(
            $"CPU: {processorModel} \n" +
            $"GPU: {gpuModelName} \n" +
            $"RAM Size: {memorySize} MB"
            );
    }

    [Header("UI Inputs")]
    public TMP_Text bugDescription;
    public TMP_Dropdown bugCategory;
    public Toggle computerSpecsToggle;

    string formID = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSf95Glv17uAoOSKyA9FVN53290rVuAuldECT4jQrLnH8nVNJQ/formResponse";

    public void SubmitBug()
    {
        Debug.Log("Started Bug Report");

        FeedbackData feedbackData = new FeedbackData();
        feedbackData.bugCategoryData = bugCategory.options[bugCategory.value].text;
        feedbackData.bugDescriptionData = bugDescription.text;

        if (computerSpecsToggle.isOn)
        {
            feedbackData.computerSpecsData = new ComSpecsData().systemInfo;
        }
        else
        {
            feedbackData.computerSpecsData = "Not Shared";
        }

        StartCoroutine(Post(feedbackData));
    }

    public IEnumerator Post(FeedbackData feedback)
    {
        WWWForm form = new WWWForm();

        // Uses the entry IDs from the Google Survey to create a form locally, which is later uploaded to create a response
        // Entry IDs can be found by pressing F12 in a new survey response, filling a reponse with a placeholder value, and finding that
        // response in the Inspector. i.e. Put "Limes" in the name element, Ctrl+F for Limes, and you'll find the entry ID for the names element
        form.AddField("entry.1336814993", feedback.bugCategoryData);
        form.AddField("entry.1102763912", feedback.bugDescriptionData);
        form.AddField("entry.749335548", feedback.computerSpecsData);

        using (UnityWebRequest www = UnityWebRequest.Post(formID, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Feedback Submitted Successfully");
            }
            else
            {
                Debug.LogError("Error in feedback submission: " + www.error);
            }
        }

    }
}
