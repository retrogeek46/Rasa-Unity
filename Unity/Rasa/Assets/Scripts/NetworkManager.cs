using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// A struct to help in creating the Json object to be sent to the rasa server
public struct PostMessage {
    public string message;
    public string sender;
}

public class NetworkManager : MonoBehaviour {

    private const string rasa_url = "http://localhost:5005/webhooks/rest/webhook";

    public void SendMessageToRasa () {
        // Create a json object from user message
        PostMessage postMessage = new PostMessage {
            sender = "user",
            message = "Hi"
        };

        string jsonBody = JsonUtility.ToJson(postMessage);
        print("User json : " + jsonBody);

        // Create a post request with the data to send to Rasa server
        StartCoroutine(PostRequest(rasa_url, jsonBody));
    }

    private IEnumerator PostRequest (string url, string jsonBody) {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] rawBody = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawBody);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Response: " + request.downloadHandler.text);
    }
}