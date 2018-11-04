using Algorithm.Quadtree;
using UnityEngine;

public static class MapColors
{
    // WARNING:
    // Adjusting the Threshold might lead to pixels outside of the map (RGBA 0.804,0.804,0.804,0.804) not getting recognized as water!
    public static readonly int ColorFilterThreshold = 50;

    public static MapTypes GetMapType(this Color32 color)
    {
        if (color.r >= 255 - ColorFilterThreshold && color.g <= ColorFilterThreshold && color.b <= ColorFilterThreshold)
            return MapTypes.Quax;
        if (color.r <= ColorFilterThreshold && color.g >= 255 - ColorFilterThreshold && color.b <= ColorFilterThreshold)
            return MapTypes.City;
        if (color.r >= 255 - ColorFilterThreshold && color.g >= 255 - ColorFilterThreshold &&
            color.b >= 255 - ColorFilterThreshold)
            return MapTypes.Water;
        if (color.r <= ColorFilterThreshold && color.g <= ColorFilterThreshold && color.b <= ColorFilterThreshold)
            return MapTypes.Ground;
        return MapTypes.Unknown;
    }

    public static MapTypes GetMapType(this Color colorA)
    {
        return GetMapType(colorA.ToColor32());
    }
}