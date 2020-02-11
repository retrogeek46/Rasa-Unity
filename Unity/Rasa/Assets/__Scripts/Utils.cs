using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureExtentions {
    public static Texture2D ToTexture2D (this Texture texture) {
        Texture2D tex2D = new Texture2D(
            texture.width,
            texture.height,
            TextureFormat.RGBA32,
            false);

        //RenderTexture current = RenderTexture.active;
        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        //RenderTexture.active = renderTexture;
        tex2D.ReadPixels(new Rect(
            0,
            0,
            renderTexture.width,
            renderTexture.height),
            0,
            0);
        tex2D.Apply();
        return tex2D;

        //return Texture2D.CreateExternalTexture(
        //    texture.width,
        //    texture.height,
        //    TextureFormat.RGB24,
        //    false, false,
        //    texture.GetNativeTexturePtr());
    }
}

public class Utils {
    
}
