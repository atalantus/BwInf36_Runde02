﻿using System;
using System.IO;
using Algorithm.Pathfinding;
using Algorithm.Quadtree;
using Crosstales.FB;
using UnityEngine;
using UnityEngine.UI;
using Grid = Algorithm.Pathfinding.Grid;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

/// <summary>
///     Manages the LoadImage GUI
/// </summary>
public class LoadImageManager : MonoBehaviour
{
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    [DllImport("user32.dll")] static extern uint GetActiveWindow();
    [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
#endif

    #region Properties

    public delegate void LoadingImageEventHandler(LoadingState state);

    public event LoadingImageEventHandler UpdatedLoadingState;

    public enum LoadingState
    {
        NotLoading,
        Loading,
        Failed,
        Done
    }

    private IntPtr _hWndUnity;

    /// <summary>
    ///     Is the LoadImage GUI open
    /// </summary>
    private bool IsOpen { get; set; }

    /// <summary>
    ///     The world position when the LoadImage GUI is open
    /// </summary>
    private Vector3 _openPos;

    [SerializeField] private PathfindingManager _pathfindingManager;
    [SerializeField] private Text _imageDimensionsText;
    [SerializeField] private Text _filePathText;
    [SerializeField] private GameObject _toggleIcon;

    /// <summary>
    ///     The map texture
    /// </summary>
    public Texture2D MapTexture { get; private set; }


    private bool _isCheckingMap;
    private bool _isMapValid;
    private bool _isProcessingImg;

    #endregion

    #region Methods

    private void Awake()
    {
        IsOpen = true;
        _openPos = transform.position;
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        _hWndUnity = (IntPtr)GetActiveWindow();
#endif
    }

    private void Start()
    {
        if (UpdatedLoadingState != null)
            UpdatedLoadingState.Invoke(LoadingState.NotLoading);
    }

    private void Update()
    {
        if (_isProcessingImg && !_isCheckingMap)
        {
            if (_isMapValid)
            {
                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.Done);
            }
            else
            {
                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.Failed);
            }

            _isProcessingImg = false;

            _isMapValid = false;
        }
    }

    /// <summary>
    ///     Load an selected image file
    /// </summary>
    public void OpenFile()
    {
#if CT_FB
        // Open the file browser
        var path = FileBrowser.OpenSingleFile("Open Quax MapTexture", "", "png");
        if (path != string.Empty)
        {
            _isCheckingMap = true;
            _isProcessingImg = true;

            ProcessImage(path);
        }
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        SetForegroundWindow(_hWndUnity);
#endif
#else
        Debug.LogWarning("File Browser NOT installed!");
#endif
    }

    /// <summary>
    ///     Toggles the LoadImage GUI
    /// </summary>
    public void ToggleGUI()
    {
        var offset = 0;
        if (IsOpen) offset = 50;
        var target = new Vector3(_openPos.x, _openPos.y + offset, _openPos.z);
        iTween.MoveTo(gameObject, target, .5f);
        iTween.RotateBy(_toggleIcon, new Vector3(0, 0, .5f), .5f);
        IsOpen = !IsOpen;
    }

    /// <summary>
    ///     Processes a selected image file
    /// </summary>
    /// <param name="imagePath">The file path</param>
    private void ProcessImage(string imagePath)
    {
        try
        {
            if (File.Exists(imagePath))
            {
                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.Loading);

                // Create map texture
                var imageData = File.ReadAllBytes(imagePath);
                MapTexture = new Texture2D(2, 2) {filterMode = FilterMode.Point};
                MapTexture.LoadImage(imageData);

                var imgWidth = MapTexture.width;
                var imgHeight = MapTexture.height;
                if (imgHeight > imgWidth) throw new Exception("Flip image (width must be larger or same as height)");
                var mapSize = Mathf.Max(imgWidth, imgHeight);

                // Check if map is valid and search city and quax positions
                var pixels = MapTexture.GetPixels32();
                _pathfindingManager.PathfindingGrid = new Grid(imgWidth, imgHeight);
                ThreadQueuer.Instance.StartThreadedAction(() => { CheckMapPixels(pixels, imgWidth); });

                // Resize map
                MapTexture.Resize(mapSize, mapSize);
                MapTexture.SetPixels32(0, 0, imgWidth, imgHeight, pixels);
                MapTexture.Apply();

                MapDataManager.Instance.MapTexture = MapTexture;

                if (MapTexture.width <= 2 && MapTexture.height <= 2)
                    throw new Exception("Image is to small");

                // Set GUI text
                _imageDimensionsText.text = imgWidth + "x" + imgHeight;
                _filePathText.text = imagePath;

                // Update the MapDataManager instance
                MapDataManager.Instance.Dimensions = new Vector2Int(MapTexture.width, MapTexture.height);
            }
            else
            {
                throw new FileNotFoundException("File " + imagePath + " not found!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            if (UpdatedLoadingState != null)
                UpdatedLoadingState.Invoke(LoadingState.Failed);
        }
    }

    /// <summary>
    ///     Checks if the map only contains black, white, red or green pixels and
    ///     searches for city and quax positions
    /// </summary>
    /// <param name="pixels">The image pixels starting bottom left</param>
    /// <param name="width">The image width</param>
    private void CheckMapPixels(Color32[] pixels, int width)
    {
        var error = false;
        MapDataManager.Instance.QuaxPositions.Clear();

        for (var i = 0; i < pixels.Length; i++)
        {
            var type = pixels[i].GetMapType();

            switch (type)
            {
                case MapTypes.Quax:
                    MapDataManager.Instance.QuaxPositions.Add(IndexToMapPos(i, width));
                    break;
                case MapTypes.City:
                    MapDataManager.Instance.CityPosition = IndexToMapPos(i, width);
                    break;
                case MapTypes.Unknown:
                    Debug.LogError("Unexpected pixel color " + pixels[i] + " in map at " + IndexToMapPos(i, width) +
                                   "\nMaybe increase the ColorFilterThreshold!");
                    error = true;
                    break;
                case MapTypes.Water:
                    break;
                case MapTypes.Ground:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (error) break;
        }

        _isMapValid = !error;
        _isCheckingMap = false;
    }

    /// <summary>
    ///     Converts an pixel index (starting bottom left) to a 2D position
    /// </summary>
    /// <param name="i">The index of the pixel</param>
    /// <param name="width">The width of the image</param>
    /// <returns></returns>
    private Vector2Int IndexToMapPos(int i, int width)
    {
        return new Vector2Int(i % width, i / width);
    }

    #endregion
}