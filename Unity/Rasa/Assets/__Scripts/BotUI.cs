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

    public GameObject   userBubble;     // reference to user chat bubble prefab
    public GameObject   botBubble;      // reference to bot chat bubble prefab

    private int messageCounter = 0;
    private int messageHeight = 15;      // int to keep track of where next message should be rendered

    /// <summary>
    /// This method is used to update the display text object with the
    /// user's and bot's messages.
    /// </summary>
    /// <param name="sender">The one who wrote this message</param>
    /// <param name="message">The message</param>
    public void UpdateDisplay (string sender, string message, string messageType) {
        // create chat bubble and add components
        GameObject chatBubbleChildObject = CreateChatBubble(sender);
        AddChatComponent(chatBubbleChildObject, message, messageType);

        // set the chat bubble in correct place
        RectTransform chatBubblePos = 
            chatBubbleChildObject.transform.parent.gameObject.GetComponent<RectTransform>();
        messageHeight += 15 + (int)chatBubblePos.sizeDelta.y;
        chatBubblePos.anchoredPosition3D = new Vector3(
            chatBubblePos.anchoredPosition3D.x, 
            -messageHeight,
            0);


        // Set focus on input field
        //input.Select();
        input.ActivateInputField();
    }

    /// <summary>
    /// This method creates chat bubbles from prefabs and sets their positions.
    /// </summary>
    /// <param name="sender">The sender of message for which bubble is rendered</param>
    /// <returns>Reference to empty gameobject on which message components can be added</returns>
    private GameObject CreateChatBubble (string sender) {
        GameObject chat = null;
        if (sender == "Doku") {
            // Create user chat bubble from prefabs and set it's position
            chat = Instantiate(userBubble);
            chat.transform.SetParent(display.transform, false);
            RectTransform userPos = chat.GetComponent<RectTransform>();
            userPos.anchoredPosition3D = new Vector3(-20, -50 * (messageCounter + 1), 0);
        } else if (sender == "Bot") {
            // Create bot chat bubble from prefabs and set it's position
            chat = Instantiate(botBubble);
            chat.transform.SetParent(display.transform, false);
            RectTransform botPos = chat.GetComponent<RectTransform>();
            botPos.anchoredPosition3D = new Vector3(20, -50 * (messageCounter + 1), 0);
        }

        // Add content size fitter
        ContentSizeFitter chatSize = chat.AddComponent<ContentSizeFitter>();
        chatSize.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        chatSize.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Add vertical layout group
        VerticalLayoutGroup verticalLayout = chat.AddComponent<VerticalLayoutGroup>();
        if (sender == "Doku") {
            verticalLayout.padding = new RectOffset(10, 20, 5, 5);
        } else if (sender == "Bot") {
            verticalLayout.padding = new RectOffset(20, 10, 5, 5);
        }
        verticalLayout.childAlignment = TextAnchor.MiddleCenter;

        // increment counter and return reference to empty gameobject on which
        // components can be added.
        messageCounter++;
        return chat.transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// This method adds message component to chat bubbles based on message type.
    /// </summary>
    /// <param name="chatBubbleObject">The empty gameobject under chat bubble</param>
    /// <param name="message">message to be shown</param>
    /// <param name="messageType">The type of message (text, image etc)</param>
    private void AddChatComponent (GameObject chatBubbleObject, string message, string messageType) {
        switch (messageType) {
            case "text":
                // Create and init Text component
                Text chatMessage = chatBubbleObject.AddComponent<Text>();
                chatMessage.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                chatMessage.fontSize = 18;
                chatMessage.alignment = TextAnchor.MiddleLeft;
                chatMessage.text = message;
                break;
            case "image":
                // Create and init Image component
                Image chatImage = chatBubbleObject.AddComponent<Image>();
                StartCoroutine(NetworkManager.SetImageTextureFromUrl(message, chatImage));
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
