using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains the gameobjects and methods for interacting
/// with the UI.
/// </summary>
public class BotUI : MonoBehaviour {
    public GameObject   contetnDisplayObject;       // Text gameobject where all the conversation is shown
    public InputField   input;                      // InputField gameobject wher user types their message

    public GameObject   userBubble;                 // reference to user chat bubble prefab
    public GameObject   botBubble;                  // reference to bot chat bubble prefab

    private const int messagePadding = 15;          // space between chat bubbles 
    private int messageHeight = messagePadding;     // int to keep track of where next message should be rendered
    public bool increaseContentObjectHeight;        // bool to check if content object height should be increased


    /// <summary>
    /// This method is used to update the display text object with the user's and bot's messages.
    /// </summary>
    /// <param name="sender">The one who wrote this message</param>
    /// <param name="message">The message</param>
    public void UpdateDisplay (string sender, string message, string messageType) {
        // Create chat bubble and add components
        GameObject chatBubbleChild = CreateChatBubble(sender);
        AddChatComponent(chatBubbleChild, message, messageType);

        // Set chat bubble position
        SetChatBubblePosition(chatBubbleChild.transform.parent.GetComponent<RectTransform>(), sender);

        // Set focus on input field
        input.Select();
        input.ActivateInputField();
    }

    /// <summary>
    /// This method sets the position of the chat bubble inside the contentDisplayObject
    /// </summary>
    /// <param name="chatBubblePos">RectTransform of chat bubble</param>
    /// <param name="sender">Sender who sent the message</param>
    private void SetChatBubblePosition (RectTransform chatBubblePos, string sender) {
        // get horizontal position based on sender
        int horizontalPos = 0;
        if (sender == "Doku") {
            horizontalPos = -20;
        } else if (sender == "Bot") {
            horizontalPos = 20;
        }

        // set the chat bubble in correct place
        messageHeight += 15 + (int)chatBubblePos.sizeDelta.y;
        chatBubblePos.anchoredPosition3D = new Vector3(horizontalPos, -messageHeight, 0);

        if (messageHeight > 340) {
            // update contentDisplayObject hieght
            RectTransform contentRect = contetnDisplayObject.GetComponent<RectTransform>();
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, messageHeight + messagePadding);
        }
    }

    public void RefreshChatBubblePosition (GameObject display) { 
        // refresh position of all gameobjects based on size
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
            chat.transform.SetParent(contetnDisplayObject.transform, false);
        } else if (sender == "Bot") {
            // Create bot chat bubble from prefabs and set it's position
            chat = Instantiate(botBubble);
            chat.transform.SetParent(contetnDisplayObject.transform, false);
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

        // Return empty gameobject on which chat components will be added
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
