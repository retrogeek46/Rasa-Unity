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
    public string button;
    public string element;
    public string quick_replie;
}

/// <summary>
/// This class is a wrapper for individual messages sent by the bot.
/// </summary>
[Serializable]
public class RootMessages {
    public RecieveData[] messages;
}

public static class TextureExtentions {
    public static Texture2D ToTexture2D (this Texture texture) {
        return Texture2D.CreateExternalTexture(
            texture.width,
            texture.height,
            TextureFormat.RGB24,
            false, false,
            texture.GetNativeTexturePtr());
    }
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
        botUI.UpdateDisplay("Doku", message, "text");

        // Create a post request with the data to send to Rasa server
        StartCoroutine(PostRequest(rasa_url, jsonBody));
    }

    /// <summary>
    /// This method updates the UI with the bot's response.
    /// </summary>
    /// <param name="response">The response json recieved from the bot</param>
    public void RecieveMessage (string response) {
        // Deserialize response recieved from the bot
        RootMessages recieveMessages = 
            JsonUtility.FromJson<RootMessages>("{\"messages\":" + response + "}");

        // show message based on message type on UI
        foreach (RecieveData message in recieveMessages.messages) {
            FieldInfo[] fields = typeof(RecieveData).GetFields();
            foreach (FieldInfo field in fields) {
                string data = null;
                // extract data from response in try-catch for 
                // handling null exceptions
                try {
                    data = field.GetValue(message).ToString();
                } catch (NullReferenceException) {}
                // print data
                if (data != null && field.Name != "recipient_id") {
                    botUI.UpdateDisplay("Bot", data, field.Name);
                }
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

    /// <summary>
    /// This method gets url resource from link and applies it to the passed texture.
    /// </summary>
    /// <param name="url">url where the image resource is located</param>
    /// <param name="image">RawImage object on which the texture will be applied</param>
    /// <returns></returns>
    public static IEnumerator SetImageTextureFromUrl (string url, Image image) {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else {
            // Create Texture2D from Texture object
            Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Texture2D texture2D = texture.ToTexture2D();
            
            // set max size for image width and height based on texture dimensions
            float imageWidth = 0, imageHeight = 0, texWidth = texture2D.width, texHeight = texture2D.height;
            if ((texture2D.width > texture2D.height) && texHeight > 0) {
                // Landscape image
                imageWidth = texWidth;
                if (imageWidth > 200) imageWidth = 200;
                float ratio = texWidth / imageWidth;
                imageHeight = texHeight / ratio;
            }
            if ((texture2D.width < texture2D.height) && texWidth > 0) {
                // Portrait image
                imageHeight = texHeight;
                if (imageHeight > 200) imageHeight = 200;
                float ratio = texHeight / imageHeight;
                imageWidth = texWidth/ ratio;
            }
            //print("Image dimensions : " + imageWidth + ", " + imageHeight);
            //texture2D.Resize((int)imageWidth, (int)imageHeight);
            //texture2D.Apply();
            //Texture2D tex = new Texture2D((int)imageWidth, (int)imageHeight);
            //tex.SetPixels(texture2D.GetPixels());
            //tex.Apply();

            //TextureScale.Bilinear(texture2D, (int)imageWidth, (int)imageHeight);

            image.sprite = Sprite.Create(
                texture2D, 
                new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), 
                new Vector2(0.5f, 0.5f), 100.0f);
            //image.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
}
