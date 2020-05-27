using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

/// <summary>
/// This class contains methods and variables for STT service
/// </summary>
public class SpeechRecognition : MonoBehaviour {

    [Header("Gameobject References")]
    public Animator                 voiceIconAnimator;          // Animator component for voice icon
    public InputField               inputField;                 // reference to input field component
    public Image                    voiceIcon;                  // reference to voice icon image component

    [HideInInspector]
    public string                   dictationResult = "";       // string to hold STT prediction result
    private string                  tempString = "";            // flag string to know when text prediction occurs
    private DictationRecognizer     dictationRecognizer;        // DictationRecognizer component to convert speech to text
    private SpeechSystemStatus      dictationRecognizerStatus;  // variable to check if STT is active

    /// <summary>
    /// This method initilizes necessary variables with default values
    /// </summary>
    void Start () {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizerStatus = SpeechSystemStatus.Stopped;
        voiceIconAnimator.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        // if any text has been recognized from speech, stop STT service
        if (dictationResult != tempString) {
            tempString = dictationResult;
            inputField.text = dictationResult;
            StopSpeechToText();
        }
        
        // update STT status variable 
        if (dictationRecognizer.Status != dictationRecognizerStatus) {
            dictationRecognizerStatus = dictationRecognizer.Status;
        }
    }

    /// <summary>
    /// This method starts the STT service along with voice icon animation
    /// </summary>
    public void GetSpeechToText () {
        dictationRecognizer.DictationResult += (text, confidence) => {
            Debug.Log("The result of STT is : " + text + ", " + confidence);
            dictationResult = text;
        };
        dictationRecognizer.Start();
        voiceIcon.color = Color.red;
        voiceIconAnimator.enabled = true;
    }

    /// <summary>
    /// This method stops the STT service
    /// </summary>
    public void StopSpeechToText () {
        voiceIconAnimator.enabled = false;
        voiceIcon.color = Color.red;
        dictationRecognizer.Stop();
    }

    /// <summary>
    /// This method stops the STT service upon closing of the application
    /// </summary>
    private void OnApplicationQuit () {
        if (dictationRecognizer.Status == SpeechSystemStatus.Running) { 
            dictationRecognizer.Stop();
        }
    }
}
