using System;
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
    private bool _isOpen;
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

    private void Awake()
    {
        _isOpen = true;
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
            // Check if selected image is a valid quax map
            if (ProcessImage(path))
            {
                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.DONE);
            }
            else
            {
                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.FAILED);
            }
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
    public void TogglePanel()
    {
        var offset = 0;
        if (_isOpen) offset = 50;
        var target = new Vector3(_openPos.x, _openPos.y + offset, _openPos.z);
        iTween.MoveTo(gameObject, target, .5f);
        iTween.RotateBy(_toggleIcon, new Vector3(0, 0, .5f), .5f);
        _isOpen = !_isOpen;
    }

    /// <summary>
    /// Processes a selected image file and checks if it's a valid quax map
    /// </summary>
    /// <param name="imagePath">The file path</param>
    /// <returns>True if the selected image it is a valid quax map</returns>
    private bool ProcessImage(string imagePath)
    {
        if (UpdatedLoadingState != null)
            UpdatedLoadingState.Invoke(LoadingState.LOADING);

        // TODO: Process Image
        // Check if it's a valid map (only black, white, green and red pixels)
        // if not return false

        if (File.Exists(imagePath))
        {
            // Create map texture
            var imageData = File.ReadAllBytes(imagePath);
            MapTexture = new Texture2D(2, 2) { filterMode = FilterMode.Point };
            MapTexture.LoadImage(imageData);

            // Set GUI text
            _imageDimensionsText.text = MapTexture.width + "x" + MapTexture.height;
            _filePathText.text = imagePath;

            // Update the MapDataManager instance
            MapDataManager.Instance.Dimensions = new Vector2(MapTexture.width, MapTexture.height);
        }
        return true;
    }
}
