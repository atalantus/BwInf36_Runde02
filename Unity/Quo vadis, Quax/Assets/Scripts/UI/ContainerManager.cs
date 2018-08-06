using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the main container GUI
/// </summary>
public class ContainerManager : MonoBehaviour
{
    [SerializeField] private LoadImageManager _loadImage;
    [SerializeField] private MapGUIManager _mapManager;
    [SerializeField] private GameObject _noContentPanel;
    [SerializeField] private GameObject _mapContentPanel;
    [SerializeField] private Text _noContentText;

    private void Awake()
    {
        _loadImage.UpdatedLoadingState += OnLoadingState_Changed;
    }

    private void OnLoadingState_Changed(LoadImageManager.LoadingState state)
    {
        switch (state)
        {
            case LoadImageManager.LoadingState.NOT_LOADING:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                _noContentText.text = "NO MAP LOADED!";
                break;
            case LoadImageManager.LoadingState.LOADING:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                _noContentText.text = "PROCESSING IMAGE...";
                break;
            case LoadImageManager.LoadingState.FAILED:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                _noContentText.text = "FAILED LOADING THE MAP!";
                break;
            case LoadImageManager.LoadingState.DONE:
                _mapContentPanel.SetActive(true);
                _noContentPanel.SetActive(false);
                _mapManager.SetMap(_loadImage.MapTexture);
                break;
            default:
                throw new ArgumentOutOfRangeException("state", state, null);
        }
    }
}
