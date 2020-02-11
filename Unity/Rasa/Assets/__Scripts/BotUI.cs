using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains the gameobjects and methods for interacting
/// with the UI.
/// </summary>
public class BotUI : MonoBehaviour {
    public Text         display;    // Text gameobject where all the conversation is shown
    public InputField   input;      // InputField gameobject wher user types their message

    /// <summary>
    /// This method is used to update the display text object with the
    /// user's and bot's messages.
    /// </summary>
    /// <param name="sender">The one who wrote this message</param>
    /// <param name="message">The message</param>
    public void UpdateDisplay (string sender, string message) { 
        display.text += "\n" + sender + " : " + message;
    }
}
