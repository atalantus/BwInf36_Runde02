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

    private Animator _animator;
    private Texture2D _mapTexture;
    [SerializeField] private Text _filePathText;
    public Texture2D MapTexture
    {
        get { return _mapTexture; }
    }
    public event LoadingMapEventHandler UpdatedLoadingState;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        else
        {
            _filePathText.text = "<b>No File Loaded!</b>";
            if (UpdatedLoadingState != null)
                UpdatedLoadingState.Invoke(LoadingState.NOT_LOADING);
        }
#else
        Debug.LogWarning("File Browser NOT installed!");
#endif
    }

    public void TogglePanel()
    {
        _animator.SetTrigger("Toggle");
    }

    private bool ProcessImage(string imagePath)
    {
        if (UpdatedLoadingState != null)
            UpdatedLoadingState.Invoke(LoadingState.LOADING);

        // TODO: Process Image
        // Check if it's a valid map (only black, white, green and red pixels)

        byte[] imageData;

        if (File.Exists(imagePath))
        {
            imageData = File.ReadAllBytes(imagePath);
            _mapTexture = new Texture2D(2, 2);
            _mapTexture.LoadImage(imageData);
        }
        return true;
    }
}
