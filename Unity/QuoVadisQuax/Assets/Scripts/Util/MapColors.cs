using System.Collections;
using System.Collections.Generic;
using Algorithm.Quadtree;
using UnityEngine;

public static class MapColors
{
    public static readonly int ColorFilterThreshold = 50;

    public static MapTypes GetMapType(this Color32 color)
    {
        if (color.r >= 255 - ColorFilterThreshold && color.g <= ColorFilterThreshold && color.b <= ColorFilterThreshold)
            return MapTypes.QUAX;
        if (color.r <= ColorFilterThreshold && color.g >= 255 - ColorFilterThreshold && color.b <= ColorFilterThreshold)
            return MapTypes.CITY;
        if (color.r >= 255 - ColorFilterThreshold && color.g >= 255 - ColorFilterThreshold && color.b >= 255 - ColorFilterThreshold)
            return MapTypes.WATER;
        if (color.r <= ColorFilterThreshold && color.g <= ColorFilterThreshold && color.b <= ColorFilterThreshold)
            return MapTypes.GROUND;
        return MapTypes.NONE;
    }
    
    public static MapTypes GetMapType(this Color colorA)
    {
        return GetMapType(colorA.ToColor32());
    }
}
