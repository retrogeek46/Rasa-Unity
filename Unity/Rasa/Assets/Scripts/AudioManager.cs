using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // singleton
    private static AudioManager _instance = null;   // singleton object for the Audio Manager
    private AudioSource         themeAudioSource;   // audio source which will play the audio

    public AudioClip            theme;              // reference to the theme audio clip

    void Awake () {
        if (_instance != null) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        themeAudioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void Start () {
        PlayTheme(theme);
    }

    public void PlayTheme (AudioClip clip) {
        themeAudioSource.clip = clip;
        themeAudioSource.loop = true;
        themeAudioSource.Play();
    }
}
