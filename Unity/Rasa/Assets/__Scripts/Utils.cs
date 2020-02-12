using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// This code sample has been taken from http://answers.unity.com/answers/1292514/view.html
/// <summary>
/// This class extends the inbuilt Texture class
/// </summary>
public static class TextureExtentions {
    /// <summary>
    /// This methods creates a Texture2D from Texture
    /// </summary>
    /// <param name="texture">the Texture to be converted</param>
    /// <returns>The Texture2D create</returns>
    public static Texture2D ToTexture2D (this Texture texture) {
        // Create a texture2d with appropriate dimensions
        Texture2D tex2D = new Texture2D(
            texture.width,
            texture.height,
            TextureFormat.RGBA32,
            false);

        // Create renderTexture to get pixel data from texture
        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32);
		Graphics.Blit(texture, renderTexture);

        // Get pixel data from the render texture and apply to the texture2D created above 
        tex2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex2D.Apply();
		
		// Release the render texture
		RenderTexture.active = null;
		renderTexture.Release();
		return tex2D;
    }
}

// This code sample has been taken from  http://wiki.unity3d.com/index.php/TextureScale
// Only works on ARGB32, RGB24 and Alpha8 textures that are marked readable
public class TextureScale {
    public class ThreadData {
        public int start;
        public int end;
        public ThreadData (int s, int e) {
            start = s;
            end = e;
        }
    }

    private static Color32[] texColors;
    private static Color32[] newColors;
    private static int w;
    private static float ratioX;
    private static float ratioY;
    private static int w2;
    private static int finishCount;
    private static Mutex mutex;

    public static void Point (Texture2D tex, int newWidth, int newHeight) {
        ThreadedScale(tex, newWidth, newHeight, false);
    }

    public static void Bilinear (Texture2D tex, int newWidth, int newHeight) {
        ThreadedScale(tex, newWidth, newHeight, true);
    }

    private static void ThreadedScale (Texture2D tex, int newWidth, int newHeight, bool useBilinear) {
        texColors = tex.GetPixels32();
        newColors = new Color32[newWidth * newHeight];
        if (useBilinear) {
            ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
            ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
        } else {
            ratioX = ((float)tex.width) / newWidth;
            ratioY = ((float)tex.height) / newHeight;
        }
        w = tex.width;
        w2 = newWidth;
        var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
        var slice = newHeight/cores;

        finishCount = 0;
        if (mutex == null) {
            mutex = new Mutex(false);
        }
        if (cores > 1) {
            int i = 0;
            ThreadData threadData;
            for (i = 0; i < cores - 1; i++) {
                threadData = new ThreadData(slice * i, slice * (i + 1));
                ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
                Thread thread = new Thread(ts);
                thread.Start(threadData);
            }
            threadData = new ThreadData(slice * i, newHeight);
            if (useBilinear) {
                BilinearScale(threadData);
            } else {
                PointScale(threadData);
            }
            while (finishCount < cores) {
                Thread.Sleep(1);
            }
        } else {
            ThreadData threadData = new ThreadData(0, newHeight);
            if (useBilinear) {
                BilinearScale(threadData);
            } else {
                PointScale(threadData);
            }
        }

        tex.Resize(newWidth, newHeight);
        tex.SetPixels32(newColors);
        tex.Apply();

        texColors = null;
        newColors = null;
    }

    public static void BilinearScale (System.Object obj) {
        ThreadData threadData = (ThreadData) obj;
        for (var y = threadData.start; y < threadData.end; y++) {
            int yFloor = (int)Mathf.Floor(y * ratioY);
            var y1 = yFloor * w;
            var y2 = (yFloor+1) * w;
            var yw = y * w2;

            for (var x = 0; x < w2; x++) {
                int xFloor = (int)Mathf.Floor(x * ratioX);
                var xLerp = x * ratioX-xFloor;
                newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                                                       ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                                                       y * ratioY - yFloor);
            }
        }

        mutex.WaitOne();
        finishCount++;
        mutex.ReleaseMutex();
    }

    public static void PointScale (System.Object obj) {
        ThreadData threadData = (ThreadData) obj;
        for (var y = threadData.start; y < threadData.end; y++) {
            var thisY = (int)(ratioY * y) * w;
            var yw = y * w2;
            for (var x = 0; x < w2; x++) {
                newColors[yw + x] = texColors[(int)(thisY + ratioX * x)];
            }
        }

        mutex.WaitOne();
        finishCount++;
        mutex.ReleaseMutex();
    }

    private static Color ColorLerpUnclamped (Color c1, Color c2, float value) {
        return new Color(c1.r + (c2.r - c1.r) * value,
                          c1.g + (c2.g - c1.g) * value,
                          c1.b + (c2.b - c1.b) * value,
                          c1.a + (c2.a - c1.a) * value);
    }
}

/// <summary>
/// This class is used to serialize users message into a json
/// object which can be sent over http request to the bot.
/// </summary>
public struct PostData {
	public string message;
	public string sender;
}

/// <summary>
/// This class is used to deserialize the resonse json for each
/// individual message.
/// </summary>
[Serializable]
public class RecieveData {
	public string recipient_id;
	public string text;
	public string image;
	public string attachemnt;
	public string button;
	public string element;
	public string quick_replie;
}

/// <summary>
/// This class is a wrapper for individual messages sent by the bot.
/// </summary>
[Serializable]
public class RootMessages {
	public RecieveData[] messages;
}
