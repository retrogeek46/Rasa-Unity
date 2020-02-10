using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public struct PostMessage {
    public string message;
    public string sender;
}

public class RecievedMessage {
    public struct BotMessages {
        public string recipient_id;
        public string text;
    }
}


public class NetworkManager : MonoBehaviour {

    public BotUI botUI;
    private const string rasa_url = "http://127.0.0.1:5005/webhooks/unity/webhook";

    public void SendMessage () {
        // Get message from textbox and clear the input field
        string message = botUI.input.text;
        botUI.input.text = "";
        botUI.UpdateDisplay("doku", message);
        print("User entered : " + message);

        // Create a json object from user message
        PostMessage postMessage = new PostMessage {
            sender = "doku",
            message = message
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
        string rec = request.downloadHandler.text;
        Debug.Log("Response: " + rec);
        RecievedMessage[] receivedData = JsonHelper.FromJson<RecievedMessage>("{ " + "BotMessages" + ": " + rec + "}");
        print(receivedData);
        botUI.UpdateDisplay("bot", receivedData[0].text);
    }
}
