using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains the gameobjects and methods for interacting
/// with the UI.
/// </summary>
public class BotUI : MonoBehaviour {
    public GameObject   display;        // Text gameobject where all the conversation is shown
    public InputField   input;          // InputField gameobject wher user types their message

    public GameObject   userBubble;
    public GameObject   botBubble;

    private int messageCounter = 0;

    /// <summary>
    /// This method is used to update the display text object with the
    /// user's and bot's messages.
    /// </summary>
    /// <param name="sender">The one who wrote this message</param>
    /// <param name="message">The message</param>
    public void UpdateDisplay (string sender, string message, string messageType) {
        // create chat bubble and add components
        GameObject chatBubble = CreateChatBubble(sender);
        AddChatComponent(chatBubble, message, messageType);

        // Set focus on input field
        input.Select();
        input.ActivateInputField();
    }

    private GameObject CreateChatBubble (string sender) {
        if (sender == "Doku") {
            // Create user chat bubble from prefabs and set it's position
            GameObject userChat = Instantiate(userBubble);
            userChat.transform.SetParent(display.transform, false);
            RectTransform userPos = userChat.GetComponent<RectTransform>();
            userPos.anchoredPosition3D = new Vector3(-20, -50 * (messageCounter + 1), 0);
            messageCounter++;

            // Add component based on 
            return userChat.transform.GetChild(0).gameObject;
        } else if (sender == "Bot") {
            // Create bot chat bubble from prefabs and set it's position
            GameObject botChat = Instantiate(botBubble);
            botChat.transform.SetParent(display.transform, false);
            RectTransform botPos = botChat.GetComponent<RectTransform>();
            botPos.anchoredPosition3D = new Vector3(20, -50 * (messageCounter + 1), 0);
            messageCounter++;
            return botChat.transform.GetChild(0).gameObject;
        }
        return null;
    }

    private void AddChatComponent (GameObject chatBubble, string message, string messageType) {
        switch (messageType) {
            case "text":
                Text chatMessage = chatBubble.AddComponent<Text>();
                chatMessage.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                chatMessage.fontSize = 14;
                chatMessage.alignment = TextAnchor.UpperLeft;
                chatMessage.text = message;
                break;
            case "image":
                break;
            case "attachment":
                break;
            case "buttons":
                break;
            case "elements":
                break;
            case "quick_replies":
                break;
        }
    }
}
