using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the main container GUI
/// </summary>
public class ContainerManager : MonoBehaviour
{
    [SerializeField] private LoadImageManager _loadImage;
    [SerializeField] private OptionsManager _options;
    [SerializeField] private MapGUIManager _mapManager;
    [SerializeField] private GameObject _noContentPanel;
    [SerializeField] private GameObject _mapContentPanel;
    [SerializeField] private GameObject _messagePanel;
    [SerializeField] private GameObject _messagePrefab;

    private List<InfoMessage> _messages;

    private static readonly string PROCESSING_IMG_MSG_ID = "processing_img";
    private static readonly string ERROR_LOADING_MAP_MSG_ID = "error_loading_map";

    private void Awake()
    {
        _loadImage.UpdatedLoadingState += OnLoadingState_Changed;
        _messages = new List<InfoMessage>();
    }

    private void OnLoadingState_Changed(LoadImageManager.LoadingState state)
    {
        switch (state)
        {
            case LoadImageManager.LoadingState.NOT_LOADING:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                break;
            case LoadImageManager.LoadingState.LOADING:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                DestroyMessage(ERROR_LOADING_MAP_MSG_ID);
                CreateMessage("Processing Image...", PROCESSING_IMG_MSG_ID);
                break;
            case LoadImageManager.LoadingState.FAILED:
                _noContentPanel.SetActive(true);
                _mapContentPanel.SetActive(false);
                DestroyMessage(PROCESSING_IMG_MSG_ID);
                CreateMessage("Error while processing the image!", ERROR_LOADING_MAP_MSG_ID, 5f);
                break;
            case LoadImageManager.LoadingState.DONE:
                _mapContentPanel.SetActive(true);
                _noContentPanel.SetActive(false);
                _mapManager.SetMap(_loadImage.MapTexture);
                DestroyMessage(PROCESSING_IMG_MSG_ID);
                //if (_loadImage.IsOpen) _loadImage.ToggleGUI();
                //if (!_options.IsOpen) _options.ToggleGUI();
                break;
            default:
                throw new ArgumentOutOfRangeException("state", state, null);
        }
    }

    public void CreateMessage(string msg, string id, float livetime = -1f)
    {
        var newMsgObj = GameObject.Instantiate(_messagePrefab, _messagePanel.transform);
        var infoMsg = newMsgObj.GetComponent<InfoMessage>();
        _messages.Add(infoMsg);
        infoMsg.DestroyingMsg += On_DestroyingMsg;
        infoMsg.Setup(msg, id, livetime);

        if (_messages.Count >= 5)
            DestroyMessage(_messages[0].ID);
    }

    public void DestroyMessage(string id)
    {
        var messages = _messages.Where(m => m.ID == id).ToArray();
        foreach (var t in messages)
        {
            Destroy(t.gameObject);
        }
    }

    private void On_DestroyingMsg(string id)
    {
        _messages.RemoveAll(m => m.ID == id);
    }
}
