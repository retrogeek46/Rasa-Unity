using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// This class is used to serialize users message into a json
/// object which can be sent over http request to the bot.
/// </summary>
public struct PostData {
    public string message;
    public string sender;
}

/// <summary>
/// This class is used to deserialize the resonse json for each
/// individual message.
/// </summary>
[Serializable]
public class RecieveData {
    public string recipient_id;
    public string text;
    public string image;
    public string attachemnt;
    public string buttons;
    public string elements;
    public string quick_replies;
}

/// <summary>
/// This class is a wrapper for individual messages sent by the bot.
/// </summary>
[Serializable]
public class RootMessages {
    public RecieveData[] messages;
}

/// <summary>
/// This class handles all the requests and serialization and
/// deserialization of data.
/// </summary>
public class NetworkManager : MonoBehaviour {
    // reference to the BotUI class
    public BotUI            botUI;
    // the url at which the bot's custom component is hosted
    private const string    rasa_url = "http://127.0.0.1:5005/webhooks/unity/webhook";


    /// <summary>
    /// This method is called when user has entered their message and hits
    /// the send button. It calls the <see cref="NetworkManager.PostRequest"/> coroutine
    /// to send the user message to bot and also updates UI with the users message.
    /// </summary>
    public void SendMessage () {
        // Get message from textbox and clear the input field
        string message = botUI.input.text;
        botUI.input.text = "";

        // Create a json object from user message
        PostData postMessage = new PostData {
            sender = "doku",
            message = message
        };

        string jsonBody = JsonUtility.ToJson(postMessage);

        // Update display
        botUI.UpdateDisplay("Doku", message);

        // Create a post request with the data to send to Rasa server
        StartCoroutine(PostRequest(rasa_url, jsonBody));
    }

    /// <summary>
    /// This method updates the UI with the bot's response.
    /// </summary>
    /// <param name="response">The response json recieved from the bot</param>
    public void RecieveMessage (string response) {
        // Deserialize response recieved from the bot and show on UI
        Debug.Log("recieved message : " + response);
        RootMessages recieveMessages = 
            JsonUtility.FromJson<RootMessages>("{\"messages\":" + response + "}");
        foreach (RecieveData message in recieveMessages.messages) {
            FieldInfo[] fields = typeof(RecieveData).GetFields();
            //print("properties count is : " + fields.Length);
            foreach (FieldInfo field in fields) {
                string data = null;
                try {
                    data = field.GetValue(message).ToString();
                } catch (NullReferenceException) {
                    Debug.Log("No data");
                }
                if (data != null && field.Name != "recipient_id") {
                    print("getting data for : " + field.Name);
                    botUI.UpdateDisplay("Bot", data);
                }
            }
        }
    }

    public void RecieveMessageTest (string response) {
        // Deserialize response recieved from the bot
        Debug.Log("recieved message : " + response);
        RootMessages recieveMessages =
            JsonUtility.FromJson<RootMessages>("{\"messages\":" + response + "}");

        // show message based on message type on UI
        for (int i = 0; i < recieveMessages.messages.Length; i++) {
            print("looping : " + i);
            PropertyInfo[] properties = recieveMessages.messages[i].GetType().GetProperties();
            print(properties.Length);
            foreach (PropertyInfo property in properties) {
                print("recieved : " + property.GetValue(recieveMessages.messages[i]));
                botUI.UpdateDisplay("Bot", "Under construction");
            }
        }
    }

    /// <summary>
    /// This is a coroutine to asynchronously hit the server url with users message
    /// wrapped in request. The response is deserialized and rendered on the UI
    /// </summary>
    /// <param name="url">The url where the rasa server's custom connector is located</param>
    /// <param name="jsonBody">User message serialized into a json object</param>
    /// <returns></returns>
    private IEnumerator PostRequest (string url, string jsonBody) {
        // Create a request to hit the rasa custom connector
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] rawBody = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawBody);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        // recieve the response asynchronously
        yield return request.SendWebRequest();
        
        // Show response on UI
        RecieveMessage(request.downloadHandler.text);
    }
}
