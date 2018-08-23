﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Crosstales.FB;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the LoadImage GUI
/// </summary>
public class LoadImageManager : MonoBehaviour
{
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    [DllImport("user32.dll")] static extern uint GetActiveWindow();
    [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
#endif

    public delegate void LoadingImageEventHandler(LoadingState state);

    public enum LoadingState
    {
        NOT_LOADING,
        LOADING,
        FAILED,
        DONE
    }

    private IntPtr _hWndUnity;
    /// <summary>
    /// Is the LoadImage GUI open
    /// </summary>
    public bool IsOpen { get; private set; }
    /// <summary>
    /// The world position when the LoadImage GUI is open
    /// </summary>
    private Vector3 _openPos;

    [SerializeField] private Text _imageDimensionsText;
    [SerializeField] private Text _filePathText;
    [SerializeField] private GameObject _toggleIcon;
    /// <summary>
    /// The map texture
    /// </summary>
    public Texture2D MapTexture { get; private set; }
    public event LoadingImageEventHandler UpdatedLoadingState;
    private bool _isCheckingMap;
    private bool _isMapValid;
    private bool _isProcessingImg;

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
            UpdatedLoadingState.Invoke(LoadingState.NOT_LOADING);
    }

    private void Update()
    {
        if (_isProcessingImg && !_isCheckingMap)
        {
            if (_isMapValid)
            {
                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.DONE);
            }
            else
            {
                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.FAILED);
            }

            _isProcessingImg = false;
            _isMapValid = false;
        }
    }

    /// <summary>
    /// Load an selected image file
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
    /// Toggles the LoadImage GUI
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
    /// Processes a selected image file
    /// </summary>
    /// <param name="imagePath">The file path</param>
    private void ProcessImage(string imagePath)
    {
        if (File.Exists(imagePath))
        {
            if (UpdatedLoadingState != null)
                UpdatedLoadingState.Invoke(LoadingState.LOADING);

            // Create map texture
            var imageData = File.ReadAllBytes(imagePath);
            MapTexture = new Texture2D(2, 2) { filterMode = FilterMode.Point };
            MapTexture.LoadImage(imageData);

            // Check if map is valid and search city and quax positions
            var pixels = MapTexture.GetPixels();
            var width = MapTexture.width;
            ThreadQueuer.Instance.StartThreadedAction(() => { CheckMapPixels(pixels, width); });

            // Set GUI text
            _imageDimensionsText.text = MapTexture.width + "x" + MapTexture.height;
            _filePathText.text = imagePath;

            // Update the MapDataManager instance
            MapDataManager.Instance.Dimensions = new Vector2(MapTexture.width, MapTexture.height);
        }
        else
        {
            Debug.LogError("File " + imagePath + " not found!");
            if (UpdatedLoadingState != null)
                UpdatedLoadingState.Invoke(LoadingState.FAILED);
        }
    }

    /// <summary>
    /// Checks if the map only contains black, white, red or green pixels and
    /// searches for city and quax positions
    /// </summary>
    /// <param name="pixels">The image pixels starting bottom left</param>
    /// <param name="width">The image width</param>
    private void CheckMapPixels(Color[] pixels, int width)
    {
        var error = false;
        MapDataManager.Instance.QuaxPositions.Clear();

        for (var i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] == Color.green)
            {
                // Stadt
                MapDataManager.Instance.CityPosition = IndexToMapPos(i, width);
            }
            else if (pixels[i] == Color.red)
            {
                // Quax
                MapDataManager.Instance.QuaxPositions.Add(IndexToMapPos(i, width));
            }
            else if (pixels[i] != Color.black && pixels[i] != Color.white)
            {
                // ERROR!
                Debug.LogError("Unexpected pixel color " + pixels[i] + " in map at " + IndexToMapPos(i, width));
                error = true;
                break;
            }
        }

        _isMapValid = !error;
        _isCheckingMap = false;
    }

    /// <summary>
    /// Converts an pixel index (starting bottom left) to a 2D position
    /// </summary>
    /// <param name="i">The index of the pixel</param>
    /// <param name="width">The width of the image</param>
    /// <returns></returns>
    private Vector2 IndexToMapPos(int i, int width)
    {
        // ReSharper disable once PossibleLossOfFraction
        return new Vector2(i % width, i / width);
    }
}