using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Text _filePathText;
    public event LoadingMapEventHandler UpdatedLoadingState;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenFile()
    {
#if CT_FB
        var path = FileBrowser.OpenSingleFile("Open Quax Map", "", "png");
        if (path != string.Empty)
        {
            if (ProcessImage(path))
            {
                _filePathText.text = path;
                if (UpdatedLoadingState != null)
                    UpdatedLoadingState.Invoke(LoadingState.LOADING);
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
        // TODO: Process Image
        return true;
    }
}
