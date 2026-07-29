using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SubmitFeedback : MonoBehaviour
{

    public string nameData;
    public string commentData;
    public int ratingData;

    string formID = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSf6Fh66b0k3bAWV5MM_9Quqa3fanP-QolyjFK5U64R4swvlOA/formResponse";

    public void SubmitFeedbackToSurvey()
    {
        StartCoroutine(Post(nameData, commentData, ratingData));
    }

    public IEnumerator Post(string nameData, string commentData, int ratingData)
    {
        WWWForm form = new WWWForm();

        // Uses the entry IDs from the Google Survey to create a form locally, which is later uploaded to create a response
        // Entry IDs can be found by pressing F12 in a new survey response, filling a reponse with a placeholder value, and finding that
        // response in the Inspector. i.e. Put "Limes" in the name element, Ctrl+F for Limes, and you'll find the entry ID for the names element
        form.AddField("entry.2109252829", nameData);
        form.AddField("entry.223125615", commentData);
        form.AddField("entry.420491183", ratingData);

        using (UnityWebRequest www = UnityWebRequest.Post(formID, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
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
