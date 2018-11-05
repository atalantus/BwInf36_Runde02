using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Manages the main container GUI
/// </summary>
public class ContainerManager : MonoBehaviour
{
    #region Properties

    private const string PROCESSING_IMG_MSG_ID = "processing_img";
    private const string ERROR_LOADING_MAP_MSG_ID = "error_loading_map";
    [SerializeField] private LoadImageManager _loadImage;
    [SerializeField] private GameObject _mapContentPanel;
    [SerializeField] private MapGUIManager _mapManager;
    [SerializeField] private GameObject _messagePanel;
    [SerializeField] private GameObject _messagePrefab;

    private List<InfoMessage> _messages;
    [SerializeField] private GameObject _noContentPanel;

    #endregion

    #region Methods

    private void Awake()
    {
        _loadImage.UpdatedLoadingState += OnLoadingState_Changed;
        _messages = new List<InfoMessage>();
    }

    private void OnLoadingState_Changed(LoadImageManager.LoadingState state)
    {
        switch (state)
        {
            case LoadImageManager.LoadingState.NotLoading:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                break;
            case LoadImageManager.LoadingState.Loading:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                DestroyMessage(ERROR_LOADING_MAP_MSG_ID);
                CreateMessage("Processing Image...", PROCESSING_IMG_MSG_ID, true);
                break;
            case LoadImageManager.LoadingState.Failed:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                DestroyMessage(PROCESSING_IMG_MSG_ID);
                CreateMessage("Error while processing the image!", ERROR_LOADING_MAP_MSG_ID, false, 5f);
                break;
            case LoadImageManager.LoadingState.Done:
                _mapContentPanel.SetActive(true);
                _noContentPanel.SetActive(false);
                _mapManager.SetMap(_loadImage.MapTexture);
                DestroyMessage(PROCESSING_IMG_MSG_ID);
                break;
            default:
                throw new ArgumentOutOfRangeException("state", state, null);
        }
    }

    public void CreateMessage(string msg, string id, bool spinnerIcon = false, float livetime = -1f)
    {
        var newMsgObj = Instantiate(_messagePrefab, _messagePanel.transform);
        var infoMsg = newMsgObj.GetComponent<InfoMessage>();
        _messages.Add(infoMsg);
        infoMsg.DestroyingMsg += On_DestroyingMsg;
        infoMsg.Setup(msg, id, spinnerIcon, livetime);

        if (_messages.Count >= 5)
            DestroyMessage(_messages[0].Id);
    }

    public void DestroyMessage(string id)
    {
        var messages = _messages.Where(m => m.Id == id).ToArray();
        foreach (var t in messages) Destroy(t.gameObject);
    }

    private void On_DestroyingMsg(string id)
    {
        _messages.RemoveAll(m => m.Id == id);
    }

    #endregion
}