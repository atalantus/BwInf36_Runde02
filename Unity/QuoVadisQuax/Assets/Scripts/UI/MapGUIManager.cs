﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

/// <summary>
/// Manages the Map GUI
/// </summary>
public class MapGUIManager : MonoBehaviour
{
    /// <summary>
    /// Container for the map and the overlay
    /// This gets moved and scaled for user interaction
    /// </summary>
    [SerializeField] private RectTransform _mapContainer;
    [SerializeField] private RawImage _mapRawImage;
    [SerializeField] private RawImage _overlayRawImage;
    [SerializeField] private ContainerManager _containerManager;

    private float _defaultZoomLevel;
    private float _maxZoomLevel;
    private float _minZoomLevel;

    private Texture2D _overlayTexture;
    private bool _isColoringOverlayPixels;
    private Color[] _overlayPixels;
    private Thread _clearOverlayTextureThread;

    public static readonly string COLORING_OVERLAY_MSG_ID = "coloring_overlay";

    /// <summary>
    /// Sets up the map-container, the map and the overlay object
    /// </summary>
    /// <param name="texture">The map texture</param>
    public void SetMap(Texture2D texture)
    {
        var dimensions = new Vector2(texture.width, texture.height);
        CalculateZoomLevels(dimensions);
        _mapContainer.sizeDelta = dimensions;
        _mapContainer.localScale = new Vector3(_defaultZoomLevel, _defaultZoomLevel, _defaultZoomLevel);

        _mapRawImage.texture = texture;

        // Create overlay texture
        _overlayTexture =
            new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false) { filterMode = FilterMode.Point };

        // Create overlay pixels ( way faster than _overlayTexture.getPixels() )
        _overlayPixels = new Color[texture.width * texture.height];
        _overlayRawImage.texture = _overlayTexture;

        _containerManager.CreateMessage("Coloring overlay texture...", COLORING_OVERLAY_MSG_ID, true);
        // Color overlay texture's pixels transparent
        // There can multiple millions of pixels to color --> threading
        ThreadQueuer.Instance.StartThreadedAction(() => { SetOverlayPixels(ref _overlayPixels, Color.clear); });
    }

    private void Update()
    {
        /**
         * Check for scroll wheel input
         */
        var scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta != 0f)
        {
            scrollDelta = Mathf.Clamp(scrollDelta, -0.15f, 0.15f);
            //var mousePos = Input.mousePosition;
            //mousePos.x -= Screen.width / 2;
            //mousePos.y -= Screen.height / 2;
            // Calculate zoom delta
            var zoomDelta = Mathf.Abs(scrollDelta * 650 * Time.deltaTime);

            if (scrollDelta > 0f)
            {
                // Zoom in
                _mapContainer.transform.localScale *= zoomDelta;

                // TODO: Zoom image to current mouse position
                //_mapContainer.transform.localPosition -= (mousePos / 4f);

                // Clamp to max zoom level
                if (_mapContainer.transform.localScale.x > _maxZoomLevel)
                    _mapContainer.transform.localScale = new Vector3(_maxZoomLevel, _maxZoomLevel, _maxZoomLevel);
            }
            else
            {
                // Zoom out
                _mapContainer.transform.localScale /= zoomDelta;
                // Center image when zooming out
                _mapContainer.transform.localPosition -= (_mapContainer.transform.localPosition / 5);

                // Clamp to min zoom level
                if (_mapContainer.transform.localScale.x < _minZoomLevel)
                    _mapContainer.transform.localScale = new Vector3(_minZoomLevel, _minZoomLevel, _minZoomLevel);
            }
        }
    }

    /// <summary>
    /// Calculates the default, max and min zoom level based on the map dimensions
    /// </summary>
    /// <param name="dimensions">The map dimensions</param>
    private void CalculateZoomLevels(Vector2 dimensions)
    {
        // Get biggest dimension of map
        var x = Mathf.Max(dimensions.x, dimensions.y);
        // Get biggest dimension of screen
        var y = Mathf.Min(Screen.width, Screen.height);

        // Calculate the default zoom level (The matches the whole screen with some padding)
        var z = y / (x * 1.25f);

        _defaultZoomLevel = z;
        // Calculate the maximum zoom level based on the default zoom level and the biggest map dimension
        // Makes every map zoomable up to the point where one image pixel covers x mm of the screen
        _maxZoomLevel = z * x * 0.15f;
        // Calculate the minimum zoom level
        _minZoomLevel = z / 3;
    }

    /// <summary>
    /// Colors all the pixels with a given color
    /// </summary>
    /// <param name="pixels">Pixels to color</param>
    /// <param name="fillColor">The new color of the pixels</param>
    private void SetOverlayPixels(ref Color[] pixels, Color fillColor)
    {
        Debug.Log(pixels.Length + " pixels to color");
        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i] = fillColor;
        }

        Action applyPixels = () =>
        {
            _overlayTexture.SetPixels(_overlayPixels);
            _overlayTexture.Apply();
            _containerManager.DestroyMessage(COLORING_OVERLAY_MSG_ID);
        };

        ThreadQueuer.Instance.QueueMainThreadAction(applyPixels);
    }
}