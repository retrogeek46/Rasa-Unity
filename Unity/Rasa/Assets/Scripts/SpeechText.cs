using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechText : MonoBehaviour {

    KeywordRecognizer keywordRecognizer;
    public string[] keywordsArray;

    // Start is called before the first frame update
    void Start () {
        keywordsArray = new string[2];
        keywordsArray[0] = "hello";
        keywordsArray[1] = "how are you";

        keywordRecognizer = new KeywordRecognizer(keywordsArray);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized (PhraseRecognizedEventArgs args) {
        Debug.Log("Keyword: " + args.text + "; Confidence: " + args.confidence + "; Start Time: " + args.phraseStartTime + "; Duration: " + args.phraseDuration);
        // write your own logic
    }

    // Update is called once per frame
    void Update () {

    }
}
