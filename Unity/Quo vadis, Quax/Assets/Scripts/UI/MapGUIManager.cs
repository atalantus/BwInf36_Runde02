using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform _mapContainer;
    [SerializeField] private RawImage _mapRawImage;
    [SerializeField] private RawImage _mapOverlay;
    private Texture2D _overlayTexture;

    private float _defaultZoomLevel;

    public void SetMap(Texture2D texture)
    {
        var dimensions = new Vector2(texture.width, texture.height);
        CalculateDefaultZoomLevel(dimensions);
        _mapContainer.sizeDelta = dimensions;
        _mapContainer.localScale = new Vector3(_defaultZoomLevel, _defaultZoomLevel, _defaultZoomLevel);

        _mapRawImage.texture = texture;

        _overlayTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        _overlayTexture.filterMode = FilterMode.Point;

        var fillColor = Color.clear;
        var fillPixels = new Color[texture.width * texture.height];

        for (int i = 0; i < fillPixels.Length; i++)
        {
            fillPixels[i] = fillColor;
        }

        _overlayTexture.SetPixels(fillPixels);

        _mapOverlay.texture = _overlayTexture;
    }

    private void Update()
    {

    }

    private void CalculateDefaultZoomLevel(Vector2 dimensions)
    {
        var x = Mathf.Max(dimensions.x, dimensions.y);
        var y = Mathf.Min(Screen.width, Screen.height);

        var z = y / (x * 1.25f);

        _defaultZoomLevel = z;
    }
}
