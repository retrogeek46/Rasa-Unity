using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class enables a scrolling background animation
/// </summary>
public class BackgroundScroll : MonoBehaviour {

    public float scrollSpeed = 0.5f;
    public Renderer bgRenderer;

    // Update is called once per frame
    void Update () {
        Vector2 offset = new Vector2(Time.time * -scrollSpeed, 0);
        bgRenderer.material.mainTextureOffset = offset;
        
    }
}
