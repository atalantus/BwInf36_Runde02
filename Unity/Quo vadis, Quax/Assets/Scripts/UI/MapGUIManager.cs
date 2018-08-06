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
    [SerializeField] private GameObject _messageContainer;
    [SerializeField] private Text _messageText;

    private float _defaultZoomLevel;
    private float _maxZoomLevel;
    private float _minZoomLevel;

    public void SetMap(Texture2D texture)
    {
        var dimensions = new Vector2(texture.width, texture.height);
        CalculateZoomLevels(dimensions);
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

        _overlayTexture.Apply();
        _mapOverlay.texture = _overlayTexture;
    }

    private void Update()
    {
        var scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0f)
        {
            scrollDelta = Mathf.Clamp(scrollDelta, -0.15f, 0.15f);
            //var mousePos = Input.mousePosition;
            //mousePos.x -= Screen.width / 2;
            //mousePos.y -= Screen.height / 2;
            var zoomDelta = Mathf.Abs(scrollDelta * 650 * Time.deltaTime);

            if (scrollDelta > 0f)
            {
                _mapContainer.transform.localScale *= zoomDelta;
                // TODO: Zoom image to current mouse position
                //_mapContainer.transform.localPosition -= (mousePos / 4f);

                if (_mapContainer.transform.localScale.x > _maxZoomLevel)
                    _mapContainer.transform.localScale = new Vector3(_maxZoomLevel, _maxZoomLevel, _maxZoomLevel);
            }
            else
            {
                _mapContainer.transform.localScale /= zoomDelta;
                _mapContainer.transform.localPosition -= (_mapContainer.transform.localPosition / 5);

                if (_mapContainer.transform.localScale.x < _minZoomLevel)
                    _mapContainer.transform.localScale = new Vector3(_minZoomLevel, _minZoomLevel, _minZoomLevel);
            }
        }
    }

    private void CalculateZoomLevels(Vector2 dimensions)
    {
        var x = Mathf.Max(dimensions.x, dimensions.y);
        var y = Mathf.Min(Screen.width, Screen.height);

        var z = y / (x * 1.25f);

        _defaultZoomLevel = z;
        _maxZoomLevel = z * x * 0.15f;
        _minZoomLevel = z / 3;
    }
}
