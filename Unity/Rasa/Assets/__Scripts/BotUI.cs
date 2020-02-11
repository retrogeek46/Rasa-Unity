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

        //// Add horizontal layout group
        //HorizontalLayoutGroup horizontalLayout = chat.AddComponent<HorizontalLayoutGroup>();
        //horizontalLayout.childAlignment = TextAnchor.MiddleCenter;

        // increment counter and return reference to empty gameobject on which
        // components can be added.
        messageCounter++;
        return chat.transform.GetChild(0).gameObject;
    }

    private void AddChatComponent (GameObject chatBubbleObject, string message, string messageType) {
        switch (messageType) {
            case "text":
                Text chatMessage = chatBubbleObject.AddComponent<Text>();
                chatMessage.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                chatMessage.fontSize = 18;
                chatMessage.alignment = TextAnchor.MiddleLeft;
                chatMessage.text = message;
                break;
            case "image":
                // disable content size fitter and 
                //GameObject parentChatBubble = chatBubbleObject.transform.parent.gameObject;
                //Destroy(parentChatBubble.GetComponent<ContentSizeFitter>());
                //Destroy(parentChatBubble.GetComponent<VerticalLayoutGroup>());

                Image chatImage = chatBubbleObject.AddComponent<Image>();
                //chatImage.rectTransform.sizeDelta = new Vector2(150, 100);

                //Image img = chatBubbleObject.GetComponent<Image>();
                //img.rectTransform.sizeDelta = new Vector2(120, 70);

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
