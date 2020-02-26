using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

// A class to help in creating the Json object to be sent to the rasa server
public class PostMessageJson {
    public string message;
    public string sender;
}

[Serializable]
// A class to extract multiple json objects nested inside a value
public class RootReceiveMessageJson {
    public ReceiveMessageJson[] messages;
}

[Serializable]
// A class to extract a single message returned from the bot
public class ReceiveMessageJson {
    public string recipient_id;
    public string text;
    public string image;
    public string attachemnt;
    public string button;
    public string element;
    public string quick_replie;
}

public class NetworkManager : MonoBehaviour {

    private const string rasa_url = "http://localhost:5005/webhooks/rest/webhook";

    public void SendMessageToRasa () {
        // Create a json object from user message
        PostMessageJson postMessage = new PostMessageJson {
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

        RecieveResponse(request.downloadHandler.text);
    }

    // Parse the response received from the bot
    public void RecieveResponse (string response) {
        // Deserialize response recieved from the bot
        RootReceiveMessageJson recieveMessages =
            JsonUtility.FromJson<RootReceiveMessageJson>("{\"messages\":" + response + "}");

        // show message based on message type on UI
        foreach (ReceiveMessageJson message in recieveMessages.messages) {
            FieldInfo[] fields = typeof(ReceiveMessageJson).GetFields();
            foreach (FieldInfo field in fields) {
                string data = null;

                // extract data from response in try-catch for handling null exceptions
                try {
                    data = field.GetValue(message).ToString();
                } catch (NullReferenceException) { }

                // print data
                if (data != null && field.Name != "recipient_id") {
                    Debug.Log("Bot said \"" + data + "\"");
                }
            }
        }
    }
}