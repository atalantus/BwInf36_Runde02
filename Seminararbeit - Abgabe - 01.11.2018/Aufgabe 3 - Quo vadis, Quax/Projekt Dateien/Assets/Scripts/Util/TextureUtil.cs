using System;
using UnityEngine;

/// <summary>
///     Utility methods for working with textures
/// </summary>
public static class TextureUtil
{
    /// <summary>
    ///     Draws a square on a texture and automatically applies the changes
    /// </summary>
    /// <param name="texture">The texture to draw on</param>
    /// <param name="bottomLeft">The coordinates of the bottom left corner of the square</param>
    /// <param name="width">The width of the square</param>
    /// <param name="color">The fill color of the square</param>
    /// <param name="callback">The callback method</param>
    public static void DrawSquare(this Texture2D texture, Vector2Int bottomLeft, int width, Color32 color,
        Action callback = null)
    {
        var startPoint = new Vector2Int(Mathf.Clamp(bottomLeft.X, 0, texture.width),
            Mathf.Clamp(bottomLeft.Y, 0, texture.height));

        var offset = new Vector2Int(Mathf.Abs(startPoint.X - bottomLeft.X), Mathf.Abs(startPoint.Y - bottomLeft.Y));

        var dimensions = new Vector2Int(Mathf.Clamp(width - offset.X, 0, texture.width - startPoint.X),
            Mathf.Clamp(width - offset.Y, 0, texture.height - startPoint.Y));

        var pixels = texture.GetPixels(startPoint.X, startPoint.Y, dimensions.X, dimensions.Y).ToColor32();

        Action applyPixels = () =>
        {
            texture.SetPixels(startPoint.X, startPoint.Y, dimensions.X, dimensions.Y, pixels.ToColor());
            texture.Apply();
        };

        ThreadQueuer.Instance.StartThreadedAction(() => { ColorPixels(ref pixels, color, applyPixels, callback); });
    }

    /// <summary>
    ///     Clears a texture
    /// </summary>
    /// <param name="texture">The texture to clear</param>
    /// <param name="callback">The callback method</param>
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

    /// <summary>
    ///     Colors an array of pixels with a given color
    /// </summary>
    /// <param name="pixels">The pixels to color</param>
    /// <param name="color">The color to color the pixels</param>
    /// <param name="applyPixels">The method for applying the colored pixels to some texture</param>
    /// <param name="callback">The callback method</param>
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

    /// <summary>
    ///     Convert a <see cref="Color" /> array to a <see cref="Color32" /> array
    /// </summary>
    /// <param name="colors">The <see cref="Color" /> array</param>
    /// <returns>The converted <see cref="Color32" /> array</returns>
    public static Color32[] ToColor32(this Color[] colors)
    {
        var colors32 = new Color32[colors.Length];

        for (var i = 0; i < colors.Length; i++) colors32[i] = colors[i];

        return colors32;
    }

    /// <summary>
    ///     Convert a <see cref="Color32" /> array to a <see cref="Color" /> array
    /// </summary>
    /// <param name="colors32">The <see cref="Color32" /> array</param>
    /// <returns>The converted <see cref="Color" /> array</returns>
    public static Color[] ToColor(this Color32[] colors32)
    {
        var colors = new Color[colors32.Length];

        for (var i = 0; i < colors32.Length; i++) colors[i] = colors32[i];

        return colors;
    }

    /// <summary>
    ///     Convert a <see cref="Color" /> to a <see cref="Color32" />
    /// </summary>
    /// <param name="color">The <see cref="Color" /> to convert</param>
    /// <returns>The converted <see cref="Color32" /></returns>
    public static Color32 ToColor32(this Color color)
    {
        var color32 = new Color32[1];
        color32[0] = color;
        return color32[0];
    }
}