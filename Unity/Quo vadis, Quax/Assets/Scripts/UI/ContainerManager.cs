using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerManager : MonoBehaviour
{
    [SerializeField] private LoadImage _loadImage;
    [SerializeField] private MapGUIManager _mapManager;
    [SerializeField] private GameObject _noContentPanel;
    [SerializeField] private GameObject _mapContentPanel;
    [SerializeField] private Text _noContentText;

    private void Awake()
    {
        _loadImage.UpdatedLoadingState += OnLoadingState_Changed;
    }

    private void OnLoadingState_Changed(LoadImage.LoadingState state)
    {
        switch (state)
        {
            case LoadImage.LoadingState.NOT_LOADING:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                _noContentText.text = "NO MAP LOADED!";
                break;
            case LoadImage.LoadingState.LOADING:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                _noContentText.text = "PROCESSING IMAGE...";
                break;
            case LoadImage.LoadingState.FAILED:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                _noContentText.text = "FAILED LOADING THE MAP!";
                break;
            case LoadImage.LoadingState.DONE:
                _mapContentPanel.SetActive(true);
                _noContentPanel.SetActive(false);
                _mapManager.SetMap(_loadImage.MapTexture);
                break;
            default:
                throw new ArgumentOutOfRangeException("state", state, null);
        }
    }
}
