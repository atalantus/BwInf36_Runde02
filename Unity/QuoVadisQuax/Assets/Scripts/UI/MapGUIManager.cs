using System;
using System.Collections.Generic;
using System.Threading;
using Algorithm;
using Algorithm.Pathfinding;
using Algorithm.Quadtree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Node = Algorithm.Pathfinding.Node;

/// <summary>
///     Manages the Map GUI
/// </summary>
public class MapGUIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static readonly string COLORING_OVERLAY_MSG_ID = "coloring_overlay";
    [SerializeField] private AlgorithmManager _algorithmManager;
    private Thread _clearOverlayTextureThread;

    private Queue<Action> _colorSquareOverlayActions;
    [SerializeField] private ContainerManager _containerManager;
    private float _defaultZoomLevel;

    private bool _hasFocus;
    private bool _isColoringOverlayPixels;
    private bool _isColoringSquareOverlay;

    /// <summary>
    ///     Container for the map and the overlay
    ///     This gets moved and scaled for user interaction
    /// </summary>
    [SerializeField] private RectTransform _mapContainer;

    [SerializeField] private RawImage _mapRawImage;
    private float _maxZoomLevel;
    private float _minZoomLevel;
    [SerializeField] private OptionsManager _optionsManager;
    [SerializeField] private RawImage _overlayRawImage;

    private Texture2D _overlayTexture;
    private List<Node> _path;
    private bool _readyToColorPath;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hasFocus = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hasFocus = false;
    }

    private void Start()
    {
        _colorSquareOverlayActions = new Queue<Action>();

        _optionsManager.StartedAlgorithm += SetUpOverlayTexture;
        PathfindingManager.Instance.FinishedPathfinding += (path, foundPath) =>
        {
            //Debug.LogWarning("MapGUIManager - ColorPath");

            _path = foundPath ? path : new List<Node>();

            _readyToColorPath = true;
        };
        QuadtreeManager.Instance.CreatedNode += ColorQuadtreeNode;
    }

    private void ColorQuadtreeNode(MapSquare mapSquare)
    {
        if (!_optionsManager.ShowNodes) return;

        Color32 color;

        switch (mapSquare.MapType)
        {
            case MapTypes.Water:
                color = new Color32(255, 0, 0, 100);
                break;
            case MapTypes.Ground:
                color = new Color32(0, 255, 0, 100);
                break;
            case MapTypes.Mixed:
                color = new Color32(200, 150, 50, 50);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //Debug.Log("Queue: SW " + mapSquare.SW_Point + " NE " + mapSquare.NE_Point);

        _colorSquareOverlayActions.Enqueue(() =>
        {
            _overlayTexture.DrawSquare(mapSquare.SW_Point, mapSquare.Width, color, () =>
            {
                _colorSquareOverlayActions.Dequeue();
                _isColoringSquareOverlay = false;
            });
        });
    }

    private void ColorPath()
    {
        for (var index = 0; index < _path.Count - 1; index++)
        {
            var node = _path[index];
            int posX, posY;
            posX = node.Position.x;
            posY = node.Position.y;

            ThreadQueuer.Instance.QueueMainThreadActionMultiple(() =>
            {
                _overlayTexture.SetPixel(posX, posY, Color.magenta);
            });
        }

        ThreadQueuer.Instance.QueueMainThreadActionMultiple(() =>
        {
            //Debug.LogWarning("----- APPLY -----");
            _overlayTexture.Apply();
            _algorithmManager.FinishAlgorithm();
        });

        //Debug.Log("Colored Path");
    }

    private void SetUpOverlayTexture(Vector2Int quaxPos, Vector2Int cityPos)
    {
        _overlayTexture.ClearTexture(() => _algorithmManager.StartAlgorithm(quaxPos, cityPos));
    }

    /// <summary>
    ///     Sets up the map-container, the map and the overlay object
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
            new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false) {filterMode = FilterMode.Point};

        _overlayRawImage.texture = _overlayTexture;

        _containerManager.CreateMessage("Coloring overlay texture...", COLORING_OVERLAY_MSG_ID, true);

        _overlayTexture.ClearTexture(() => _containerManager.DestroyMessage(COLORING_OVERLAY_MSG_ID));
    }

    private void Update()
    {
        // Execute overlay stuff things
        if (!_isColoringSquareOverlay)
        {
            if (_colorSquareOverlayActions.Count > 0)
            {
                _isColoringSquareOverlay = true;
                _colorSquareOverlayActions.Peek()();
            }
            else if (_readyToColorPath)
            {
                _readyToColorPath = false;
                ThreadQueuer.Instance.StartThreadedAction(ColorPath);
            }
        }

        /**
         * Check for scroll wheel input
         */
        var scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta != 0f)
        {
            if (!_hasFocus) return;

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
                _mapContainer.transform.localPosition -= _mapContainer.transform.localPosition / 5;

                // Clamp to min zoom level
                if (_mapContainer.transform.localScale.x < _minZoomLevel)
                    _mapContainer.transform.localScale = new Vector3(_minZoomLevel, _minZoomLevel, _minZoomLevel);
            }
        }
    }

    /// <summary>
    ///     Calculates the default, max and min zoom level based on the map dimensions
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
}