using System.Collections;
using System.Collections.Generic;
using System.IO;
using Crosstales.FB;
using UnityEngine;
using UnityEngine.UI;

public class LoadImage : MonoBehaviour
{
    public delegate void LoadingMapEventHandler(LoadingState state);

    public enum LoadingState
    {
        NOT_LOADING,
        LOADING,
        FAILED,
        DONE
    }

    private bool _isOpen;
    private Vector3 _openPos;
    private Texture2D _mapTexture;
    [SerializeField] private Text _filePathText;
    [SerializeField] private GameObject _toggleIcon;
    public Texture2D MapTexture
    {
        get { return _mapTexture; }
    }
    public event LoadingMapEventHandler UpdatedLoadingState;

    private void Awake()
    {
        _isOpen = true;
        _openPos = transform.position;
    }

    private void Start()
    {
        if (UpdatedLoadingState != null)
            UpdatedLoadingState.Invoke(LoadingState.NOT_LOADING);
    }

    public void OpenFile()
    {
#if CT_FB
        var path = FileBrowser.OpenSingleFile("Open Quax MapTexture", "", "png");
        if (path != string.Empty)
        {
            if (ProcessImage(path))
            {
                _filePathText.text = path;

                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.DONE);
            }
            else
            {
                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.FAILED);
            }
        }
#else
        Debug.LogWarning("File Browser NOT installed!");
#endif
    }

    public void TogglePanel()
    {
        var offset = 0;
        if (_isOpen) offset = 50;
        var target = new Vector3(_openPos.x, _openPos.y + offset, _openPos.z);
        iTween.MoveTo(gameObject, target, .5f);
        iTween.RotateBy(_toggleIcon, new Vector3(0, 0, .5f), .5f);
        _isOpen = !_isOpen;
    }

    private bool ProcessImage(string imagePath)
    {
        if (UpdatedLoadingState != null)
            UpdatedLoadingState.Invoke(LoadingState.LOADING);

        // TODO: Process Image
        // Check if it's a valid map (only black, white, green and red pixels)

        if (File.Exists(imagePath))
        {
            var imageData = File.ReadAllBytes(imagePath);
            _mapTexture = new Texture2D(2, 2);
            _mapTexture.filterMode = FilterMode.Point;
            _mapTexture.LoadImage(imageData);

            MapDataManager.Instance.Dimensions = new Vector2(_mapTexture.width, _mapTexture.height);
        }
        return true;
    }
}
