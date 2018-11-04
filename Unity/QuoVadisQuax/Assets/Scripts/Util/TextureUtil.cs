﻿using System;
using UnityEngine;

public static class TextureUtil
{
    public static void DrawSquare(this Texture2D texture, Vector2Int bottomLeft, int width, Color32 color,
        Action callback = null)
    {
        var startPoint = new Vector2Int(Mathf.Clamp(bottomLeft.x, 0, texture.width),
            Mathf.Clamp(bottomLeft.y, 0, texture.height));

        var offset = new Vector2Int(Mathf.Abs(startPoint.x - bottomLeft.x), Mathf.Abs(startPoint.y - bottomLeft.y));

        var dimensions = new Vector2Int(Mathf.Clamp(width - offset.x, 0, texture.width - startPoint.x),
            Mathf.Clamp(width - offset.y, 0, texture.height - startPoint.y));

        var pixels = texture.GetPixels(startPoint.x, startPoint.y, dimensions.x, dimensions.y).ToColor32();

        Action applyPixels = () =>
        {
            texture.SetPixels(startPoint.x, startPoint.y, dimensions.x, dimensions.y, pixels.ToColor());
            texture.Apply();
        };

        ThreadQueuer.Instance.StartThreadedAction(() => { ColorPixels(ref pixels, color, applyPixels, callback); });
    }

    //public static void DrawCircle(ref Texture2D texture, int centerX, int centerY, int r, Color color)
    //{
    //    int x, y, px, nx, py, ny, d;
    //    var width = texture.width;
    //    var pixels = texture.GetPixels32();
    //    var size = pixels.Length;

    //    for (x = 0; x <= r; x++)
    //    {
    //        d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
    //        for (y = 0; y <= d; y++)
    //        {
    //            px = centerX + x;
    //            nx = centerX - x;
    //            py = centerY + y;
    //            ny = centerY - y;

    //            var v1 = py * width + px;
    //            var v2 = py * width + nx;
    //            var v3 = ny * width + px;
    //            var v4 = ny * width + nx;

    //            Debug.Log(v1 + ", " + v2 + ", " + v3 + ", " + v4 + " | " + size);

    //            if (v1 < size && v1 >= 0) pixels[v1] = color;
    //            if (v2 < size && v2 >= 0) pixels[v2] = color;
    //            if (v3 < size && v3 >= 0) pixels[v3] = color;
    //            if (v4 < size && v4 >= 0) pixels[v4] = color;
    //        }
    //    }
    //    texture.SetPixels32(pixels);
    //    texture.Apply();
    //}

    public static void ClearTexture(this Texture2D texture, Action callback = null)
    {
        var pixels = texture.GetPixels32();

        Action applyPixels = () =>
        {
            texture.SetPixels32(pixels);
            texture.Apply();
        };

        ThreadQueuer.Instance.StartThreadedAction(
            () => { ColorPixels(ref pixels, Color.clear, applyPixels, callback); });
    }

    public static void ColorPixels(ref Color32[] pixels, Color32 color, Action applyPixels, Action callback = null)
    {
        for (var i = 0; i < pixels.Length; i++) pixels[i] = color;

        if (applyPixels != null)
            ThreadQueuer.Instance.QueueMainThreadAction(applyPixels);
        else
            Debug.LogWarning("No applyPixels Action defined! Colored pixels won't be applied to texture!");

        if (callback != null)
            ThreadQueuer.Instance.QueueMainThreadAction(callback);
    }

    public static Color32[] ToColor32(this Color[] colors)
    {
        var colors32 = new Color32[colors.Length];

        for (var i = 0; i < colors.Length; i++) colors32[i] = colors[i];

        return colors32;
    }

    public static Color[] ToColor(this Color32[] colors32)
    {
        var colors = new Color[colors32.Length];

        for (var i = 0; i < colors32.Length; i++) colors[i] = colors32[i];

        return colors;
    }

    public static Color32 ToColor32(this Color color)
    {
        var color32 = new Color32[1];
        color32[0] = color;
        return color32[0];
    }
}